using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICtrl : MonoBehaviour
{
    [Header("��ƒ��H")]
    [SerializeField] private bool isConstruction = false;
    [Header("���U���g�I�u�W�F�N�g")]
    [SerializeField] private GameObject result_obj;
    [SerializeField] private ComboCtrl comboCtrl;

    [Header("����e")]
    [SerializeField] private GameObject judge_par;
    [Header("�ォ�画��4��")]
    [SerializeField] private GameObject[] judge_obj_ori;

    [Header("�g�s�b�N�Ԋu")]
    [SerializeField] private float topic_interval;
    [Header("�g�s�b�N�e(Content)")]
    [SerializeField] private GameObject topicContent_obj;
    [Header("�g�s�b�N�u���b�N")]
    [SerializeField] private GameObject topicBlock_obj;
    [Header("�g�s�b�N���j�^(Image)")]
    [SerializeField] private Image[] topicMonitor_ima;

    [Header("�ȉ����U���g")]
    [SerializeField] private TextMeshProUGUI result_title;
    [SerializeField] private TextMeshProUGUI result_composer;
    [SerializeField] private TextMeshProUGUI result_pcritical_num;
    [SerializeField] private TextMeshProUGUI result_critical_num;
    [SerializeField] private TextMeshProUGUI result_hit_num;
    [SerializeField] private TextMeshProUGUI result_miss_num;
    [SerializeField] private TextMeshProUGUI result_score;
    [SerializeField] private TextMeshProUGUI result_rank;

    [SerializeField] private GameCtrl gameCtrl;

    private GameObject[] judge_obj;
    private int musicTopic_maxnum;
    private int selectTopic_num;

    void Start()
    {
        //��
        judge_obj = new GameObject[4];
        for(int i = 0;i < 4; i++)
        {
            judge_obj[i] = Instantiate(judge_obj_ori[i], judge_par.transform);
            judge_obj[i].SetActive(false);
        }

        if (isConstruction)
        {
            musicTopic_maxnum = 0;
        }
    }

    void Update()
    {
        
    }

    //�X�^�[�g�A���X�^�[�g���̏�����
    public void Init_Start()
    {
        result_obj.SetActive(false);//����
        comboCtrl.Init_Start();
    }

    //�y�ȊJ�n
    public void GameStart()
    {
        comboCtrl.GameStart();
    }

    //�R���{�ύX
    public void ChangeCombo(int combo)
    {
        comboCtrl.ChangeCombo(combo);
    }

    //����UI�̕\��
    public void AdventJudgeUI(int judgement_num, Vector3 pos)
    {
        GameObject g = Instantiate(judge_obj[judgement_num], pos, Quaternion.identity, judge_par.transform);
        g.SetActive(true);
    }

    //���U���g�̃Z�b�g
    public void SetResult(MusicData md, ResultData rd)
    {
        result_title.text = md.title;
        result_composer.text = md.composer;
        result_score.text = rd.score.ToString();
        result_rank.text = rd.rank;
        result_pcritical_num.text = rd.p_cri_num.ToString();
        result_critical_num.text = rd.cri_num.ToString();
        result_hit_num.text = rd.hit_num.ToString();
        result_miss_num.text = rd.miss_num.ToString();
    }

    //���U���g��ʂ̏o��
    public void AdventResultUI()
    {
        result_obj.SetActive(true);//��
    }

    //�y�ȃg�s�b�N�̒ǉ�
    public void AddMusicTopic(MusicData md, int num)
    {
        if (isConstruction)
        {
            GameObject g = Instantiate(topicBlock_obj, topicContent_obj.transform);
            g.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(musicTopic_maxnum * topic_interval, 0, 0);
            g.GetComponent<MusicTopic>().Init(md, num);
            musicTopic_maxnum++;
        }
    }

    //�y�ȃg�s�b�N�̑I��
    public void SelectMusicTopic(MusicData md)
    {
        for(int i = 0; i < topicMonitor_ima.Length; i++)
        {
            topicMonitor_ima[i].sprite = md.thumbneil;
        }
    }

    //--------------�{�^���n--------------

    //�y�ȃv���C�{�^��
    public void PushPlayButton()
    {
        gameCtrl.TransitionGameTrigger();
        gameCtrl.SetDataTrigger();
        gameCtrl.SetFileTrigger();
    }
}