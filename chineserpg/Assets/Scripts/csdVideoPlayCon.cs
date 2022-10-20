using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class csdVideoPlayCon : MonoBehaviour
{
    public bool isInPlay = false;
    public bool isPlayFinish = false;
    private Action playFinishEvent;
    public VideoPlayer vio;

    private const string csVideoPath = "mp4/";

    public void initParam(Action callBack) {
        playFinishEvent = callBack;
    }

    public void setVideo(string fileName) {
        //
        string tmpFile = csVideoPath + fileName;  //是否含扩展名??
        VideoClip tmpVideo = (VideoClip)Resources.Load(tmpFile) as VideoClip;
        if (tmpVideo == null) {
            Debug.LogError("Video file not find! FileName:"+fileName);
        }
        vio.clip = tmpVideo;
       // vio
        //sceneImage.sprite = Sprite.Create(tmpPic, new Rect(0, 0, tmpPic.width, tmpPic.height), new Vector2(0.5f, 0.5f));
    }

    public void startPlay() {
        if (vio == null) {
            vio = GetComponent<VideoPlayer>();
            //vio.loopPointReached += EndVideo;
           // vio.frameDropped += EndVideo;
        }

        isPlayFinish = false;
        isInPlay = true;
        vio.frame = 0;
        vio.Play();
    }
    /*
    private void EndVideo(VideoPlayer video) {
        Debug.LogWarning("endVideo");
        if (playFinishEvent != null)
            playFinishEvent();
    }*/

    // Update is called once per frame
    private void Update()
    {
        
        if (isInPlay) {

            if (vio.frame > 0)
            {
                if ((ulong)vio.frame >= vio.frameCount - 5)
                {
                    Debug.LogWarning("frame:" + vio.frame.ToString() + "/" + vio.frameCount.ToString());
                    isPlayFinish = true;
                    isInPlay = false;
                     if(playFinishEvent != null)
                         playFinishEvent();
                }
            }
        }
        
    }
}
