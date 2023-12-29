using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Common;

public class ActionBox : MonoBehaviour
{
    [Header("�a�����󂷂��ǂ���")]
    [SerializeField] private bool isDestroy;
    [Header("��������")]
    [SerializeField] private float revival_time;

    [Header("���s�C�x���g")]
    [SerializeField] private UnityEvent do_event;

    [SerializeField] private GameObject box_obj;
    [SerializeField] private GameObject effect_obj;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private bool isForceSetActive = true;

    void Start()
    {
        //�d������
        audioClip.LoadAudioData();
    }

    void Update()
    {

    }

    //�A�N�V�����{�b�N�X�̕�������
    public void RevivalActionBox()
    {
        box_obj.SetActive(isForceSetActive);
    }

    //�A�N�V�����{�b�N�X�̎a������
    public void GetNoteJudgeFlag(bool isRight)
    {
        //�����̍Đ�(SE�I�u�W�F�N�g�̕���)
        audioSource.PlayOneShot(audioClip);
        //�R���g���[���̐U��
        StartCoroutine(Vibration(isRight));
        //�C�x���g���s
        do_event.Invoke();
        //���̃I�u�W�F�N�g�𖕏� ����
        if (isDestroy) { Destroy(this.gameObject); }
        else
        {
            box_obj.SetActive(false);   //��
            Invoke("RevivalActionBox", revival_time);
        }
    }

    //���̕\����\��
    public void SetActiveBox(bool b)
    {
        isForceSetActive = b;
        box_obj.SetActive(b);
    }

    //�U���R���[�`��
    private IEnumerator Vibration(bool isRight)
    {
        //�o�C�u���[�V�����̎��s����
        IEnumerator coroutine = OculusController.VibrationController(isRight);
        //�I���҂�
        yield return StartCoroutine(coroutine);
    }
}
