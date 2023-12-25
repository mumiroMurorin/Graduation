using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private float magni;        //�͂̑傫��(�����ˑ�)
    private Vector3 pos_past;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        //���̌��݈ʒu���擾
        Vector3 pos_now = this.gameObject.transform.position;
        
        //��(����)���v�Z���đ��
        magni = ReturnMagnitude(pos_past, pos_now) / Time.fixedDeltaTime;
        Debug.Log("����: " + magni);

        //���̌��݈ʒu���ߋ��̃|�W�V�����Ƃ��ēo�^
        pos_past = this.gameObject.transform.position;
    }

    //���̓����̑傫��(����)��Ԃ�
    private float ReturnMagnitude(Vector3 f, Vector3 l)
    {
        return  Mathf.Sqrt(Mathf.Pow(f.x - l.x, 2) + Mathf.Pow(f.y - l.y, 2) + Mathf.Pow(f.z - l.z, 2));
    }

    //�͂̑傫����Ԃ�(�Q�b�^�[)
    public float ReturnMagni()
    {
        return magni;
    }
}
