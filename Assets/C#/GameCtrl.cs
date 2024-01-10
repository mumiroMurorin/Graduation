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
    const int SLASH_COLUMN = 4;
    const int BLOKEN_COLUMN = 5;
    const int JUDGE_COLUMN = 6;

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

    private List<MusicData> musicDataList;
    private GameManager g_manager;
    private int now_playing_index;

    private string musicFile_name;

    //ステップ系
    private bool isLoadGameData;
    private bool isFileGettingReady;
    private bool isDataGettingReady;
    private bool isPlayingGame;
    
    //トリガー系
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

        if (!g_manager.isConstruction)
        {
            musicFile_name = "try";
            SetFileTrigger();   //仮
            SetDataTrigger();   //仮
        }
        else
        {
            TransitionSelectTrigger();//仮
        }
    }

    void Update()
    {
        //ファイル準備
        if (isLoadGameData && isFileLoadTrigger)
        { SetFileStep(); }
        //データ準備(プレイ準備)
        else if(isFileGettingReady && scoreCtrl.IsReturnReadDataComp() && isDataPreparationTrigger)
        { SetDataStep(); }
        //ファイル準備完了？
        else if (!isReadyComp && isDataGettingReady && scoreCtrl.IsReturnScoreReady() && soundCtrl.IsReturnLoadComp()
            && directingCtrl.IsReturnLoadComp())
        { isReadyComp = true; }
        //楽曲プレイ
        else if (!isPlayingGame && isReadyComp && isGameStartTrigger)
        { StartStep(); }
        //楽曲終了
        else if(isPlayingGame && !scoreCtrl.IsReturnPlaying() && !directingCtrl.IsReturnPlaying())
        { FinishGameStep(); }
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
        scoreCtrl.ReadStart(musicFile_name);
        //音声系のファイルセット
        soundCtrl.Init();
        soundCtrl.ReadStart(musicFile_name);
        //演出系のファイルセット
        directingCtrl.Init();
        directingCtrl.ReadStart(musicFile_name, gameScene_obj.transform);

        //設定の設定
        g_manager.sword_effect_magni = musicDataList[now_playing_index].sword_effect_magni;
        g_manager.judge_correct_effect_magni = musicDataList[now_playing_index].judge_correct_effect_magni;
        g_manager.judgeUI_magni = musicDataList[now_playing_index].judgeUI_magni;

        sword_left_obj.SetActive(true);
        sword_right_obj.SetActive(true);
        stick_left_obj.SetActive(false);
        stick_right_obj.SetActive(false);

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

        SetActiveActionBox(true, false, false);

        isDataGettingReady = true;
        isPlayingGame = false;

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
        uiCtrl.SetResult(musicDataList[now_playing_index], scoreCtrl.ReturnResult());
        uiCtrl.AdventResultUI();    //リザルトUI出現

        SetActiveActionBox(false, true, true);
    }

    //セレクトに戻る処理
    private void BackSelectScene()
    {
        SetActiveActionBox(false, false, false);
        sword_left_obj.SetActive(false);
        sword_right_obj.SetActive(false);
        stick_left_obj.SetActive(true);
        stick_right_obj.SetActive(true);
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
                sword_effect_magni = float.Parse(csvDatas[i][SLASH_COLUMN]),
                judge_correct_effect_magni = float.Parse(csvDatas[i][BLOKEN_COLUMN]),
                judgeUI_magni = float.Parse(csvDatas[i][JUDGE_COLUMN])
            });

            //スプライト(サムネ)の読み込み
            Sprite sprite = null;
            Addressables.LoadAssetAsync<Sprite>(musicDataList[i - 1].file_name + "_thumbneil").Completed += op =>
            {
                sprite = Instantiate(op.Result);
                //Addressables.Release(op);
            };

            do
            {
                yield return null;
            } while (sprite == null);
            musicDataList[i - 1].thumbneil = sprite;

            //スプライト(サムネ)の読み込み
            AudioClip ac = null;
            Addressables.LoadAssetAsync<AudioClip>(musicDataList[i - 1].file_name + "_preview").Completed += op =>
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

    //--------------------トリガー、セット系--------------------

    //セレクトシーン遷移トリガー
    public void TransitionSelectTrigger()
    {
        gameScene_obj.SetActive(false);
        selectScene_obj.SetActive(true);
        BackSelectScene();
    }

    //ゲームシーン遷移トリガー
    public void TransitionGameTrigger()
    {
        gameScene_obj.SetActive(true);
        selectScene_obj.SetActive(false);
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
        now_playing_index = index;
        musicFile_name = musicDataList[index].file_name;
        //UIのセット
        uiCtrl.SelectMusicTopic(musicDataList[index]);
        soundCtrl.PlayPreview(musicDataList[index].preview);
    }
}
