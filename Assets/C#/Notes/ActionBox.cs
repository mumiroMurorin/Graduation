using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionBox : MonoBehaviour
{
    [SerializeField] private GameObject se_obj;
    [SerializeField] private GameObject effect_obj;
    [Header("���s�C�x���g")]
    [SerializeField] private UnityEvent do_event;

    private float speed = 1.0f;

    void Start()
    {

    }

    void Update()
    {

    }

    //�m�[�g�̎a������
    public void GetNoteJudgeFlag()
    {
        //�����̍Đ�(SE�I�u�W�F�N�g�̕���)
        Instantiate(se_obj, this.gameObject.gameObject.transform.position, Quaternion.identity);
        //�C�x���g���s
        do_event.Invoke();
        //���̃I�u�W�F�N�g�𖕏�
        Destroy(this.gameObject);
    }
}
