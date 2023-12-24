using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCtrl : MonoBehaviour
{
    [Header("���ʖ�(��)")]
    [SerializeField] private string csvfile_name;

    [Header("���ʐ����ꏊ")]
    [SerializeField] private GameObject generate_pnt;
    [Header("�m�[�g����ꏊ")]
    [SerializeField] private GameObject judge_pnt;
    [Header("�ʏ�m�[�g")]
    [SerializeField] private GameObject generalNote_obj;

    [SerializeField] private GameCtrl gameCtrl;

    private ReadScoreData readScore;
    private GameManager g_manager;
    private List<NotesBlock> score_data;    //���ʃf�[�^
    private GameObject note_par;            //�m�[�g�e
    private Vector3 generate_pos;           //�m�[�g�o��pos
    private Vector3 judge_pos;              //�m�[�g����pos
    private bool isGenerateComp;            //���ʐ��������H
    private bool isPlaying;                 //�v���C���H
    private float note_generate_time;       //�m�[�g������O����
    private float game_time;                //�o�ߎ���

    void Start()
    {
        note_par = new GameObject("Note_par");
    }

    void Update()
    {
        //���ʂ̓ǂݍ��݂Ɛ�s����
        if(readScore && readScore.IsReturnScoreCompleted() && !isGenerateComp)
        { 
            SetScoreData();
            GenerateNoteInAdvance();
            gameCtrl.ScoreReadyComp();
        }
    }

    private void FixedUpdate()
    {
        if (isPlaying)
        {
            //�o�ߎ��Ԃ̉��Z
            game_time += Time.fixedDeltaTime;
            //�m�[�g�̃A�N�e�B�u��
            SetActiveNote();
        }
    }

    //������
    public void Init()
    {
        isPlaying = false;
        generate_pos = generate_pnt.transform.position;
        judge_pos = judge_pnt.transform.position;
        note_generate_time = Mathf.Abs(generate_pos.z - generate_pos.z) / g_manager.speed;
    }

    //����CSV�̓ǂݍ���
    public void ReadStart()
    {
        readScore = new ReadScoreData(csvfile_name);
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
        obj.AddComponent<Note>().Init(g_manager.speed);
        return obj;
    }

    //-------------------�Q�b�^�[-------------------

    public bool IsReturnScoreReady()
    {
        return isGenerateComp;
    }
}
