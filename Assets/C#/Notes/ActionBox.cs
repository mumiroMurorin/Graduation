using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Common;

public class ActionBox : MonoBehaviour
{
    [Header("破片を表示する？")]
    [SerializeField] private bool isAdventFlag = true;
    [Header("斬撃時壊すかどうか")]
    [SerializeField] private bool isDestroy;
    [Header("復活時間")]
    [SerializeField] private float revival_time;

    [Header("実行イベント")]
    [SerializeField] private UnityEvent do_event;

    [SerializeField] private GameObject box_obj;
    [SerializeField] private GameObject effect_obj;
    [SerializeField] private GameObject broken_obj;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private bool isFinishVibration;
    private bool isForceSetActive = true;

    void Start()
    {
        //重いかな
        audioClip.LoadAudioData();
        //box_obj.SetActive(true);
        broken_obj.SetActive(false);
        effect_obj.SetActive(false);
    }

    void Update()
    {
        //このオブジェクトを抹消
        if (isDestroy && isFinishVibration && !audioSource.isPlaying && !effect_obj)
        { Destroy(this.gameObject); }
    }

    //アクションボックスの復活処理
    public void RevivalActionBox()
    {
        box_obj.SetActive(isForceSetActive);
    }

    //アクションボックスの斬撃判定
    public void GetNoteJudgeFlag(bool isRight)
    {
        //音声の再生(SEオブジェクトの複製)
        audioSource.PlayOneShot(audioClip);
        //コントローラの振動
        StartCoroutine(Vibration(isRight));
        //イベント実行
        do_event.Invoke();
        //分割ノートの複製
        if (isAdventFlag)
        {
            Instantiate(broken_obj, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform).SetActive(true);
        }
        //エフェクトの表示
        Instantiate(effect_obj, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform).SetActive(true);

        if (!isDestroy)
        {
            box_obj.SetActive(false);
            Invoke("RevivalActionBox", revival_time);
        }
    }

    //箱の表示非表示
    public void SetActiveBox(bool b)
    {
        isForceSetActive = b;
        box_obj.SetActive(b);
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
