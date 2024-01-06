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
        //フェードアウト処理
        if (isfadeOut)
        {
            fadeDeltaTime += Time.deltaTime;
            if (fadeDeltaTime >= fadeOutSeconds)
            {
                fadeDeltaTime = fadeOutSeconds;
                isfadeOut = false;
            }
            audioSource.volume = (float)(1 - fadeDeltaTime / fadeOutSeconds); //仮
        }
    }

    //初期化
    public void Init()
    {
        music = null;
        isLoadComp = false;
        isfadeOut = false;
    }

    //ゲームリスタート
    public void Init_Start()
    {
        audioSource.Stop();
        fadeDeltaTime = 0;
    }

    //プレビューの再生
    public void PlayPreview(AudioClip preview)
    {
        audioSource.clip = preview;
        audioSource.Play();
    }

    //ゲームスタート(楽曲の再生)
    public void PlayMusic()
    {
        audioSource.volume = 1;//仮
        isfadeOut = false;
        audioSource.Play();
    }

    //楽曲の終了(フェードアウト)
    public void StopMusic()
    {
        isfadeOut = true;
    }

    //ロード開始(セッター)
    public void ReadStart(string file_name)
    {
        StartCoroutine(LoadMusic(file_name + "_music"));
    }

    //楽曲の読み込み
    private IEnumerator LoadMusic(string file_name)
    {
        //CSVデータの読み込み
        Addressables.LoadAssetAsync<AudioClip>(file_name).Completed += op =>
        {
            music = Instantiate(op.Result);
            //一旦リリースをデクリメントするが、良くないと思われる
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

    //-----------------ゲッター-----------------

    //準備完了か返す
    public bool IsReturnLoadComp()
    {
        return isLoadComp;
    }
}
