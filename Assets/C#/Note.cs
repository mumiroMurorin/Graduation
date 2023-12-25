using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private GameObject se_obj;
    [SerializeField] private GameObject effect_obj;

    private float speed = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MoveObject();
    }

    public void Init(float s)
    {
        speed = s;
    }

    //�m�[�g�𓮂���(��U�^������)
    private void MoveObject()
    {
        this.gameObject.transform.position += -Vector3.forward * Time.fixedDeltaTime * speed;
    }

    //�m�[�g�̎a������
    public void GetNoteJudgeFlag()
    {
        //�����̍Đ�(SE�I�u�W�F�N�g�̕���)
        Instantiate(se_obj, this.gameObject.gameObject.transform.position, Quaternion.identity);
        //���̃I�u�W�F�N�g�𖕏�
        Destroy(this.gameObject);
    }
}
