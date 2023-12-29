using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Note : MonoBehaviour
{
    [SerializeField] private GameObject effect_obj;
    [SerializeField] private GameObject box_obj;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private ScoreCtrl scoreCtrl;
    private float speed = 1.0f;
    private bool isMoving = true;
    private bool isFinishVibration;

    void Start()
    {
        //�d������
        audioClip.LoadAudioData();
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
        if (isMoving) { MoveObject(); }
        //���̃I�u�W�F�N�g�𖕏�
        else if (isFinishVibration && !audioSource.isPlaying) 
        { Destroy(this.gameObject); }
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
    public void GetNoteJudgeFlag(bool isRight)
    {
        audioSource.PlayOneShot(audioClip);
        //�R���g���[���̐U��
        StartCoroutine(Vibration(isRight));
        //ScoreCtrl�ɔ����n��
        scoreCtrl.SetNoteJudge(GrovalConst.P_CRITICAL_NUMBER, this.gameObject.transform.position);
        //�m�[�c�{�b�N�X�̔�\��
        box_obj.SetActive(false);
        isMoving = false;
    }

    //�U���R���[�`��
    private IEnumerator Vibration(bool isRight)
    {
        //�o�C�u���[�V�����̎��s����
        IEnumerator coroutine = OculusController.VibrationController(isRight);
        //�I���҂�
        yield return StartCoroutine(coroutine);
        isFinishVibration = true;
    }
}
