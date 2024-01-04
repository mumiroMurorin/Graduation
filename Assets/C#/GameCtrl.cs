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

    [Header("MusicData名")]
    [SerializeField] private string musicData_name;

    [Header("楽曲スタートアクションボックス")]
    [SerializeField] private ActionBox musicStart_actionbox;
    [SerializeField] private ScoreCtrl scoreCtrl;
    [SerializeField] private SoundCtrl soundCtrl;
    [SerializeField] private DirectingCtrl directingCtrl;
    [SerializeField] private UICtrl uiCtrl;

    private List<MusicData> musicDataList;

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

    //準備系
    private bool isReadyComp;

    void Start()
    {
        StartCoroutine(LoadGameData(musicData_name));

        //SetFileTrigger();   //仮
        //SetDataTrigger();   //仮
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
    //(楽曲、譜面、演出他の準備トリガー)
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
        directingCtrl.ReadStart(musicFile_name);

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

        musicStart_actionbox.SetActiveBox(true);

        isDataGettingReady = true;
        isPlayingGame = false;

        isDataPreparationTrigger = false;
    }

    //楽曲(ゲーム)スタートトリガー(ボタンかなにか)
    private void StartStep()
    {
        if (!isReadyComp) { return; }
        scoreCtrl.GameStart();  //譜面開始
        soundCtrl.PlayMusic();  //楽曲再生
        directingCtrl.PlayDirecting(); //演出再生
        uiCtrl.GameStart();     //色々表示

        musicStart_actionbox.SetActiveBox(false);

        isPlayingGame = true;
        isGameStartTrigger = false;
    }

    //楽曲(ゲーム)終了処理
    private void FinishGameStep()
    {
        isPlayingGame = false;
        soundCtrl.StopMusic();      //楽曲停止(フェードアウト)
        uiCtrl.AdventResultUI();    //リザルトUI出現
    }

    //楽曲データの読み込み
    private IEnumerator LoadGameData(string file_name)
    {
        TextAsset csvFile; // CSVファイル
        List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト
        Sprite sprite = null;

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
            { title = csvDatas[i][TITLE_COLUMN], composer = csvDatas[i][COMPOSER_COLUMN], 
                file_name = csvDatas[i][FILENAME_COLUMN], thumbneil = sprite });
        }

        //スプライト(サムネ)の読み込み
        foreach (MusicData md in musicDataList)
        {
            Addressables.LoadAssetAsync<Sprite>(md.file_name + "_thumbneil").Completed += op =>
            {
                sprite = Instantiate(op.Result);
                //Addressables.Release(op);
            };

            do
            {
                yield return null;
            } while (sprite == null);
        }

        //一旦ここに配置
        foreach (MusicData md in musicDataList)
        {
            uiCtrl.AddMusicTopic(md);
        }

        isLoadGameData = true;
    }

    //--------------------トリガー系--------------------

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
}
