using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StreamManager : MonoBehaviour
{

	public string URL;
	public string textureURL;
	public AudioSource audioSource;
	public Slider slider;

	public RawImage rawImageContainer;
	public Texture myTexture;

    // Start is called before the first frame update
    void Start()
    {
		audioSource = GetComponent<AudioSource>();
		//StartCoroutine(GetAudio());
		StartCoroutine(GetAudioRequest());
		StartCoroutine(GetTexture());
		//StartCoroutine(GetText());
    }


	private IEnumerator GetAudio()
	{
		WWW www = new WWW(URL);
		StartCoroutine(ShowProgress(www));
		while (www.progress < 0.1f)
		{
			yield return new WaitForSeconds(0.1f);
		}

		if (string.IsNullOrEmpty(www.error) == false)
		{
			Debug.Log("Did not work");
			yield break;
		}

		
		AudioClip clip = www.GetAudioClip(false, false);
		//clip = WWWAudioExtensions.GetAudioClip(www, false, true, AudioType.OGGVORBIS);
		clip.name = "Song";
		audioSource.clip = clip;
		Debug.Log("Loaded Clip");
	}


	private IEnumerator GetAudioRequest()
	{
		using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(URL, AudioType.MPEG))
		{
			StartCoroutine(ShowProgress(request));
			yield return request.SendWebRequest();
			//print(request.downloadProgress);
			if (request.isNetworkError)
			{
				print(request.error);
			}
			else
			{
				AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
				clip.name = "clip";
				audioSource.clip = clip;
			}
		}
	}

	private IEnumerator ShowProgress(WWW www)
	{
		while (!www.isDone)
		{
			print(www.progress);
			if (www.progress >= slider.minValue)
			{
				slider.value = www.progress;
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
	private IEnumerator ShowProgress(UnityWebRequest webRequest)
	{
		while (!webRequest.isDone)
		{
			print((float)webRequest.downloadProgress);
			if (webRequest.downloadProgress >= slider.minValue)
			{
				slider.value = webRequest.downloadProgress;
			}	
			yield return new WaitForSeconds(0.1f);
		}
	}

	private IEnumerator GetTexture()
	{
		UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://unity.com/sites/default/files/styles/16_9_s_scale_width/public/recruiting-10_12-1020x574%402x%20.jpg?itok=A8TbLRR4");
		yield return request.SendWebRequest();

		if (request.isNetworkError || request.isHttpError)
		{
			print(request.error);
		}
		else
		{
			myTexture = DownloadHandlerTexture.GetContent(request);
			rawImageContainer.texture = myTexture;
		}

	}

	private IEnumerator GetText()
	{
		UnityWebRequest request = UnityWebRequest.Get("https://unity.com/");
		yield return request.Send();
		if (request.isError)
		{
			print(request.error);
		}
		else
		{
			print(request.downloadHandler.text);
		}
	}
	void Update()
    {
		//if (!audioSource.isPlaying && audioSource.clip != null)
		//{
		//	print("now start playing");
		//	audioSource.Play();
		//}

		
	}

}
