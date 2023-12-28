using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionBox : MonoBehaviour
{
    [Header("�a�����󂷂��ǂ���")]
    [SerializeField] private bool isDestroy;
    [Header("��������")]
    [SerializeField] private float revival_time;

    [Header("���s�C�x���g")]
    [SerializeField] private UnityEvent do_event;

    [SerializeField] private GameObject se_obj;
    [SerializeField] private GameObject effect_obj;

    void Start()
    {

    }

    void Update()
    {

    }

    //�A�N�V�����{�b�N�X�̕�������
    public void RevivalActionBox()
    {
        this.gameObject.SetActive(true);
    }

    //�A�N�V�����{�b�N�X�̎a������
    public void GetNoteJudgeFlag()
    {
        //�����̍Đ�(SE�I�u�W�F�N�g�̕���)
        Instantiate(se_obj, this.gameObject.gameObject.transform.position, Quaternion.identity);
        //�C�x���g���s
        do_event.Invoke();
        //���̃I�u�W�F�N�g�𖕏�
        if (isDestroy) { Destroy(this.gameObject); }
        else
        {
            this.gameObject.SetActive(false);   //��
            Invoke("RevivalActionBox", revival_time);
        }
    }
}
