using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    [Header("�t�@�C����(��)")]
    [SerializeField] private string file_name;

    [SerializeField] private ScoreCtrl scoreCtrl;
    [SerializeField] private SoundCtrl soundCtrl;

    private bool isGettingReady;
    private bool isPlayingGame;

    //�����n
    private bool isReadyComp;


    void Start()
    {
        
    }

    void Update()
    {
        //�t�@�C������
        if (!isGettingReady) { SetFileTrigger(); }
        //�t�@�C�����������H
        else if (!isReadyComp && scoreCtrl.IsReturnScoreReady() && soundCtrl.IsReturnLoadComp()) 
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
        scoreCtrl.ReadStart(file_name);
        //�����n�̃t�@�C���Z�b�g
        soundCtrl.Init();
        soundCtrl.ReadStart(file_name);

        isGettingReady = true;
    }

    //�Q�[���X�^�[�g�g���K�[(�{�^�����Ȃɂ�)
    public void StartTrigger()
    {
        if (!isReadyComp) { return; }
        scoreCtrl.GameStart();  //���ʊJ�n
        soundCtrl.PlayMusic();  //�y�ȍĐ�
        isPlayingGame = true;
    }

}
