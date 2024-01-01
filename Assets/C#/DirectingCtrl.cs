using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DirectingCtrl : MonoBehaviour
{
    private GameObject directing_obj;
    private PlayableDirector director;
    private bool isLoadComp;
    public bool isPlaying;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //初期化
    public void Init()
    {
        if (directing_obj) { Destroy(directing_obj); }
        director = null;
        isLoadComp = false;
    }

    //ゲームリスタート
    public void Init_Start()
    {
        isPlaying = true;
        if (director) { director.Stop(); }
    }

    //ゲームスタート(演出の再生)
    public void PlayDirecting()
    {
        director.Play();
    }

    //ロード開始(セッター)
    public void ReadStart(string file_name)
    {
        StartCoroutine(LoadDirecting(file_name + "_directing"));
    }

    //演出オブジェクトの読み込み
    private IEnumerator LoadDirecting(string file_name)
    {
        //TimeLineデータの読み込み
        Addressables.LoadAssetAsync<GameObject>(file_name).Completed += op =>
        {
            directing_obj = Instantiate(op.Result);
            //一旦リリースをデクリメントするが、良くないと思われる
            //Addressables.Release(op);
        };

        do
        {
            yield return null;
        } while (directing_obj == null);

        director = directing_obj.GetComponentInChildren<PlayableDirector>();

        do
        {
            yield return null;
        } while (director == null);

        isLoadComp = true;
    }

    //準備完了か返す
    public bool IsReturnLoadComp()
    {
        return isLoadComp;
    }

    //演出が再生中か返す
    public bool IsReturnPlaying()
    {
        return isPlaying;
    }

    //-----------------セッター-----------------

    //しぐなるをうけとる
    public void SetDirectingFinish()
    {
        isPlaying = false;
    }
}
