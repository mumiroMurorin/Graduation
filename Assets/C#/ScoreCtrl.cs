using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCtrl : MonoBehaviour
{
    [Header("譜面生成場所")]
    [SerializeField] private GameObject generate_pnt;
    [Header("ノート判定場所")]
    [SerializeField] private GameObject judge_pnt;
    [Header("通常ノート")]
    [SerializeField] private GameObject generalNote_obj;

    [SerializeField] private GameCtrl gameCtrl;

    private ScoreCtrl scoreCtrl;
    private ReadScoreData readScore;
    private GameManager g_manager;
    private List<NotesBlock> score_data;    //譜面データ
    private GameObject note_par;            //ノート親
    private Vector3 generate_pos;           //ノート出現pos
    private Vector3 judge_pos;              //ノート判定pos
    private bool isGenerateComp;            //譜面生成官僚？
    private bool isPlaying;                 //プレイ中？
    private int max_combo_num;              //全ノーツ数
    private float note_generate_time;       //ノート生成手前時間
    private float game_time;                //経過時間

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
        //譜面の読み込みと先行生成
        if(readScore && readScore.IsReturnScoreCompleted() && !isGenerateComp)
        { 
            SetScoreData();
            //isGenetrateCompは以下の関数でtrueになる
            GenerateNoteInAdvance();
        }
        else if (isPlaying)
        {
            CheckScoreFinish();                 //譜面終了のチェック
        }
    }

    private void FixedUpdate()
    {
        if (isPlaying)
        {
            game_time += Time.fixedDeltaTime;   //経過時間の加算
            SetActiveNote();                    //ノートのアクティブ化
        }
    }

    //初期化
    public void Init()
    {
        isPlaying = false;
        generate_pos = generate_pnt.transform.position;
        judge_pos = judge_pnt.transform.position;
        note_generate_time = Mathf.Abs(generate_pos.z - judge_pos.z) / g_manager.speed;
    }

    //判定の初期化
    private void JudgementInit()
    {
        p_critical_num = 0;
        critical_num = 0;
        hit_num = 0;
        miss_num = 0;
        score = 0;
        combo = 0;
    }

    //譜面CSVの読み込み
    public void ReadStart(string file_name)
    {
        //readScore = new ReadScoreData(csvfile_name);
        readScore = this.gameObject.AddComponent<ReadScoreData>();
        readScore.LoadScoreCSV(file_name);
    }

    //ゲーム開始トリガー
    public void GameStart()
    {
        isPlaying = true;
    }

    //譜面ファイルを代入
    private void SetScoreData()
    {
        score_data = readScore.ReturnScoreData();
        max_combo_num = readScore.ReturnNoteNum();
    }

    //譜面クラスからノートを生成
    public void GenerateNoteInAdvance()
    {
        //全てのノーツを事前生成
        for (int i = 0; i < score_data.Count; i++)
        {
            //通常ノーツの生成
            foreach (GeneralNote g in score_data[i].general_list)
            {
                g.obj = GenerateGeneralNote(generate_pos + g.pos, g.angle);
                g.obj.SetActive(false);
            }
        }

        isGenerateComp = true;
    }

    //ゲーム開始後、時間が来たらノートを有効にする
    private void SetActiveNote()
    {
        //譜面生成終了
        if (score_data.Count == 0) { return; }

        //DOPath使うならこの関数
        //note.transform.DOPath(markerPositionArray, time, PathType.Linear)
        //    .SetLookAt(0.001f).SetEase(Ease.Linear);
        //時間の計算
        //float time = Mathf.Abs(START_GROUND_Z - END_GROUND_Z) / speed;

        //データ上の時間を超過する限り繰り返す
        while (score_data.Count != 0 && IsReturnOverNextTime(score_data[0].time))
        {
            //通常ノーツの生成
            foreach (GeneralNote g in score_data[0].general_list)
            {
                //g.obj.transform.position += ReturnLittleDistance(g.time);
                g.obj.SetActive(true);
            }

            score_data.RemoveAt(0);
        }
    }

    //現在時間が譜面データの次のデータのTimeを超えたかどうか返す
    private bool IsReturnOverNextTime(float t)
    {
        if (t - note_generate_time <= game_time) { return true; }
        return false;
    }

    //ノートの生成
    private GameObject GenerateGeneralNote(Vector3 born_pos, float angle)
    {
        GameObject obj = Instantiate(generalNote_obj, born_pos, Quaternion.Euler(0, 0, angle), note_par.transform);
        obj.GetComponent<Note>().Init(g_manager.speed, scoreCtrl);
        return obj;
    }

    //譜面終了の判定
    private void CheckScoreFinish()
    {
        if(p_critical_num + critical_num + hit_num + miss_num == max_combo_num)
        {
            isPlaying = false;
            Debug.Log("譜面終了");
        }
    }

    //-------------------ゲッター-------------------

    //譜面の準備が完了したか返す
    public bool IsReturnScoreReady()
    {
        return isGenerateComp;
    }

    //-------------------セッター-------------------

    //判定をノーツから得る(※一旦全部p_critical)
    public void SetNoteJudge()
    {
        p_critical_num++;
    }
}
