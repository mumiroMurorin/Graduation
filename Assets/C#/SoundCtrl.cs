using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.IO;
using UnityEngine;

public class SoundCtrl : MonoBehaviour
{
    [Header("フェード時間")]
    [SerializeField] private float fadeOutSeconds = 1.0f;

    private AudioSource audioSource;
    private bool isfadeOut;
    private float fadeDeltaTime;

    void Start()
    {
        
    }

    void Update()
    {
        //フェードアウト処理
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

    //楽曲のセット
    public void SetMusic(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    //ゲームスタート(楽曲の再生)
    public void PlayMusic()
    {
        isfadeOut = false;
        audioSource.Play();
    }

    //楽曲の終了(フェードアウト)
    public void StopMusic()
    {
        isfadeOut = true;
    }

    //譜面CSVの読み込み
    //private IEnumerator LoadMusic(string file_name)
    //{
        
    //}
}
