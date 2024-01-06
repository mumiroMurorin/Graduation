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

    [Header("��ƒ��H")]
    [SerializeField] private bool isConstruction = false;

    [Header("MusicData��")]
    [SerializeField] private string musicData_name;

    [Header("�Z���N�g�V�[��(GameObject)")]
    [SerializeField] private GameObject selectScene_obj;

    [Header("�Q�[���V�[��(GameObject)")]
    [SerializeField] private GameObject gameScene_obj;

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
        TransitionSelectTrigger();//��

        if (!isConstruction)
        {
            musicFile_name = "try";
            SetFileTrigger();   //��
            SetDataTrigger();   //��
        }
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
    //(�y�ȁA���ʁA���o���̏���)
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

    //�y��(�Q�[��)�X�^�[�g
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
            {
                title = csvDatas[i][TITLE_COLUMN],
                composer = csvDatas[i][COMPOSER_COLUMN],
                file_name = csvDatas[i][FILENAME_COLUMN]
            });

            //�X�v���C�g(�T���l)�̓ǂݍ���
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

            //�X�v���C�g(�T���l)�̓ǂݍ���
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

        //��U�����ɔz�u
        for(int i = 0; i < musicDataList.Count; i++)
        {
            uiCtrl.AddMusicTopic(musicDataList[i], i);
        }

        isLoadGameData = true;
    }

    //���O�o��
    public void OutputLog(string str)
    {
        Debug.Log(str);
    }

    //--------------------�g���K�[�A�Z�b�g�n--------------------

    //�Z���N�g�V�[���J�ڃg���K�[
    public void TransitionSelectTrigger()
    {
        gameScene_obj.SetActive(false);
        selectScene_obj.SetActive(true);
    }

    //�Q�[���V�[���J�ڃg���K�[
    public void TransitionGameTrigger()
    {
        gameScene_obj.SetActive(true);
        selectScene_obj.SetActive(false);
    }

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

    //�v���C�y�ȃC���f�b�N�X�̃Z�b�g
    public void SetPlayMusicData(int index)
    {
        musicFile_name = musicDataList[index].file_name;
        //UI�̃Z�b�g
        uiCtrl.SelectMusicTopic(musicDataList[index]);
        soundCtrl.PlayPreview(musicDataList[index].preview);
    }
}
