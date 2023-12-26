using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    [SerializeField] private ScoreCtrl scoreCtrl;

    private bool isGettingReady;
    private bool isScoreReadyComp;
    private bool isReadyComp;
    private bool isPlayingGame;

    void Start()
    {
        
    }

    void Update()
    {
        //ファイル準備
        if (!isGettingReady) { SetFileTrigger(); }
        //ファイル準備完了？
        else if (!isReadyComp && isScoreReadyComp) 
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
        scoreCtrl.ReadStart();

        isGettingReady = true;
    }

    //ゲームスタートトリガー(ボタンかなにか)
    public void StartTrigger()
    {
        if (!isReadyComp) { return; }
        scoreCtrl.GameStart();
        isPlayingGame = true;
    }

    //譜面の準備完了
    public void ScoreReadyComp()
    {
        isScoreReadyComp = true;
    }
}
