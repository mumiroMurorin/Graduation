using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ScoreCtrl : MonoBehaviour
{
    private const int MAX_SCORE = 1000000;
    [Header("���ʐ����ꏊ")]
    [SerializeField] private GameObject generate_pnt;
    [Header("�m�[�g����ꏊ")]
    [SerializeField] private GameObject judge_pnt;
    [Header("�~�X�\���ꏊ")]
    [SerializeField] private GameObject miss_pnt;
    [Header("�ʏ�m�[�g")]
    [SerializeField] private GameObject generalNote_obj;

    [Header("D�{�[�_�[")]
    [SerializeField] private int border_D;
    [Header("C�{�[�_�[")]
    [SerializeField] private int border_C;
    [Header("B�{�[�_�[")]
    [SerializeField] private int border_B;
    [Header("A�{�[�_�[")]
    [SerializeField] private int border_A;
    [Header("S�{�[�_�[")]
    [SerializeField] private int border_S;
    [Header("SS�{�[�_�[")]
    [SerializeField] private int border_SS;
    [Header("MAX�{�[�_�[")]
    [SerializeField] private int border_MAX;

    [SerializeField] private GameCtrl gameCtrl;
    [SerializeField] private UICtrl uiCtrl;

    private ScoreCtrl scoreCtrl;
    private ReadScoreData readScore;
    private GameManager g_manager;
    private List<NotesBlock> score_data;    //���ʃf�[�^
    private GameObject note_par;            //�m�[�g�e
    private Vector3 generate_pos;           //�m�[�g�o��pos
    private Vector3 judge_pos;              //�m�[�g����pos
    private Vector3 miss_pos;               //miss����\��pos
    private bool isReadDataComp;            //���ʓǂݍ��݊����H
    private bool isGenerateComp;            //���ʐ��������H
    private bool isPlaying;                 //�v���C���H
    private int max_combo_num;              //�S�m�[�c��
    private int scoreData_index;            //���ʃf�[�^�̃C���f�b�N�X
    private float note_generate_time;       //�m�[�g������O����
    private float game_time;                //�o�ߎ���

    private float score;
    private int combo;
    private int[] judges_num;
    private float[] add_score;

    void Start()
    {
        scoreCtrl = this.gameObject.GetComponent<ScoreCtrl>();
        note_par = new GameObject("Note_par");
        g_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        //���ʂ̓ǂݍ���
        if(readScore && readScore.IsReturnScoreCompleted() && !isReadDataComp)
        { 
            SetScoreData();
            isReadDataComp = true;
        }
        else if (isPlaying)
        {
            //���ʏI���̃`�F�b�N
            CheckScoreFinish();
        }
    }

    private void FixedUpdate()
    {
        if (isPlaying)
        {
            game_time += Time.fixedDeltaTime;   //�o�ߎ��Ԃ̉��Z
            SetActiveNote();                    //�m�[�g�̃A�N�e�B�u��
        }
    }

    //�X�^�[�g���̏�����
    public void Init()
    {
        generate_pos = generate_pnt.transform.position;
        judge_pos = judge_pnt.transform.position;
        miss_pos = miss_pnt.transform.position;
    }

    //�X�^�[�g�A���X�^�[�g���̏�����
    public void Init_Start()
    {
        //�e��ϐ��̏�����
        isPlaying = false;
        scoreData_index = 0;
        game_time = 0;
        note_generate_time = Mathf.Abs(generate_pos.z - judge_pos.z) / g_manager.speed;
        add_score[0] = (float)MAX_SCORE / max_combo_num;
        add_score[1] = (float)MAX_SCORE / max_combo_num * 0.9f;
        add_score[2] = (float)MAX_SCORE / max_combo_num * 0.5f;
        add_score[3] = 0;

        judges_num = new int[4];
        score = 0;
        combo = 0;
        
        //���ʂ̎��O����
        GenerateNoteInAdvance();
    }

    //����CSV�̓ǂݍ���
    public void ReadStart(string file_name)
    {
        //readScore = new ReadScoreData(csvfile_name);
        readScore = this.gameObject.AddComponent<ReadScoreData>();
        readScore.LoadScoreCSV(file_name);
    }

    //�Q�[���J�n�g���K�[
    public void GameStart()
    {
        isPlaying = true;
    }

    //���ʃt�@�C������
    private void SetScoreData()
    {
        score_data = readScore.ReturnScoreData();
        max_combo_num = readScore.ReturnNoteNum();
    }

    //���ʃN���X����m�[�g�𐶐�
    //(����m�[�c�o�������͂����ɒǋL)
    public void GenerateNoteInAdvance()
    {
        //�S�Ẵm�[�c�����O����
        for (int i = 0; i < score_data.Count; i++)
        {
            //�ʏ�m�[�c�̐���
            foreach (GeneralNote g in score_data[i].general_list)
            {
                g.obj = GenerateGeneralNote(generate_pos + g.pos, g.angle);
                g.obj.SetActive(false);
            }

            //�ȉ�����m�[�c�̒ǉ�


        }

        isGenerateComp = true;
    }

    //�Q�[���J�n��A���Ԃ�������m�[�g��L���ɂ���
    //(����m�[�c�o�������͂����ɒǋL)
    private void SetActiveNote()
    {
        //���ʐ����I��
        if (scoreData_index == score_data.Count) { return; }

        //DOPath�g���Ȃ炱�̊֐�
        //note.transform.DOPath(markerPositionArray, time, PathType.Linear)
        //    .SetLookAt(0.001f).SetEase(Ease.Linear);
        //���Ԃ̌v�Z
        //float time = Mathf.Abs(START_GROUND_Z - END_GROUND_Z) / speed;

        //�f�[�^��̎��Ԃ𒴉߂������J��Ԃ�
        while (scoreData_index < score_data.Count && 
            IsReturnOverNextTime(score_data[scoreData_index].time))
        {
            //�ʏ�m�[�c�̐���
            foreach (GeneralNote g in score_data[scoreData_index].general_list)
            {
                //g.obj.transform.position += ReturnLittleDistance(g.time);
                g.obj.SetActive(true);
            }

            //�ȉ�����m�[�c�̒ǉ�


            scoreData_index++;
        }
    }

    //���ݎ��Ԃ����ʃf�[�^�̎��̃f�[�^��Time�𒴂������ǂ����Ԃ�
    private bool IsReturnOverNextTime(float t)
    {
        if (t - note_generate_time <= game_time) { return true; }
        return false;
    }

    //�m�[�g�̐���
    private GameObject GenerateGeneralNote(Vector3 born_pos, float angle)
    {
        GameObject obj = Instantiate(generalNote_obj, born_pos, Quaternion.Euler(0, 0, angle), note_par.transform);
        obj.GetComponent<Note>().Init(g_manager.speed, scoreCtrl, g_manager.judge_correct_effect_magni);
        return obj;
    }

    //���ʏI���̔���
    private void CheckScoreFinish()
    {
        //�����̋L�q�����ǂ��낳��
        if(judges_num[0] + judges_num[1] + judges_num[2] + judges_num[3] == max_combo_num)
        {
            isPlaying = false;
            Debug.Log("���ʏI��");
        }
    }

    //-------------------�Q�b�^�[-------------------

    //���ʃt�@�C���ǂݍ��݂������������Ԃ�
    public bool IsReturnReadDataComp()
    {
        return isReadDataComp;
    }

    //���ʂ̏����������������Ԃ�
    public bool IsReturnScoreReady()
    {
        return isGenerateComp;
    }

    //���ʂ��Đ������Ԃ�
    public bool IsReturnPlaying()
    {
        return isPlaying;
    }

    //���U���g��Ԃ�
    public ResultData ReturnResult()
    {
        return new ResultData
        {
            score = (int)score,
            p_cri_num = judges_num[0],
            cri_num = judges_num[1],
            hit_num = judges_num[2],
            miss_num = judges_num[3],
            rank = ReturnScoreToRank((int)score)
        };
    }

    //�X�R�A���烉���N��Ԃ�
    public string ReturnScoreToRank(int score)
    {
        if(score >= border_MAX) { return "MAX"; }
        if(score >= border_SS) { return "SS"; }
        if(score >= border_S) { return "S"; }
        if(score >= border_A) { return "A"; }
        if(score >= border_B) { return "B"; }
        if(score >= border_C) { return "C"; }
        if(score >= border_D) { return "D"; }
        return "E";
    }

    //-------------------�Z�b�^�[-------------------

    //������m�[�c���瓾��(����U�S��p_critical)
    public void SetNoteJudge(int judgement_num, Vector3 pos)
    {
        //����ʕ���
        switch (judgement_num)
        {
            case GrovalConst.P_CRITICAL_NUMBER: //P_Critical
                combo++;
                break;
            case GrovalConst.CRITICAL_NUMBER: //Critical
                combo++;
                break;
            case GrovalConst.HIT_NUMBER: //Hit
                combo++;
                break;
            case GrovalConst.MISS_NUMBER: //Miss
                combo = 0;
                pos = miss_pos;
                break;
        }

        judges_num[judgement_num]++;
        score += add_score[judgement_num];
        uiCtrl.ChangeCombo(combo);
        uiCtrl.AdventJudgeUI(judgement_num, pos);
    }
}
