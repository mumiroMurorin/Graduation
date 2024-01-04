using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Common;

public class EscapeNoteObject : MonoBehaviour
{
    [Header("実行イベント(右コントローラ)")]
    [SerializeField] private UnityEvent right_event;
    [Header("実行イベント(左コントローラ)")]
    [SerializeField] private UnityEvent left_event;

    [Header("斬撃判定となる剣の力(距離)")]
    [SerializeField] private float judge_magni;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //判定開始
    private void OnTriggerEnter(Collider other)
    {
        //入ってきたオブジェクトのタグが「MainCamera」だったとき
        if (other.transform.CompareTag("MainCamera"))
        {
            right_event.Invoke();
            left_event.Invoke();
        }
    }

    /*
    //コライダーが出て行ったとき
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Sword"))
        {
           
        }
    }*/
}
