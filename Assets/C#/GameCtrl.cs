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
    const int MUSIC_NAME_COLUMN = 3;
    const int PREVIEW_NAME_COLUMN = 4;
    const int DIRECTING_NAME_COLUMN = 5;
    const int SCORE_NAME_COLUMN = 6;
    const int THUMBNEIL_NAME_COLUMN = 7;
    const int SLASH_COLUMN = 8;
    const int BLOKEN_COLUMN = 9;
    const int JUDGE_COLUMN = 10;

    [Header("MusicData��")]
    [SerializeField] private string musicData_name;

    [Header("�Z���N�g�V�[��(GameObject)")]
    [SerializeField] private GameObject selectScene_obj;

    [Header("�Q�[���V�[��(GameObject)")]
    [SerializeField] private GameObject gameScene_obj;

    [Header("�y�ȃX�^�[�g�A�N�V�����{�b�N�X")]
    [SerializeField] private ActionBox musicStart_actionbox;
    [Header("���X�^�[�g�A�N�V�����{�b�N�X")]
    [SerializeField] private ActionBox reStart_actionbox;
    [Header("�o�b�N�A�N�V�����{�b�N�X")]
    [SerializeField] private ActionBox back_actionbox;
    [Header("�R���g���[��UI���C��")]
    [SerializeField] private LineRenderer ui_line;
    [Header("�E�X�e�B�b�N")]
    [SerializeField] private GameObject stick_right_obj;
    [Header("���X�e�B�b�N")]
    [SerializeField] private GameObject stick_left_obj;
    [Header("�E��")]
    [SerializeField] private GameObject sword_right_obj;
    [Header("����")]
    [SerializeField] private GameObject sword_left_obj;

    [SerializeField] private ScoreCtrl scoreCtrl;
    [SerializeField] private SoundCtrl soundCtrl;
    [SerializeField] private DirectingCtrl directingCtrl;
    [SerializeField] private UICtrl uiCtrl;
    [SerializeField] private SceneTransitionParticle particle;

    private List<MusicData> musicDataList;
    private MusicData now_md;
    private GameManager g_manager;

    //private string musicFile_name;

    //�X�e�b�v�n
    private bool isLoadGameData;
    private bool isFileGettingReady;
    private bool isDataGettingReady;
    private bool isPlayingGame;
    
    //�g���K�[�n
    private bool isTransMusicSceneTrigger;
    private bool isTransSelectSceneTrigger;
    private bool isFileLoadTrigger;
    private bool isDataPreparationTrigger;
    private bool isGameStartTrigger;
    private bool isBackSelectSceneTrigger;

    //�����n
    private bool isReadyComp;

    void Start()
    {
        g_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(LoadGameData(musicData_name));
        TransitionSelectTrigger();//��
    }

    void Update()
    {
        //�Q�[���V�[���J��
        if(isLoadGameData && isTransMusicSceneTrigger && particle.IsReturnHideScene()) 
        { TransitionMusicSceneStep(); }
        //�t�@�C������
        else if (isLoadGameData && isFileLoadTrigger)
        { SetFileStep(); }
        //�f�[�^����(�v���C����)
        else if(isFileGettingReady && scoreCtrl.IsReturnReadDataComp() && isDataPreparationTrigger)
        { SetDataStep(); }
        //�t�@�C�����������H
        else if (!isReadyComp && isDataGettingReady && scoreCtrl.IsReturnScoreReady() && soundCtrl.IsReturnLoadComp()
            && directingCtrl.IsReturnLoadComp())
        { 
            isReadyComp = true;
            particle.FinishParticle();
            StartCoroutine(DisActionBoxLater());
        }
        //�y�ȃv���C
        else if (!isPlayingGame && isReadyComp && isGameStartTrigger)
        { StartStep(); }
        //�y�ȏI��
        else if(isPlayingGame && !scoreCtrl.IsReturnPlaying() && !directingCtrl.IsReturnPlaying())
        { FinishGameStep(); }
        //�Z���N�g�V�[���J��
        else if(!isPlayingGame && isTransSelectSceneTrigger && particle.IsReturnHideScene())
        { TransitionSelectSceneStep(); }
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
        scoreCtrl.ReadStart(now_md.score_name ?? now_md.file_name);
        //�����n�̃t�@�C���Z�b�g
        soundCtrl.Init();
        soundCtrl.ReadStart(now_md.music_name ?? now_md.file_name);
        //���o�n�̃t�@�C���Z�b�g
        directingCtrl.Init();
        directingCtrl.ReadStart(now_md.directing_name ?? now_md.file_name, gameScene_obj.transform);

        //�ݒ�̐ݒ�
        g_manager.sword_effect_magni = now_md.sword_effect_magni;
        g_manager.judge_correct_effect_magni = now_md.judge_correct_effect_magni;
        g_manager.judgeUI_magni = now_md.judgeUI_magni;

        sword_right_obj.GetComponentInChildren<Sword>().SetSwordEffect(now_md.sword_effect_magni, now_md.sword_effect_magni);
        sword_left_obj.GetComponentInChildren<Sword>().SetSwordEffect(now_md.sword_effect_magni, now_md.sword_effect_magni);

        sword_left_obj.SetActive(true);
        sword_right_obj.SetActive(true);
        stick_left_obj.SetActive(false);
        stick_right_obj.SetActive(false);
        ui_line.enabled = false;

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

        isDataGettingReady = true;
        isPlayingGame = false;
        isReadyComp = false;

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

        SetActiveActionBox(false, false, false);

        isPlayingGame = true;
        isGameStartTrigger = false;
    }

    //�y��(�Q�[��)�I������
    private void FinishGameStep()
    {
        isPlayingGame = false;
        soundCtrl.StopMusic();      //�y�Ȓ�~(�t�F�[�h�A�E�g)
        uiCtrl.SetResult(now_md, scoreCtrl.ReturnResult());
        uiCtrl.AdventResultUI();    //���U���gUI�o��

        SetActiveActionBox(false, true, true);
    }

    //�y�ȃV�[���J�ڏ���
    private void TransitionMusicSceneStep()
    {
        SetDataTrigger();
        SetFileTrigger();

        gameScene_obj.SetActive(true);
        selectScene_obj.SetActive(false);
        isTransMusicSceneTrigger = false;
    }

    //�Z���N�g�ɖ߂鏈��
    private void TransitionSelectSceneStep()
    {
        gameScene_obj.SetActive(false);
        selectScene_obj.SetActive(true);
        sword_left_obj.SetActive(false);
        sword_right_obj.SetActive(false);
        stick_left_obj.SetActive(true);
        stick_right_obj.SetActive(true);
        ui_line.enabled = true;
        particle.FinishParticle();
        isTransSelectSceneTrigger = false;
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
                file_name = csvDatas[i][FILENAME_COLUMN],
                music_name = (csvDatas[i][MUSIC_NAME_COLUMN] == "" ? null : csvDatas[i][MUSIC_NAME_COLUMN]),
                preview_name = (csvDatas[i][PREVIEW_NAME_COLUMN] == "" ? null : csvDatas[i][PREVIEW_NAME_COLUMN]),
                directing_name = (csvDatas[i][DIRECTING_NAME_COLUMN] == "" ? null : csvDatas[i][DIRECTING_NAME_COLUMN]),
                score_name = (csvDatas[i][SCORE_NAME_COLUMN] == "" ? null : csvDatas[i][SCORE_NAME_COLUMN]),
                thumbneil_name = (csvDatas[i][THUMBNEIL_NAME_COLUMN] == "" ? null : csvDatas[i][THUMBNEIL_NAME_COLUMN]),
                sword_effect_magni = float.Parse(csvDatas[i][SLASH_COLUMN]),
                judge_correct_effect_magni = float.Parse(csvDatas[i][BLOKEN_COLUMN]),
                judgeUI_magni = float.Parse(csvDatas[i][JUDGE_COLUMN])
            });

            //�X�v���C�g(�T���l)�̓ǂݍ���
            Sprite sprite = null;
            string f_name = musicDataList[i - 1].thumbneil_name ?? musicDataList[i - 1].file_name;
            Addressables.LoadAssetAsync<Sprite>(f_name + "_thumbneil").Completed += op =>
            {
                sprite = Instantiate(op.Result);
                //Addressables.Release(op);
            };

            do
            {
                yield return null;
            } while (sprite == null);
            musicDataList[i - 1].thumbneil = sprite;

            //�y�ȃv���r���[�̓ǂݍ���
            AudioClip ac = null;
            f_name = musicDataList[i - 1].preview_name ?? musicDataList[i - 1].file_name;
            Addressables.LoadAssetAsync<AudioClip>(f_name + "_preview").Completed += op =>
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
        //�g�s�b�N�ǉ�
        for(int i = 0; i < musicDataList.Count; i++)
        {
            uiCtrl.AddMusicTopic(musicDataList[i], i);
        }

        isLoadGameData = true;
    }

    //�A�N�V�����{�b�N�X�̕\������
    private IEnumerator DisActionBoxLater()
    {
        do
        {
            yield return null;
        } while (particle.IsReturnPlaying());
        SetActiveActionBox(true, false, false);
    }

    /// <summary>
    /// �A�N�V�����{�b�N�X�̃A�N�e�B�u�֌W
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

    //���O�o��
    public void OutputLog(string str)
    {
        Debug.Log(str);
    }

    //���u��
    public void tmp()
    {
        now_md = musicDataList[1];
        TransitionGameTrigger();
    }

    //--------------------�g���K�[�A�Z�b�g�n--------------------

    //�Z���N�g�V�[���J�ڃg���K�[
    public void TransitionSelectTrigger()
    {
        isTransSelectSceneTrigger = true;
        SetActiveActionBox(false, false, false);
        particle.PlayParticle();
    }

    //�Q�[���V�[���J�ڃg���K�[
    public void TransitionGameTrigger()
    {
        isTransMusicSceneTrigger = true;
        particle.PlayParticle();
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
        now_md = musicDataList[index];
        //musicFile_name = musicDataList[index].file_name;
        //UI�̃Z�b�g
        uiCtrl.SelectMusicTopic(musicDataList[index]);
        soundCtrl.PlayPreview(musicDataList[index].preview);
    }
}
