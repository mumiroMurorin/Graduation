using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.IO;
using UnityEngine;

public class SoundCtrl : MonoBehaviour
{
    [Header("�t�F�[�h����")]
    [SerializeField] private float fadeOutSeconds = 1.0f;

    private AudioSource audioSource;
    private AudioClip music;
    private bool isfadeOut;
    private bool isLoadComp;
    private float fadeDeltaTime;

    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        //�t�F�[�h�A�E�g����
        if (isfadeOut)
        {
            fadeDeltaTime += Time.deltaTime;
            if (fadeDeltaTime >= fadeOutSeconds)
            {
                fadeDeltaTime = fadeOutSeconds;
                isfadeOut = false;
            }
            audioSource.volume = (float)(1 - fadeDeltaTime / fadeOutSeconds); //��
        }
    }

    //������
    public void Init()
    {
        music = null;
        isLoadComp = false;
        isfadeOut = false;
    }

    //�Q�[�����X�^�[�g
    public void Init_Start()
    {
        audioSource.Stop();
        fadeDeltaTime = 0;
    }

    //�v���r���[�̍Đ�
    public void PlayPreview(AudioClip preview)
    {
        audioSource.clip = preview;
        audioSource.Play();
    }

    //�Q�[���X�^�[�g(�y�Ȃ̍Đ�)
    public void PlayMusic()
    {
        audioSource.volume = 1;//��
        isfadeOut = false;
        audioSource.Play();
    }

    //�y�Ȃ̏I��(�t�F�[�h�A�E�g)
    public void StopMusic()
    {
        isfadeOut = true;
    }

    //���[�h�J�n(�Z�b�^�[)
    public void ReadStart(string file_name)
    {
        StartCoroutine(LoadMusic(file_name + "_music"));
    }

    //�y�Ȃ̓ǂݍ���
    private IEnumerator LoadMusic(string file_name)
    {
        //CSV�f�[�^�̓ǂݍ���
        Addressables.LoadAssetAsync<AudioClip>(file_name).Completed += op =>
        {
            music = Instantiate(op.Result);
            //��U�����[�X���f�N�������g���邪�A�ǂ��Ȃ��Ǝv����
            //Addressables.Release(op);
        };

        do
        {
            yield return null;
        } while (music == null);

        audioSource.clip = music;
        music.LoadAudioData();
        isLoadComp = true;
    }

    //-----------------�Q�b�^�[-----------------

    //�����������Ԃ�
    public bool IsReturnLoadComp()
    {
        return isLoadComp;
    }
}
