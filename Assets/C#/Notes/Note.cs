using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Note : MonoBehaviour
{
    [SerializeField] private GameObject se_obj;
    [SerializeField] private GameObject effect_obj;

    private ScoreCtrl scoreCtrl;

    private float speed = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        //�����Ŕ���
        if (this.gameObject.transform.position.z < -2) {
            //ScoreCtrl�ɔ����n��
            scoreCtrl.SetNoteJudge(GrovalConst.MISS_NUMBER, this.gameObject.transform.position);
            //���̃I�u�W�F�N�g�𖕏�
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        MoveObject();
    }

    public void Init(float s, ScoreCtrl s_ctrl)
    {
        speed = s;
        scoreCtrl = s_ctrl;
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
        //ScoreCtrl�ɔ����n��
        scoreCtrl.SetNoteJudge(GrovalConst.P_CRITICAL_NUMBER, this.gameObject.transform.position);
        //���̃I�u�W�F�N�g�𖕏�
        Destroy(this.gameObject);
    }
}
