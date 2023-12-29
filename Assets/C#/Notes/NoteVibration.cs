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
        //�o�C�u���[�V�����̎��s����
        IEnumerator coroutine = OculusController.VibrationController(isRight);
        //�I���҂�
        yield return StartCoroutine(coroutine);
        Destroy(this.gameObject);
    }
}
