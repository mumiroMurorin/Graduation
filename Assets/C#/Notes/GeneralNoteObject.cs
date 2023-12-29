using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Common;

public class GeneralNoteObject : MonoBehaviour
{
    [Header("実行イベント")]
    [SerializeField] private UnityEvent do_event;

    [Header("右ばいぶれーしょん")]
    [SerializeField] private GameObject vibe_right_obj;

    [Header("左ばいぶれーしょん")]
    [SerializeField] private GameObject vibe_left_obj;

    [Header("斬撃判定となる剣の力(距離)")]
    [SerializeField] private float judge_magni;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //剣が触れたとき、判定開始
    private void OnTriggerEnter(Collider other)
    {
        //入ってきたオブジェクトのタグが「Sword」だったとき
        if (other.transform.CompareTag("Sword"))
        {
            //且つ設定した剣力よりも大きな剣力だったとき
            if (other.GetComponent<Sword>().ReturnMagni() >= judge_magni )
            {
                if (other.name.Contains("Right")){ Instantiate(vibe_right_obj); }
                else { Instantiate(vibe_left_obj);}
                do_event.Invoke();
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
