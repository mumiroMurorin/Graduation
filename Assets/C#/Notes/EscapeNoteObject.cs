using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Common;

public class EscapeNoteObject : MonoBehaviour
{
    [Header("���s�C�x���g(�E�R���g���[��)")]
    [SerializeField] private UnityEvent right_event;
    [Header("���s�C�x���g(���R���g���[��)")]
    [SerializeField] private UnityEvent left_event;

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
            {
                if (other.name.Contains("Right")){ right_event.Invoke(); }
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
           
        }
    }*/
}