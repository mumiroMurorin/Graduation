using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ScoreCtrl : MonoBehaviour
{
    private const int MAX_SCORE = 1000000;
    [Header("譜面生成場所")]
    [SerializeField] private GameObject generate_pnt;
    [Header("ノート判定場所")]
    [SerializeField] private GameObject judge_pnt;
    [Header("ミス表示場所")]
    [SerializeField] private GameObject miss_pnt;
    [Header("通常ノート")]
    [SerializeField] private GameObject generalNote_obj;

    [Header("Dボーダー")]
    [SerializeField] private int border_D;
    [Header("Cボーダー")]
    [SerializeField] private int border_C;
    [Header("Bボーダー")]
    [SerializeField] private int border_B;
    [Header("Aボーダー")]
    [SerializeField] private int border_A;
    [Header("Sボーダー")]
    [SerializeField] private int border_S;
    [Header("SSボーダー")]
    [SerializeField] private int border_SS;
    [Header("MAXボーダー")]
    [SerializeField] private int border_MAX;

    [SerializeField] private GameCtrl gameCtrl;
    [SerializeField] private UICtrl uiCtrl;

    private ScoreCtrl scoreCtrl;
    private ReadScoreData readScore;
    private GameManager g_manager;
    private List<NotesBlock> score_data;    //譜面データ
    private GameObject note_par;            //ノート親
    private Vector3 generate_pos;           //ノート出現pos
    private Vector3 judge_pos;              //ノート判定pos
    private Vector3 miss_pos;               //miss判定表示pos
    private bool isReadDataComp;            //譜面読み込み官僚？
    private bool isGenerateComp;            //譜面生成官僚？
    private bool isPlaying;                 //プレイ中？
    private int max_combo_num;              //全ノーツ数
    private int scoreData_index;            //譜面データのインデックス
    private float note_generate_time;       //ノート生成手前時間
    private float game_time;                //経過時間

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
        //譜面の読み込み
        if(readScore && readScore.IsReturnScoreCompleted() && !isReadDataComp)
        { 
            SetScoreData();
            isReadDataComp = true;
        }
        else if (isPlaying)
        {
            //譜面終了のチェック
            CheckScoreFinish();
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

    //スタート時の初期化
    public void Init()
    {
        generate_pos = generate_pnt.transform.position;
        judge_pos = judge_pnt.transform.position;
        miss_pos = miss_pnt.transform.position;
    }

    //スタート、リスタート時の初期化
    public void Init_Start()
    {
        //各種変数の初期化
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
        
        //譜面の事前生成
        GenerateNoteInAdvance();
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
    //(特殊ノーツ出現処理はここに追記)
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

            //以下特殊ノーツの追加


        }

        isGenerateComp = true;
    }

    //ゲーム開始後、時間が来たらノートを有効にする
    //(特殊ノーツ出現処理はここに追記)
    private void SetActiveNote()
    {
        //譜面生成終了
        if (scoreData_index == score_data.Count) { return; }

        //DOPath使うならこの関数
        //note.transform.DOPath(markerPositionArray, time, PathType.Linear)
        //    .SetLookAt(0.001f).SetEase(Ease.Linear);
        //時間の計算
        //float time = Mathf.Abs(START_GROUND_Z - END_GROUND_Z) / speed;

        //データ上の時間を超過する限り繰り返す
        while (scoreData_index < score_data.Count && 
            IsReturnOverNextTime(score_data[scoreData_index].time))
        {
            //通常ノーツの生成
            foreach (GeneralNote g in score_data[scoreData_index].general_list)
            {
                //g.obj.transform.position += ReturnLittleDistance(g.time);
                g.obj.SetActive(true);
            }

            //以下特殊ノーツの追加


            scoreData_index++;
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
        obj.GetComponent<Note>().Init(g_manager.speed, scoreCtrl, g_manager.judge_correct_effect_magni);
        return obj;
    }

    //譜面終了の判定
    private void CheckScoreFinish()
    {
        //ここの記述迷いどころさん
        if(judges_num[0] + judges_num[1] + judges_num[2] + judges_num[3] == max_combo_num)
        {
            isPlaying = false;
            Debug.Log("譜面終了");
        }
    }

    //-------------------ゲッター-------------------

    //譜面ファイル読み込みが完了したか返す
    public bool IsReturnReadDataComp()
    {
        return isReadDataComp;
    }

    //譜面の準備が完了したか返す
    public bool IsReturnScoreReady()
    {
        return isGenerateComp;
    }

    //譜面が再生中か返す
    public bool IsReturnPlaying()
    {
        return isPlaying;
    }

    //リザルトを返す
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

    //スコアからランクを返す
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

    //-------------------セッター-------------------

    //判定をノーツから得る(※一旦全部p_critical)
    public void SetNoteJudge(int judgement_num, Vector3 pos)
    {
        //判定別分岐
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
