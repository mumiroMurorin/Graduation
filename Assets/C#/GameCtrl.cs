using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    [Header("ファイル名(仮)")]
    [SerializeField] private string file_name;

    [SerializeField] private ScoreCtrl scoreCtrl;
    [SerializeField] private SoundCtrl soundCtrl;

    private bool isGettingReady;
    private bool isPlayingGame;

    //準備系
    private bool isReadyComp;


    void Start()
    {
        
    }

    void Update()
    {
        //ファイル準備
        if (!isGettingReady) { SetFileTrigger(); }
        //ファイル準備完了？
        else if (!isReadyComp && scoreCtrl.IsReturnScoreReady() && soundCtrl.IsReturnLoadComp()) 
        { 
            isReadyComp = true;
        }
    }

    //初期化
    private void Init()
    {
        isGettingReady = false;
        isReadyComp = false;
        isPlayingGame = false;
    }

    //楽曲、譜面、演出他の準備トリガー
    private void SetFileTrigger()
    {
        //譜面系のファイルセット
        scoreCtrl.Init();
        scoreCtrl.ReadStart(file_name);
        //音声系のファイルセット
        soundCtrl.Init();
        soundCtrl.ReadStart(file_name);

        isGettingReady = true;
    }

    //ゲームスタートトリガー(ボタンかなにか)
    public void StartTrigger()
    {
        if (!isReadyComp) { return; }
        scoreCtrl.GameStart();  //譜面開始
        soundCtrl.PlayMusic();  //楽曲再生
        isPlayingGame = true;
    }

}
