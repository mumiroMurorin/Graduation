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
    [Header("P-Criticalイベント")]
    [SerializeField] private UnityEvent pCritical_event;
    [Header("Criticalイベント")]
    [SerializeField] private UnityEvent critical_event;
    [Header("Hitイベント")]
    [SerializeField] private UnityEvent hit_event;
    [Header("Missイベント")]
    [SerializeField] private UnityEvent miss_event;

    [Header("P-Critical判定となる剣の力(距離)")]
    [SerializeField] private float judge_magni_pCri;
    [Header("Critical判定となる剣の力(距離)")]
    [SerializeField] private float judge_magni_cri;
    [Header("Hit判定となる剣の力(距離)(最低限必要な力)")]
    [SerializeField] private float judge_magni_hit;

    /*
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
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Sword"))
        {
            float power = other.GetComponent<Sword>().ReturnMagni();

            //且つ設定した剣力よりも大きな剣力だったとき
            if (power >= judge_magni_hit)
            {
                if (power >= judge_magni_pCri) { pCritical_event.Invoke(); }
                else if(power >= judge_magni_cri) { critical_event.Invoke(); }
                else { hit_event.Invoke(); }

                if (other.name.Contains("Right")) { right_event.Invoke(); }
                else { left_event.Invoke(); }
            }
        }
    }

    /*
    //コライダーが出て行ったとき
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Sword"))
        {
            //且つ設定した剣力よりも大きな剣力だったとき
            if (other.GetComponent<Sword>().ReturnMagni() >= judge_magni)
            {
                if (other.name.Contains("Right")) { right_event.Invoke(); }
                else { left_event.Invoke(); }
            }
        }
    }*/
}
