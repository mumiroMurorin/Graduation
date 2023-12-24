using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCtrl : MonoBehaviour
{
    [Header("譜面名(仮)")]
    [SerializeField] private string csvfile_name;

    [Header("譜面生成場所")]
    [SerializeField] private GameObject generate_pnt;
    [Header("ノート判定場所")]
    [SerializeField] private GameObject judge_pnt;
    [Header("通常ノート")]
    [SerializeField] private GameObject generalNote_obj;

    [SerializeField] private GameCtrl gameCtrl;

    private ReadScoreData readScore;
    private GameManager g_manager;
    private List<NotesBlock> score_data;    //譜面データ
    private GameObject note_par;            //ノート親
    private Vector3 generate_pos;           //ノート出現pos
    private Vector3 judge_pos;              //ノート判定pos
    private bool isGenerateComp;            //譜面生成官僚？
    private bool isPlaying;                 //プレイ中？
    private float note_generate_time;       //ノート生成手前時間
    private float game_time;                //経過時間

    void Start()
    {
        note_par = new GameObject("Note_par");
    }

    void Update()
    {
        //譜面の読み込みと先行生成
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
            //経過時間の加算
            game_time += Time.fixedDeltaTime;
            //ノートのアクティブ化
            SetActiveNote();
        }
    }

    //初期化
    public void Init()
    {
        isPlaying = false;
        generate_pos = generate_pnt.transform.position;
        judge_pos = judge_pnt.transform.position;
        note_generate_time = Mathf.Abs(generate_pos.z - generate_pos.z) / g_manager.speed;
    }

    //譜面CSVの読み込み
    public void ReadStart()
    {
        readScore = new ReadScoreData(csvfile_name);
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
        obj.AddComponent<Note>().Init(g_manager.speed);
        return obj;
    }

    //-------------------ゲッター-------------------

    public bool IsReturnScoreReady()
    {
        return isGenerateComp;
    }
}
