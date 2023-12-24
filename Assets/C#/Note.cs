using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip break_se;

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

    //�q�I�u�W�F�N�g���W���b�W��𖞂������Ƃ�
    public void GetNoteJudgeFlag()
    {
        audioSource.PlayOneShot(break_se);
        this.gameObject.SetActive(false);
    }
}
