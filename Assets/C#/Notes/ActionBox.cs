using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Common;

public class ActionBox : MonoBehaviour
{
    [Header("�j�Ђ�\������H")]
    [SerializeField] private bool isAdventFlag = true;
    [Header("�a�����󂷂��ǂ���")]
    [SerializeField] private bool isDestroy;
    [Header("��������")]
    [SerializeField] private float revival_time;

    [Header("���s�C�x���g")]
    [SerializeField] private UnityEvent do_event;

    [SerializeField] private GameObject box_obj;
    [SerializeField] private GameObject effect_obj;
    [SerializeField] private GameObject broken_obj;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private bool isFinishVibration;
    private bool isForceSetActive = true;

    void Start()
    {
        //�d������
        audioClip.LoadAudioData();
        //box_obj.SetActive(true);
        broken_obj.SetActive(false);
        effect_obj.SetActive(false);
    }

    void Update()
    {
        //���̃I�u�W�F�N�g�𖕏�
        if (isDestroy && isFinishVibration && !audioSource.isPlaying && !effect_obj)
        { Destroy(this.gameObject); }
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
        //�����m�[�g�̕���
        if (isAdventFlag)
        {
            Instantiate(broken_obj, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform).SetActive(true);
        }
        //�G�t�F�N�g�̕\��
        Instantiate(effect_obj, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform).SetActive(true);

        if (!isDestroy)
        {
            box_obj.SetActive(false);
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
        isFinishVibration = true;
    }
}
