using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class NoteVibration : MonoBehaviour
{
    [SerializeField] private bool isRight;
    private void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        //バイブレーションの実行処理
        IEnumerator coroutine = OculusController.VibrationController(isRight);
        //終了待ち
        yield return StartCoroutine(coroutine);
        Destroy(this.gameObject);
    }
}
