using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Common;

public class GeneralNoteObject : MonoBehaviour
{
    [Header("���s�C�x���g(�E�R���g���[��)")]
    [SerializeField] private UnityEvent right_event;
    [Header("���s�C�x���g(���R���g���[��)")]
    [SerializeField] private UnityEvent left_event;
    [Header("P-Critical�C�x���g")]
    [SerializeField] private UnityEvent pCritical_event;
    [Header("Critical�C�x���g")]
    [SerializeField] private UnityEvent critical_event;
    [Header("Hit�C�x���g")]
    [SerializeField] private UnityEvent hit_event;
    [Header("Miss�C�x���g")]
    [SerializeField] private UnityEvent miss_event;

    [Header("P-Critical����ƂȂ錕�̗�(����)")]
    [SerializeField] private float judge_magni_pCri;
    [Header("Critical����ƂȂ錕�̗�(����)")]
    [SerializeField] private float judge_magni_cri;
    [Header("Hit����ƂȂ錕�̗�(����)(�Œ���K�v�ȗ�)")]
    [SerializeField] private float judge_magni_hit;

    /*
    //�����G�ꂽ�Ƃ��A����J�n
    private void OnTriggerEnter(Collider other)
    {
        //�����Ă����I�u�W�F�N�g�̃^�O���uSword�v�������Ƃ�
        if (other.transform.CompareTag("Sword"))
        {
            //���ݒ肵�����͂����傫�Ȍ��͂������Ƃ�
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

            //���ݒ肵�����͂����傫�Ȍ��͂������Ƃ�
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
    //�R���C�_�[���o�čs�����Ƃ�
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Sword"))
        {
            //���ݒ肵�����͂����傫�Ȍ��͂������Ƃ�
            if (other.GetComponent<Sword>().ReturnMagni() >= judge_magni)
            {
                if (other.name.Contains("Right")) { right_event.Invoke(); }
                else { left_event.Invoke(); }
            }
        }
    }*/
}
