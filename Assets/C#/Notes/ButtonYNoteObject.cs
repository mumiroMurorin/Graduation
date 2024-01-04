using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Common;

public class ButtonYNoteObject : MonoBehaviour
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
        if (OVRInput.Get(OVRInput.Button.Four))
        {
            Debug.Log("updateでYボタンを押しています");
        }
    }

    //剣が触れたとき、判定開始
    private void OnTriggerEnter(Collider other)
    {
        //入ってきたオブジェクトのタグが「Sword」だったとき
        if (other.transform.CompareTag("Sword"))
        {
            if (OVRInput.Get(OVRInput.Button.Four)){
                right_event.Invoke();
                Debug.Log("Yボタンを押しています");
            }
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