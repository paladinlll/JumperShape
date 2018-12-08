using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public class Main : MonoBehaviour {
	static Main s_instance = null;
	public static Main Instance
	{
		get
		{
			if (s_instance == null)
			{
				s_instance = UIHelper.Instance.New<Main>(null);
				GameObject go = s_instance.gameObject;	

				go.name = "Main";
				DontDestroyOnLoad(go);
			}
			return s_instance;
		}
	}

	public void LoadScene(string sceneName, Action cb = null)
	{
		LoadingLayer loadingLayer = null;
		//if (sceneName == "MainMenu")
		{
			loadingLayer = UIHelper.Instance.New<LoadingLayer>(transform);
			loadingLayer.Show();
		}


		StartCoroutine(LoadAsyncScene(sceneName,loadingLayer, cb));
	}

	IEnumerator LoadAsyncScene(string sceneName, LoadingLayer loadingLayer, Action cb)
	{
		
		{
			AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

			//Wait until the last operation fully loads to return anything
			while (!asyncLoad.isDone)
			{
				loadingLayer.UpdateProcessStep(asyncLoad.progress);
				yield return null;
			}
		}


		if (loadingLayer != null)
		{
			DestroyObject(loadingLayer.gameObject);
		}
		if (cb != null)
		{
			cb();
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
