using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

public class AudioSvc : MonoBehaviour {

	public static AudioSvc Instance;
	public AudioSource bgSource;
	public AudioSource uiSource;

	public void InitSvc()
	{
		Instance = this;
	}

	public void PlayBGMusic(string name, bool isLoop = false)
	{
		AudioClip audioClip = ResSvr.instance.LoadAudio("ResAudio/" + name, true);
		Debug.LogError(audioClip.name);
		if (bgSource.clip == null || bgSource.clip.name!=audioClip.name)
		{
			bgSource.clip = audioClip;
			bgSource.loop= isLoop;
			bgSource.Play();
		}
	}

    public void PlayUIAudio(string name)
    {
        AudioClip audioClip = ResSvr.instance.LoadAudio("ResAudio/" + name, true);
        uiSource.clip = audioClip;
        uiSource.Play();
    }

    public void StopBGMusic()
    {
        if(bgSource!=null)
        {
            bgSource.Stop();
        }
    }

}
