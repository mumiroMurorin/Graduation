using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionBox : MonoBehaviour
{
    [Header("斬撃時壊すかどうか")]
    [SerializeField] private bool isDestroy;
    [Header("復活時間")]
    [SerializeField] private float revival_time;

    [Header("実行イベント")]
    [SerializeField] private UnityEvent do_event;

    [SerializeField] private GameObject se_obj;
    [SerializeField] private GameObject effect_obj;

    void Start()
    {

    }

    void Update()
    {

    }

    //アクションボックスの復活処理
    public void RevivalActionBox()
    {
        this.gameObject.SetActive(true);
    }

    //アクションボックスの斬撃判定
    public void GetNoteJudgeFlag()
    {
        //音声の再生(SEオブジェクトの複製)
        Instantiate(se_obj, this.gameObject.gameObject.transform.position, Quaternion.identity);
        //イベント実行
        do_event.Invoke();
        //このオブジェクトを抹消
        if (isDestroy) { Destroy(this.gameObject); }
        else
        {
            this.gameObject.SetActive(false);   //仮
            Invoke("RevivalActionBox", revival_time);
        }
    }
}
