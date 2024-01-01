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

    //������
    public void Init()
    {
        if (directing_obj) { Destroy(directing_obj); }
        director = null;
        isLoadComp = false;
    }

    //�Q�[�����X�^�[�g
    public void Init_Start()
    {
        isPlaying = true;
        if (director) { director.Stop(); }
    }

    //�Q�[���X�^�[�g(���o�̍Đ�)
    public void PlayDirecting()
    {
        director.Play();
    }

    //���[�h�J�n(�Z�b�^�[)
    public void ReadStart(string file_name)
    {
        StartCoroutine(LoadDirecting(file_name + "_directing"));
    }

    //���o�I�u�W�F�N�g�̓ǂݍ���
    private IEnumerator LoadDirecting(string file_name)
    {
        //TimeLine�f�[�^�̓ǂݍ���
        Addressables.LoadAssetAsync<GameObject>(file_name).Completed += op =>
        {
            directing_obj = Instantiate(op.Result);
            //��U�����[�X���f�N�������g���邪�A�ǂ��Ȃ��Ǝv����
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

    //�����������Ԃ�
    public bool IsReturnLoadComp()
    {
        return isLoadComp;
    }

    //���o���Đ������Ԃ�
    public bool IsReturnPlaying()
    {
        return isPlaying;
    }

    //-----------------�Z�b�^�[-----------------

    //�����Ȃ�������Ƃ�
    public void SetDirectingFinish()
    {
        isPlaying = false;
    }
}
