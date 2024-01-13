using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.IO;
using UnityEngine;
using Common;

public class GameCtrl : MonoBehaviour
{
    const int TITLE_COLUMN = 0;
    const int COMPOSER_COLUMN = 1;
    const int FILENAME_COLUMN = 2;
    const int MUSIC_NAME_COLUMN = 3;
    const int PREVIEW_NAME_COLUMN = 4;
    const int DIRECTING_NAME_COLUMN = 5;
    const int SCORE_NAME_COLUMN = 6;
    const int THUMBNEIL_NAME_COLUMN = 7;
    const int SLASH_COLUMN = 8;
    const int BLOKEN_COLUMN = 9;
    const int JUDGE_COLUMN = 10;

    [Header("MusicData名")]
    [SerializeField] private string musicData_name;

    [Header("セレクトシーン(GameObject)")]
    [SerializeField] private GameObject selectScene_obj;

    [Header("ゲームシーン(GameObject)")]
    [SerializeField] private GameObject gameScene_obj;

    [Header("楽曲スタートアクションボックス")]
    [SerializeField] private ActionBox musicStart_actionbox;
    [Header("リスタートアクションボックス")]
    [SerializeField] private ActionBox reStart_actionbox;
    [Header("バックアクションボックス")]
    [SerializeField] private ActionBox back_actionbox;
    [Header("コントローラUIライン")]
    [SerializeField] private LineRenderer ui_line;
    [Header("右スティック")]
    [SerializeField] private GameObject stick_right_obj;
    [Header("左スティック")]
    [SerializeField] private GameObject stick_left_obj;
    [Header("右剣")]
    [SerializeField] private GameObject sword_right_obj;
    [Header("左剣")]
    [SerializeField] private GameObject sword_left_obj;

    [SerializeField] private ScoreCtrl scoreCtrl;
    [SerializeField] private SoundCtrl soundCtrl;
    [SerializeField] private DirectingCtrl directingCtrl;
    [SerializeField] private UICtrl uiCtrl;
    [SerializeField] private SceneTransitionParticle particle;

    private List<MusicData> musicDataList;
    private MusicData now_md;
    private GameManager g_manager;

    //private string musicFile_name;

    //ステップ系
    private bool isLoadGameData;
    private bool isFileGettingReady;
    private bool isDataGettingReady;
    private bool isPlayingGame;
    
    //トリガー系
    private bool isTransMusicSceneTrigger;
    private bool isTransSelectSceneTrigger;
    private bool isFileLoadTrigger;
    private bool isDataPreparationTrigger;
    private bool isGameStartTrigger;
    private bool isBackSelectSceneTrigger;

    //準備系
    private bool isReadyComp;

    void Start()
    {
        g_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(LoadGameData(musicData_name));
        TransitionSelectTrigger();//仮
    }

    void Update()
    {
        //ゲームシーン遷移
        if(isLoadGameData && isTransMusicSceneTrigger && particle.IsReturnHideScene()) 
        { TransitionMusicSceneStep(); }
        //ファイル準備
        else if (isLoadGameData && isFileLoadTrigger)
        { SetFileStep(); }
        //データ準備(プレイ準備)
        else if(isFileGettingReady && scoreCtrl.IsReturnReadDataComp() && isDataPreparationTrigger)
        { SetDataStep(); }
        //ファイル準備完了？
        else if (!isReadyComp && isDataGettingReady && scoreCtrl.IsReturnScoreReady() && soundCtrl.IsReturnLoadComp()
            && directingCtrl.IsReturnLoadComp())
        { 
            isReadyComp = true;
            particle.FinishParticle();
            StartCoroutine(DisActionBoxLater());
        }
        //楽曲プレイ
        else if (!isPlayingGame && isReadyComp && isGameStartTrigger)
        { StartStep(); }
        //楽曲終了
        else if(isPlayingGame && !scoreCtrl.IsReturnPlaying() && !directingCtrl.IsReturnPlaying())
        { FinishGameStep(); }
        //セレクトシーン遷移
        else if(!isPlayingGame && isTransSelectSceneTrigger && particle.IsReturnHideScene())
        { TransitionSelectSceneStep(); }
    }

    //初期化
    private void Init()
    {
        isFileGettingReady = false;
        isDataGettingReady = false;
        isReadyComp = false;
        isPlayingGame = false;
    }

    //楽曲決定(初選曲)時処理
    //(楽曲、譜面、演出他の準備)
    private void SetFileStep()
    {
        Init();
        //譜面系のファイルセット
        scoreCtrl.Init();
        scoreCtrl.ReadStart(now_md.score_name ?? now_md.file_name);
        //音声系のファイルセット
        soundCtrl.Init();
        soundCtrl.ReadStart(now_md.music_name ?? now_md.file_name);
        //演出系のファイルセット
        directingCtrl.Init();
        directingCtrl.ReadStart(now_md.directing_name ?? now_md.file_name, gameScene_obj.transform);

        //設定の設定
        g_manager.sword_effect_magni = now_md.sword_effect_magni;
        g_manager.judge_correct_effect_magni = now_md.judge_correct_effect_magni;
        g_manager.judgeUI_magni = now_md.judgeUI_magni;

        sword_right_obj.GetComponentInChildren<Sword>().SetSwordEffect(now_md.sword_effect_magni, now_md.sword_effect_magni);
        sword_left_obj.GetComponentInChildren<Sword>().SetSwordEffect(now_md.sword_effect_magni, now_md.sword_effect_magni);

        sword_left_obj.SetActive(true);
        sword_right_obj.SetActive(true);
        stick_left_obj.SetActive(false);
        stick_right_obj.SetActive(false);
        ui_line.enabled = false;

        isFileGettingReady = true;
        isDataGettingReady = false;
        isPlayingGame = false;

        isFileLoadTrigger = false;
    }

    //楽曲スタート、リスタート時処理
    //(譜面事前生成ほか)
    private void SetDataStep()
    {
        //譜面の初期化
        scoreCtrl.Init_Start();
        //楽曲の初期化
        soundCtrl.Init_Start();
        //演出の初期化
        directingCtrl.Init_Start();
        //UIの初期化
        uiCtrl.Init_Start();

        isDataGettingReady = true;
        isPlayingGame = false;
        isReadyComp = false;

        isDataPreparationTrigger = false;
    }

    //楽曲(ゲーム)スタート
    private void StartStep()
    {
        if (!isReadyComp) { return; }
        scoreCtrl.GameStart();  //譜面開始
        soundCtrl.PlayMusic();  //楽曲再生
        directingCtrl.PlayDirecting(); //演出再生
        uiCtrl.GameStart();     //色々表示

        SetActiveActionBox(false, false, false);

        isPlayingGame = true;
        isGameStartTrigger = false;
    }

    //楽曲(ゲーム)終了処理
    private void FinishGameStep()
    {
        isPlayingGame = false;
        soundCtrl.StopMusic();      //楽曲停止(フェードアウト)
        uiCtrl.SetResult(now_md, scoreCtrl.ReturnResult());
        uiCtrl.AdventResultUI();    //リザルトUI出現

        SetActiveActionBox(false, true, true);
    }

    //楽曲シーン遷移処理
    private void TransitionMusicSceneStep()
    {
        SetDataTrigger();
        SetFileTrigger();

        gameScene_obj.SetActive(true);
        selectScene_obj.SetActive(false);
        isTransMusicSceneTrigger = false;
    }

    //セレクトに戻る処理
    private void TransitionSelectSceneStep()
    {
        gameScene_obj.SetActive(false);
        selectScene_obj.SetActive(true);
        sword_left_obj.SetActive(false);
        sword_right_obj.SetActive(false);
        stick_left_obj.SetActive(true);
        stick_right_obj.SetActive(true);
        ui_line.enabled = true;
        particle.FinishParticle();
        isTransSelectSceneTrigger = false;
    }

    //楽曲データの読み込み
    private IEnumerator LoadGameData(string file_name)
    {
        TextAsset csvFile; // CSVファイル
        List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト

        //CSVデータの読み込み
        Addressables.LoadAssetAsync<TextAsset>(file_name).Completed += op =>
        {
            csvFile = op.Result;
            StringReader reader = new StringReader(csvFile.text);
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                csvDatas.Add(line.Split(','));
            }
            Addressables.Release(op);
        };

        do
        {
            yield return null;
        } while (csvDatas.Count == 0);

        //MusicDataListの作成
        musicDataList = new List<MusicData>();
        for(int i = 1;i < csvDatas.Count; i++)
        {
            musicDataList.Add(new MusicData
            {
                title = csvDatas[i][TITLE_COLUMN],
                composer = csvDatas[i][COMPOSER_COLUMN],
                file_name = csvDatas[i][FILENAME_COLUMN],
                music_name = (csvDatas[i][MUSIC_NAME_COLUMN] == "" ? null : csvDatas[i][MUSIC_NAME_COLUMN]),
                preview_name = (csvDatas[i][PREVIEW_NAME_COLUMN] == "" ? null : csvDatas[i][PREVIEW_NAME_COLUMN]),
                directing_name = (csvDatas[i][DIRECTING_NAME_COLUMN] == "" ? null : csvDatas[i][DIRECTING_NAME_COLUMN]),
                score_name = (csvDatas[i][SCORE_NAME_COLUMN] == "" ? null : csvDatas[i][SCORE_NAME_COLUMN]),
                thumbneil_name = (csvDatas[i][THUMBNEIL_NAME_COLUMN] == "" ? null : csvDatas[i][THUMBNEIL_NAME_COLUMN]),
                sword_effect_magni = float.Parse(csvDatas[i][SLASH_COLUMN]),
                judge_correct_effect_magni = float.Parse(csvDatas[i][BLOKEN_COLUMN]),
                judgeUI_magni = float.Parse(csvDatas[i][JUDGE_COLUMN])
            });

            //スプライト(サムネ)の読み込み
            Sprite sprite = null;
            string f_name = musicDataList[i - 1].thumbneil_name ?? musicDataList[i - 1].file_name;
            Addressables.LoadAssetAsync<Sprite>(f_name + "_thumbneil").Completed += op =>
            {
                sprite = Instantiate(op.Result);
                //Addressables.Release(op);
            };

            do
            {
                yield return null;
            } while (sprite == null);
            musicDataList[i - 1].thumbneil = sprite;

            //楽曲プレビューの読み込み
            AudioClip ac = null;
            f_name = musicDataList[i - 1].preview_name ?? musicDataList[i - 1].file_name;
            Addressables.LoadAssetAsync<AudioClip>(f_name + "_preview").Completed += op =>
            {
                ac = Instantiate(op.Result);
                //Addressables.Release(op);
            };

            do
            {
                yield return null;
            } while (ac == null);
            musicDataList[i - 1].preview = ac;
            musicDataList[i - 1].preview.LoadAudioData();
        }

        //一旦ここに配置
        //トピック追加
        for(int i = 0; i < musicDataList.Count; i++)
        {
            uiCtrl.AddMusicTopic(musicDataList[i], i);
        }

        isLoadGameData = true;
    }

    //アクションボックスの表示処理
    private IEnumerator DisActionBoxLater()
    {
        do
        {
            yield return null;
        } while (particle.IsReturnPlaying());
        SetActiveActionBox(true, false, false);
    }

    /// <summary>
    /// アクションボックスのアクティブ関係
    /// </summary>
    /// <param name="isStart"></param>
    /// <param name="isRestart"></param>
    /// <param name="isBack"></param>
    private void SetActiveActionBox(bool isStart, bool isRestart, bool isBack)
    {
        musicStart_actionbox.SetActiveBox(isStart);
        reStart_actionbox.SetActiveBox(isRestart);
        back_actionbox.SetActiveBox(isBack);
    }

    //ログ出力
    public void OutputLog(string str)
    {
        Debug.Log(str);
    }

    //仮置き
    public void tmp()
    {
        now_md = musicDataList[1];
        TransitionGameTrigger();
    }

    //--------------------トリガー、セット系--------------------

    //セレクトシーン遷移トリガー
    public void TransitionSelectTrigger()
    {
        isTransSelectSceneTrigger = true;
        SetActiveActionBox(false, false, false);
        particle.PlayParticle();
    }

    //ゲームシーン遷移トリガー
    public void TransitionGameTrigger()
    {
        isTransMusicSceneTrigger = true;
        particle.PlayParticle();
    }

    //ファイル読み込みトリガー
    public void SetFileTrigger()
    {
        isFileLoadTrigger = true;
    }

    //データ準備トリガー
    public void SetDataTrigger()
    {
        isDataPreparationTrigger = true;
    }

    //楽曲プレイトリガー
    public void PlayGameTrigger()
    {
        isGameStartTrigger = true;
    }

    //プレイ楽曲インデックスのセット
    public void SetPlayMusicData(int index)
    {
        now_md = musicDataList[index];
        //musicFile_name = musicDataList[index].file_name;
        //UIのセット
        uiCtrl.SelectMusicTopic(musicDataList[index]);
        soundCtrl.PlayPreview(musicDataList[index].preview);
    }
}
