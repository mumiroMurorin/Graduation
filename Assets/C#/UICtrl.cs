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

    //--------------�{�^���n--------------

    //�y�ȃv���C�{�^��
    public void PushPlayButton()
    {
        gameCtrl.SetDataTrigger();
        gameCtrl.SetFileTrigger();
    }
}