using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICtrl : MonoBehaviour
{
    [Header("作業中？")]
    [SerializeField] private bool isConstruction = false;
    [Header("リザルトオブジェクト")]
    [SerializeField] private GameObject result_obj;
    [SerializeField] private ComboCtrl comboCtrl;

    [Header("判定親")]
    [SerializeField] private GameObject judge_par;
    [Header("上から判定4つ")]
    [SerializeField] private GameObject[] judge_obj_ori;

    [Header("トピック間隔")]
    [SerializeField] private float topic_interval;
    [Header("トピック親(Content)")]
    [SerializeField] private GameObject topicContent_obj;
    [Header("トピックブロック")]
    [SerializeField] private GameObject topicBlock_obj;

    [SerializeField] private GameCtrl gameCtrl;

    private GameObject[] judge_obj;
    private int musicTopic_maxnum;
    private int selectTopic_num;

    void Start()
    {
        //仮
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

    //スタート、リスタート時の初期化
    public void Init_Start()
    {
        result_obj.SetActive(false);//かり
        comboCtrl.Init_Start();
    }

    //楽曲開始
    public void GameStart()
    {
        comboCtrl.GameStart();
    }

    //コンボ変更
    public void ChangeCombo(int combo)
    {
        comboCtrl.ChangeCombo(combo);
    }

    //判定UIの表示
    public void AdventJudgeUI(int judgement_num, Vector3 pos)
    {
        GameObject g = Instantiate(judge_obj[judgement_num], pos, Quaternion.identity, judge_par.transform);
        g.SetActive(true);
    }

    //リザルト画面の出現
    public void AdventResultUI()
    {
        result_obj.SetActive(true);//仮
    }

    //楽曲トピックの追加
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

    //--------------ボタン系--------------

    //楽曲プレイボタン
    public void PushPlayButton()
    {
        gameCtrl.SetDataTrigger();
        gameCtrl.SetFileTrigger();
    }
}