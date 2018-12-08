using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;
using System.Collections.Specialized;
using System.Net;

[DisallowMultipleComponent]
public class GameServerAPI : MonoBehaviour {
	static GameServerAPI s_instance = null;
	public static GameServerAPI Instance
	{
		get
		{
			if (s_instance == null)
			{
				GameObject go = new GameObject("GameServerAPI");		
				DontDestroyOnLoad(go);

				s_instance = go.AddComponent<GameServerAPI>();
			}
			return s_instance;
		}
	}


	// Use this for initialization
	void Start () {
		
	}

	bool m_initialized = false;
	string m_baseAddress = "";
	int m_testedCount;
	public void Init()
	{
		m_initialized = false;

				
		StartCoroutine (TestConnection (0));
				
	}

	private IEnumerator TestConnection (int idx)
	{		
		m_testedCount = idx;
		string url = GameDefines.ROOT_URLS[idx] + "/ping";
		using (UnityWebRequest www = UnityWebRequest.Get (url)) {				
			AsyncOperation op = www.SendWebRequest();

			float timer = 0; 
			bool failed = false;

			while (op.isDone == false) {
				if(timer > 15) 
				{ 
					www.Abort ();
					failed = true; 
					break; 
				}
				timer += Time.deltaTime;

				yield return null;
			}
						
			if (failed || www.isNetworkError) {
						
				Debug.Log (www.error + ' ' + url);
				if (m_testedCount + 1 < GameDefines.ROOT_URLS.Length)
				{
					StartCoroutine (TestConnection (m_testedCount + 1));
				}
			} else {
				if (string.IsNullOrEmpty(m_baseAddress)) {
					m_baseAddress = GameDefines.ROOT_URLS[idx];
					m_initialized = true;
					Debug.Log ("USE: " + m_baseAddress);
					OnlinePlayerProfile.Instance.SigninAnonymous();
				}
			}
		}
	}

	public void PostAPI(string link, WWWForm form, Action<string, string> cb)
	{
		if (!m_initialized)
		{
			if (cb != null)
			{
				cb("Not initialize yet", "");
			}
			return;
		}
		StartCoroutine( IPostAPI( m_baseAddress + "/" + link, form, cb));
	}

	private IEnumerator IPostAPI(string url, WWWForm form, Action<string, string> cb)
	{
		Debug.Log("PostAPI " + url);
		using (UnityWebRequest www = UnityWebRequest.Post (url, form)) {
		      	
			AsyncOperation op = www.SendWebRequest();

			float timer = 0; 
			bool failed = false;

			while (op.isDone == false)
			{
				if(timer > 15) 
				{ 
					www.Abort ();
					failed = true; 
					break; 
				}
				timer += Time.deltaTime;

				yield return null;
			}


			if (www.isNetworkError) {				
				cb (www.error, null);
			} else {            	
				Debug.Log(www.downloadHandler.text);
				cb (null, www.downloadHandler.text);
			}
		}			
	}

	// Update is called once per frame
	void Update () {
	

	}
}
