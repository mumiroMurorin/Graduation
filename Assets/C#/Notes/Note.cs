using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Note : MonoBehaviour
{
    [SerializeField] private GameObject se_obj;
    [SerializeField] private GameObject effect_obj;

    private ScoreCtrl scoreCtrl;

    private float speed = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        //仮消滅判定
        if (this.gameObject.transform.position.z < -2) {
            //ScoreCtrlに判定を渡す
            scoreCtrl.SetNoteJudge(GrovalConst.MISS_NUMBER, this.gameObject.transform.position);
            //このオブジェクトを抹消
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        MoveObject();
    }

    public void Init(float s, ScoreCtrl s_ctrl)
    {
        speed = s;
        scoreCtrl = s_ctrl;
    }

    //ノートを動かす(一旦真っすぐ)
    private void MoveObject()
    {
        this.gameObject.transform.position += -Vector3.forward * Time.fixedDeltaTime * speed;
    }

    //ノートの斬撃判定
    public void GetNoteJudgeFlag()
    {
        //音声の再生(SEオブジェクトの複製)
        Instantiate(se_obj, this.gameObject.gameObject.transform.position, Quaternion.identity);
        //ScoreCtrlに判定を渡す
        scoreCtrl.SetNoteJudge(GrovalConst.P_CRITICAL_NUMBER, this.gameObject.transform.position);
        //このオブジェクトを抹消
        Destroy(this.gameObject);
    }
}
