using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    [Header("ファイル名(仮)")]
    [SerializeField] private string file_name;

    [Header("楽曲スタートアクションボックス")]
    [SerializeField] private ActionBox musicStart_actionbox;
    [SerializeField] private ScoreCtrl scoreCtrl;
    [SerializeField] private SoundCtrl soundCtrl;
    [SerializeField] private UICtrl uiCtrl;

    //ステップ系
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
        SetFileTrigger();   //仮
        SetDataTrigger();   //仮
    }

    void Update()
    {
        //ファイル準備
        if (isFileLoadTrigger) 
        { SetFileStep(); }
        //データ準備(プレイ準備)
        else if(isFileGettingReady && scoreCtrl.IsReturnReadDataComp() && isDataPreparationTrigger)
        { SetDataStep(); }
        //ファイル準備完了？
        else if (!isReadyComp && isDataGettingReady && scoreCtrl.IsReturnScoreReady() && soundCtrl.IsReturnLoadComp())
        { isReadyComp = true; }
        //楽曲プレイ
        else if (!isPlayingGame && isReadyComp && isGameStartTrigger)
        { StartStep(); }
        //楽曲終了
        else if(isPlayingGame && !scoreCtrl.IsReturnPlaying())
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
        scoreCtrl.ReadStart(file_name);
        //音声系のファイルセット
        soundCtrl.Init();
        soundCtrl.ReadStart(file_name);

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
