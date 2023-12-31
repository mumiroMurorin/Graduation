using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class EscapeNote : MonoBehaviour
{
    [Header("破片を表示する？")]
    [SerializeField] private bool isAdventFlag = true;
    [SerializeField] private GameObject effect_obj;
    [SerializeField] private GameObject box_obj;
    [SerializeField] private GameObject broken_obj;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private ScoreCtrl scoreCtrl;
    private float speed = 1.0f;
    private bool isMoving = true;
    private bool isFinishVibration;

    void Start()
    {
        //重いかな
        audioClip.LoadAudioData();
        box_obj.SetActive(true);
        broken_obj.SetActive(false);
        effect_obj.SetActive(false);
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
        if (isMoving) { MoveObject(); }
        //このオブジェクトを抹消
        else if (isFinishVibration && !audioSource.isPlaying && !effect_obj) 
        { Destroy(this.gameObject); }
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
    public void GetNoteJudgeFlag(bool isRight)
    {
        audioSource.PlayOneShot(audioClip);
        //コントローラの振動
        StartCoroutine(Vibration(isRight));
        //ScoreCtrlに判定を渡す
        scoreCtrl.SetNoteJudge(GrovalConst.MISS_NUMBER, this.gameObject.transform.position);//GrovalConst.P_MISS_NUMBERにすればいい
        //ノーツボックスの非アクティブ
        box_obj.SetActive(false);
        //分割ノートのアクティブ
        if (isAdventFlag) { broken_obj.SetActive(true); }
        //エフェクトの表示
        effect_obj.SetActive(true);
        isMoving = false;
    }

    //振動コルーチン
    private IEnumerator Vibration(bool isRight)
    {
        //バイブレーションの実行処理
        IEnumerator coroutine = OculusController.VibrationController(isRight);
        //終了待ち
        yield return StartCoroutine(coroutine);
        isFinishVibration = true;
    }
}
