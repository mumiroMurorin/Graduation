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
    [Header("トピックモニタ(Image)")]
    [SerializeField] private Image[] topicMonitor_ima;

    [Header("以下リザルト")]
    [SerializeField] private TextMeshProUGUI result_title;
    [SerializeField] private TextMeshProUGUI result_composer;
    [SerializeField] private TextMeshProUGUI result_pcritical_num;
    [SerializeField] private TextMeshProUGUI result_critical_num;
    [SerializeField] private TextMeshProUGUI result_hit_num;
    [SerializeField] private TextMeshProUGUI result_miss_num;
    [SerializeField] private TextMeshProUGUI result_score;
    [SerializeField] private TextMeshProUGUI result_rank;

    [SerializeField] private GameCtrl gameCtrl;

    private GameManager gameManager;
    private GameObject[] judge_obj;
    private Vector3[] judgeUI_ori_size;
    private int musicTopic_maxnum;

    public GameObject tmp_obj;

    void Start()
    {
        //仮
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        judge_obj = new GameObject[judge_obj_ori.Length];
        judgeUI_ori_size = new Vector3[judge_obj_ori.Length];
        for (int i = 0; i < judge_obj.Length; i++)
        {
            judge_obj[i] = Instantiate(judge_obj_ori[i], judge_par.transform);
            judge_obj[i].SetActive(false);
            judgeUI_ori_size[i] = judge_obj[i].transform.Find("Canvas/Image").gameObject.GetComponent<RectTransform>().localScale;
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

        //判定の大きさセット
        for(int i = 0; i < judge_obj.Length; i++)
        {
            RectTransform rt = judge_obj[i].transform.Find("Canvas/Image").gameObject.GetComponent<RectTransform>();
            rt.localScale =
                new Vector3(gameManager.judgeUI_magni * judgeUI_ori_size[i].x, 
                gameManager.judgeUI_magni * judgeUI_ori_size[i].y, gameManager.judgeUI_magni * judgeUI_ori_size[i].z);
            Debug.Log(rt.localScale + ", " + gameManager.judgeUI_magni);
        }
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
        //g.transform.localScale = new Vector3(gameManager.judgeUI_magni, gameManager.judgeUI_magni, gameManager.judgeUI_magni);
        g.SetActive(true);
    }

    //リザルトのセット
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

    //楽曲トピックの選択
    public void SelectMusicTopic(MusicData md)
    {
        for(int i = 0; i < topicMonitor_ima.Length; i++)
        {
            topicMonitor_ima[i].sprite = md.thumbneil;
        }
    }

    //--------------ボタン系--------------

    //楽曲プレイボタン
    public void PushPlayButton()
    {
        gameCtrl.TransitionGameTrigger();
        gameCtrl.SetDataTrigger();
        gameCtrl.SetFileTrigger();
    }
}