using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralNoteObject : MonoBehaviour
{
    [Header("�a������ƂȂ錕�̗�(����)")]
    [SerializeField] private float judge_magni;
    [SerializeField] Note note;

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
            { note.GetNoteJudgeFlag(); }
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
