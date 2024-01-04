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

    [Header("MusicData��")]
    [SerializeField] private string musicData_name;

    [Header("�y�ȃX�^�[�g�A�N�V�����{�b�N�X")]
    [SerializeField] private ActionBox musicStart_actionbox;
    [SerializeField] private ScoreCtrl scoreCtrl;
    [SerializeField] private SoundCtrl soundCtrl;
    [SerializeField] private DirectingCtrl directingCtrl;
    [SerializeField] private UICtrl uiCtrl;

    private List<MusicData> musicDataList;

    private string musicFile_name;

    //�X�e�b�v�n
    private bool isLoadGameData;
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
        StartCoroutine(LoadGameData(musicData_name));

        //SetFileTrigger();   //��
        //SetDataTrigger();   //��
    }

    void Update()
    {
        //�t�@�C������
        if (isLoadGameData && isFileLoadTrigger) 
        { SetFileStep(); }
        //�f�[�^����(�v���C����)
        else if(isFileGettingReady && scoreCtrl.IsReturnReadDataComp() && isDataPreparationTrigger)
        { SetDataStep(); }
        //�t�@�C�����������H
        else if (!isReadyComp && isDataGettingReady && scoreCtrl.IsReturnScoreReady() && soundCtrl.IsReturnLoadComp()
            && directingCtrl.IsReturnLoadComp())
        { isReadyComp = true; }
        //�y�ȃv���C
        else if (!isPlayingGame && isReadyComp && isGameStartTrigger)
        { StartStep(); }
        //�y�ȏI��
        else if(isPlayingGame && !scoreCtrl.IsReturnPlaying() && !directingCtrl.IsReturnPlaying())
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
        scoreCtrl.ReadStart(musicFile_name);
        //�����n�̃t�@�C���Z�b�g
        soundCtrl.Init();
        soundCtrl.ReadStart(musicFile_name);
        //���o�n�̃t�@�C���Z�b�g
        directingCtrl.Init();
        directingCtrl.ReadStart(musicFile_name);

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
        //���o�̏�����
        directingCtrl.Init_Start();
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
        directingCtrl.PlayDirecting(); //���o�Đ�
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

    //�y�ȃf�[�^�̓ǂݍ���
    private IEnumerator LoadGameData(string file_name)
    {
        TextAsset csvFile; // CSV�t�@�C��
        List<string[]> csvDatas = new List<string[]>(); // CSV�̒��g�����郊�X�g
        Sprite sprite = null;

        //CSV�f�[�^�̓ǂݍ���
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

        //MusicDataList�̍쐬
        musicDataList = new List<MusicData>();
        for(int i = 1;i < csvDatas.Count; i++)
        {
            musicDataList.Add(new MusicData
            { title = csvDatas[i][TITLE_COLUMN], composer = csvDatas[i][COMPOSER_COLUMN], 
                file_name = csvDatas[i][FILENAME_COLUMN], thumbneil = sprite });
        }

        //�X�v���C�g(�T���l)�̓ǂݍ���
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

        //��U�����ɔz�u
        foreach (MusicData md in musicDataList)
        {
            uiCtrl.AddMusicTopic(md);
        }

        isLoadGameData = true;
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
