using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Common;

public class GeneralNoteObject : MonoBehaviour
{
    [Header("実行イベント(右コントローラ)")]
    [SerializeField] private UnityEvent right_event;
    [Header("実行イベント(左コントローラ)")]
    [SerializeField] private UnityEvent left_event;

    [Header("斬撃判定となる剣の力(距離)")]
    [SerializeField] private float judge_magni;

    //剣が触れたとき、判定開始
    private void OnTriggerEnter(Collider other)
    {
        //入ってきたオブジェクトのタグが「Sword」だったとき
        if (other.transform.CompareTag("Sword"))
        {
            //且つ設定した剣力よりも大きな剣力だったとき
            if (other.GetComponent<Sword>().ReturnMagni() >= judge_magni )
            {
                if (other.name.Contains("Right")){ right_event.Invoke(); }
                else { left_event.Invoke(); }
            }
        }
    }

    //コライダーが出て行ったとき
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Sword"))
        {
            if (other.name.Contains("Right")) { right_event.Invoke(); }
            else { left_event.Invoke(); }
        }
    }
}
