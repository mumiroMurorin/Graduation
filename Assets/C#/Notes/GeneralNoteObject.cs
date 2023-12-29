using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralNoteObject : MonoBehaviour
{
    [Header("���s�C�x���g")]
    [SerializeField] private UnityEvent do_event;

    [Header("�a������ƂȂ錕�̗�(����)")]
    [SerializeField] private float judge_magni;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�����G�ꂽ�Ƃ��A����J�n
    private void OnTriggerEnter(Collider other)
    {
        //�����Ă����I�u�W�F�N�g�̃^�O���uSword�v�������Ƃ�
        if (other.transform.CompareTag("Sword"))
        {
            //���ݒ肵�����͂����傫�Ȍ��͂������Ƃ�
            if (other.GetComponent<Sword>().ReturnMagni() >= judge_magni )
            { do_event.Invoke(); }
            //�R���g���[����k�킹��
            StartCoroutine(VibrationController(other.name.Contains("Right")));
        }
    }

    //�R���g���[����k�킹��
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
    //�R���C�_�[���o�čs�����Ƃ�
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Sword"))
        {
           
        }
    }*/
}
