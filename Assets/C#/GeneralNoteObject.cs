using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralNoteObject : MonoBehaviour
{
    [Header("斬撃判定となる剣の力(距離)")]
    [SerializeField] private float judge_magni;
    [SerializeField] Note note;

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
            { note.GetNoteJudgeFlag(); }
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
