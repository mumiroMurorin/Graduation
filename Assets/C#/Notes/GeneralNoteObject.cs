using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralNoteObject : MonoBehaviour
{
    [Header("実行イベント")]
    [SerializeField] private UnityEvent do_event;

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
            { do_event.Invoke(); }
            //コントローラを震わせる
            StartCoroutine(VibrationController(other.name.Contains("Right")));
        }
    }

    //コントローラを震わせる
    private IEnumerator VibrationController(bool isRight)
    {
        if (isRight) {
            OVRInput.SetControllerVibration(0f, 1f, OVRInput.Controller.RTouch);
            yield return new WaitForSeconds(0.3f);
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
        }
        else
        {
            OVRInput.SetControllerVibration(0f, 1f, OVRInput.Controller.LTouch);
            yield return new WaitForSeconds(0.3f);
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
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
