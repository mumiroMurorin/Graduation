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
    private bool isfadeOut;
    private float fadeDeltaTime;

    void Start()
    {
        
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
            audioSource.volume = (float)(1 - fadeDeltaTime / fadeOutSeconds);
        }
    }

    //�y�Ȃ̃Z�b�g
    public void SetMusic(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    //�Q�[���X�^�[�g(�y�Ȃ̍Đ�)
    public void PlayMusic()
    {
        isfadeOut = false;
        audioSource.Play();
    }

    //�y�Ȃ̏I��(�t�F�[�h�A�E�g)
    public void StopMusic()
    {
        isfadeOut = true;
    }

    //����CSV�̓ǂݍ���
    //private IEnumerator LoadMusic(string file_name)
    //{
        
    //}
}
