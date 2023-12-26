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
        //�t�@�C������
        if (!isGettingReady) { SetFileTrigger(); }
        //�t�@�C�����������H
        else if (!isReadyComp && isScoreReadyComp) 
        { 
            isReadyComp = true;
        }
    }

    //������
    private void Init()
    {
        isGettingReady = false;
        isReadyComp = false;
        isPlayingGame = false;
    }

    //�y�ȁA���ʁA���o���̏����g���K�[
    private void SetFileTrigger()
    {
        //���ʌn�̃t�@�C���Z�b�g
        scoreCtrl.Init();
        scoreCtrl.ReadStart();

        isGettingReady = true;
    }

    //�Q�[���X�^�[�g�g���K�[(�{�^�����Ȃɂ�)
    public void StartTrigger()
    {
        if (!isReadyComp) { return; }
        scoreCtrl.GameStart();
        isPlayingGame = true;
    }

    //���ʂ̏�������
    public void ScoreReadyComp()
    {
        isScoreReadyComp = true;
    }
}
