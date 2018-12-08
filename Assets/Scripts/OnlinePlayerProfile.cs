using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Collections.Specialized;

[DisallowMultipleComponent]
public class OnlinePlayerProfile: MonoBehaviour
{
	private static OnlinePlayerProfile s_instance;

	public static OnlinePlayerProfile Instance
	{
		get
		{
			if (s_instance == null)
			{
				GameObject go = new GameObject("OnlinePlayerProfile");		
				DontDestroyOnLoad(go);

				s_instance = go.AddComponent<OnlinePlayerProfile>();
				s_instance.LoadLocal();
			}

			return s_instance;
		}
	}

	public class DataInfo
	{
		public string m_localId;
		public string m_onlineId;
		public bool m_bEmpty;

		public void Reset()
		{
			m_bEmpty = true;
			m_localId = "";
			m_onlineId = "";
		}
	};

	DataInfo m_dataInfo;

	public DataInfo dataInfo
	{
		get
		{
			return m_dataInfo;
		}
	}

	public static readonly string LOCAL_ID_TAG = "localId";
	public static readonly string ONLINE_ID_TAG = "onlineId";
	private void LoadLocal()
	{
		m_dataInfo = new DataInfo();
		m_dataInfo.Reset();

		string localId = PlayerPrefs.GetString(LOCAL_ID_TAG);
		if (string.IsNullOrEmpty(localId))
		{
			localId = System.Guid.NewGuid().ToString();
			PlayerPrefs.SetString(LOCAL_ID_TAG, localId);
		}
		m_dataInfo.m_localId = localId;

		string onlineId = PlayerPrefs.GetString(ONLINE_ID_TAG);
		if (string.IsNullOrEmpty(localId))
		{
			onlineId = "";
		}
		m_dataInfo.m_onlineId = onlineId;
	}

	private void LoadOnline(JObject jData)
	{
		if (jData != null)
		{
			string localId = JsonHelper.GetString(jData, "localId");
			string onlineId = JsonHelper.GetString(jData, "onlineId");

			m_dataInfo.m_localId = localId;
			m_dataInfo.m_onlineId = onlineId;
			m_dataInfo.m_bEmpty = false;

			PlayerPrefs.SetString(LOCAL_ID_TAG, localId);
			PlayerPrefs.SetString(ONLINE_ID_TAG, onlineId);
		}
	}

	// Use this for initialization
	void Start () {

	}

	public void SigninAnonymous()
	{
		WWWForm form = new WWWForm();
		form.AddField(LOCAL_ID_TAG, m_dataInfo.m_localId);
		form.AddField(ONLINE_ID_TAG, m_dataInfo.m_onlineId);
		GameServerAPI.Instance.PostAPI("signinAnonymous", form
			, ((err, result) =>
			{
				if(err != null)
				{
				}
				else
				{
					JObject jRet = JObject.Parse(result);
					if (jRet != null)
					{
						bool success = JsonHelper.GetBool(jRet, "success", false);
						if (success)
						{
							JObject jData = jRet["data"] as JObject;
							LoadOnline(jData);
						}
					}
				}
			})
		);
	}

	// Update is called once per frame
	void Update () {
	}
}
