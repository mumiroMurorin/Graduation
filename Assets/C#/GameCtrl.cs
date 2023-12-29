using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    [Header("�t�@�C����(��)")]
    [SerializeField] private string file_name;

    [Header("�y�ȃX�^�[�g�A�N�V�����{�b�N�X")]
    [SerializeField] private ActionBox musicStart_actionbox;
    [SerializeField] private ScoreCtrl scoreCtrl;
    [SerializeField] private SoundCtrl soundCtrl;
    [SerializeField] private UICtrl uiCtrl;

    //�X�e�b�v�n
    private bool isFileGettingReady;
    private bool isDataGettingReady;
    private bool isPlayingGame;
    
    //�g���K�[�n
    private bool isFileLoadTrigger;
    private bool isDataPreparationTrigger;
    private bool isGameStartTrigger;

    //�����n
    private bool isReadyComp;

    void Start()
    {
        SetFileTrigger();   //��
        SetDataTrigger();   //��
    }

    void Update()
    {
        //�t�@�C������
        if (isFileLoadTrigger) 
        { SetFileStep(); }
        //�f�[�^����(�v���C����)
        else if(isFileGettingReady && scoreCtrl.IsReturnReadDataComp() && isDataPreparationTrigger)
        { SetDataStep(); }
        //�t�@�C�����������H
        else if (!isReadyComp && isDataGettingReady && scoreCtrl.IsReturnScoreReady() && soundCtrl.IsReturnLoadComp())
        { isReadyComp = true; }
        //�y�ȃv���C
        else if (!isPlayingGame && isReadyComp && isGameStartTrigger)
        { StartStep(); }
        //�y�ȏI��
        else if(isPlayingGame && !scoreCtrl.IsReturnPlaying())
        { FinishGameStep(); }
    }

    //������
    private void Init()
    {
        isFileGettingReady = false;
        isDataGettingReady = false;
        isReadyComp = false;
        isPlayingGame = false;
    }

    //�y�Ȍ���(���I��)������
    //(�y�ȁA���ʁA���o���̏����g���K�[)
    private void SetFileStep()
    {
        Init();
        //���ʌn�̃t�@�C���Z�b�g
        scoreCtrl.Init();
        scoreCtrl.ReadStart(file_name);
        //�����n�̃t�@�C���Z�b�g
        soundCtrl.Init();
        soundCtrl.ReadStart(file_name);

        isFileGettingReady = true;
        isDataGettingReady = false;
        isPlayingGame = false;

        isFileLoadTrigger = false;
    }

    //�y�ȃX�^�[�g�A���X�^�[�g������
    //(���ʎ��O�����ق�)
    private void SetDataStep()
    {
        //���ʂ̏�����
        scoreCtrl.Init_Start();
        //�y�Ȃ̏�����
        soundCtrl.Init_Start();
        //UI�̏�����
        uiCtrl.Init_Start();

        musicStart_actionbox.SetActiveBox(true);

        isDataGettingReady = true;
        isPlayingGame = false;

        isDataPreparationTrigger = false;
    }

    //�y��(�Q�[��)�X�^�[�g�g���K�[(�{�^�����Ȃɂ�)
    private void StartStep()
    {
        if (!isReadyComp) { return; }
        scoreCtrl.GameStart();  //���ʊJ�n
        soundCtrl.PlayMusic();  //�y�ȍĐ�
        uiCtrl.GameStart();     //�F�X�\��

        musicStart_actionbox.SetActiveBox(false);

        isPlayingGame = true;
        isGameStartTrigger = false;
    }

    //�y��(�Q�[��)�I������
    private void FinishGameStep()
    {
        isPlayingGame = false;
        soundCtrl.StopMusic();      //�y�Ȓ�~(�t�F�[�h�A�E�g)
        uiCtrl.AdventResultUI();    //���U���gUI�o��
    }

    //--------------------�g���K�[�n--------------------

    //�t�@�C���ǂݍ��݃g���K�[
    public void SetFileTrigger()
    {
        isFileLoadTrigger = true;
    }

    //�f�[�^�����g���K�[
    public void SetDataTrigger()
    {
        isDataPreparationTrigger = true;
    }

    //�y�ȃv���C�g���K�[
    public void PlayGameTrigger()
    {
        isGameStartTrigger = true;
    }
}
