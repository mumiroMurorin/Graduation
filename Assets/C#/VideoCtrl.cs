using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoCtrl : MonoBehaviour
{
    private VideoPlayer videoPlayer; 

    void Start()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        videoPlayer.Prepare();
    }

    void Update()
    {
        
    }

    //�Đ��J�n
    public void PlayVideo()
    {
        videoPlayer.Play();
    }
}
