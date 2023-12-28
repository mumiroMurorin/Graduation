using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionBox : MonoBehaviour
{
    [SerializeField] private GameObject se_obj;
    [SerializeField] private GameObject effect_obj;
    [Header("実行イベント")]
    [SerializeField] private UnityEvent do_event;

    private float speed = 1.0f;

    void Start()
    {

    }

    void Update()
    {

    }

    //ノートの斬撃判定
    public void GetNoteJudgeFlag()
    {
        //音声の再生(SEオブジェクトの複製)
        Instantiate(se_obj, this.gameObject.gameObject.transform.position, Quaternion.identity);
        //イベント実行
        do_event.Invoke();
        //このオブジェクトを抹消
        Destroy(this.gameObject);
    }
}
