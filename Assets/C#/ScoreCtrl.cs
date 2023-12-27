using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCtrl : MonoBehaviour
{
    [Header("���ʐ����ꏊ")]
    [SerializeField] private GameObject generate_pnt;
    [Header("�m�[�g����ꏊ")]
    [SerializeField] private GameObject judge_pnt;
    [Header("�ʏ�m�[�g")]
    [SerializeField] private GameObject generalNote_obj;

    [SerializeField] private GameCtrl gameCtrl;

    private ScoreCtrl scoreCtrl;
    private ReadScoreData readScore;
    private GameManager g_manager;
    private List<NotesBlock> score_data;    //���ʃf�[�^
    private GameObject note_par;            //�m�[�g�e
    private Vector3 generate_pos;           //�m�[�g�o��pos
    private Vector3 judge_pos;              //�m�[�g����pos
    private bool isGenerateComp;            //���ʐ��������H
    private bool isPlaying;                 //�v���C���H
    private int max_combo_num;              //�S�m�[�c��
    private float note_generate_time;       //�m�[�g������O����
    private float game_time;                //�o�ߎ���

    private float score;
    private int combo;
    private int p_critical_num;
    private int critical_num;
    private int hit_num;
    private int miss_num;

    void Start()
    {
        scoreCtrl = this.gameObject.GetComponent<ScoreCtrl>();
        note_par = new GameObject("Note_par");
        g_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        //���ʂ̓ǂݍ��݂Ɛ�s����
        if(readScore && readScore.IsReturnScoreCompleted() && !isGenerateComp)
        { 
            SetScoreData();
            //isGenetrateComp�͈ȉ��̊֐���true�ɂȂ�
            GenerateNoteInAdvance();
        }
        else if (isPlaying)
        {
            CheckScoreFinish();                 //���ʏI���̃`�F�b�N
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

    //������
    public void Init()
    {
        isPlaying = false;
        generate_pos = generate_pnt.transform.position;
        judge_pos = judge_pnt.transform.position;
        note_generate_time = Mathf.Abs(generate_pos.z - judge_pos.z) / g_manager.speed;
    }

    //����̏�����
    private void JudgementInit()
    {
        p_critical_num = 0;
        critical_num = 0;
        hit_num = 0;
        miss_num = 0;
        score = 0;
        combo = 0;
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
        }

        isGenerateComp = true;
    }

    //�Q�[���J�n��A���Ԃ�������m�[�g��L���ɂ���
    private void SetActiveNote()
    {
        //���ʐ����I��
        if (score_data.Count == 0) { return; }

        //DOPath�g���Ȃ炱�̊֐�
        //note.transform.DOPath(markerPositionArray, time, PathType.Linear)
        //    .SetLookAt(0.001f).SetEase(Ease.Linear);
        //���Ԃ̌v�Z
        //float time = Mathf.Abs(START_GROUND_Z - END_GROUND_Z) / speed;

        //�f�[�^��̎��Ԃ𒴉߂������J��Ԃ�
        while (score_data.Count != 0 && IsReturnOverNextTime(score_data[0].time))
        {
            //�ʏ�m�[�c�̐���
            foreach (GeneralNote g in score_data[0].general_list)
            {
                //g.obj.transform.position += ReturnLittleDistance(g.time);
                g.obj.SetActive(true);
            }

            score_data.RemoveAt(0);
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
        obj.GetComponent<Note>().Init(g_manager.speed, scoreCtrl);
        return obj;
    }

    //���ʏI���̔���
    private void CheckScoreFinish()
    {
        if(p_critical_num + critical_num + hit_num + miss_num == max_combo_num)
        {
            isPlaying = false;
            Debug.Log("���ʏI��");
        }
    }

    //-------------------�Q�b�^�[-------------------

    //���ʂ̏����������������Ԃ�
    public bool IsReturnScoreReady()
    {
        return isGenerateComp;
    }

    //-------------------�Z�b�^�[-------------------

    //������m�[�c���瓾��(����U�S��p_critical)
    public void SetNoteJudge()
    {
        p_critical_num++;
    }
}
