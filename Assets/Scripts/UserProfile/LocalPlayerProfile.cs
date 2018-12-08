using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[DisallowMultipleComponent]
public class LocalPlayerProfile : MonoBehaviour
{
	private static LocalPlayerProfile s_instance;
	public static LocalPlayerProfile Instance
	{
		get
		{
			if (s_instance == null)
			{
				GameObject go = new GameObject("LocalPlayerProfile");		
				DontDestroyOnLoad(go);
				s_instance = go.AddComponent<LocalPlayerProfile>();
				s_instance.LoadProfile();
			}

			return s_instance;
		}
	}


	// Use this for initialization
	void Start () {
		m_delayNextSave = 0.0f;
	}

	float m_delayNextSave;
	// Update is called once per frame
	void Update () {
		if (m_delayNextSave <= 0) {
			SaveCommonData (true);
			SaveIngameData (true);
		} else {
			m_delayNextSave -= Time.deltaTime;
		}
	}

	//

	UserData m_userData;

	public UserData gameData {
		get {
			return m_userData;
		}
	}

	public void Reset()
	{
		ClearPrefs ();
		LoadProfile();
	}

	public static void ClearPrefs()
	{
		PlayerPrefs.DeleteKey("UserData");
	}

	void LoadProfile()
	{
		m_userData = new UserData();
		m_userData.Load ();
		m_userData.Verify ();
	}

	public void SaveCommonData(bool bForce = false)
	{
		if (!bForce)
		{
			m_userData.commonData.SetChanged (true);
			return;
		}
		else if (!m_userData.commonData.IsChanged())
		{
			return;
		}
		m_delayNextSave = 1.0f;
		PlayerPrefs.SetString("commonData", JsonConvert.SerializeObject(m_userData.commonData));
		Debug.Log ("SaveCommonData");
		m_userData.commonData.SetChanged (false);
	}
	public void SaveIngameData(bool bForce = false)
	{
		if (!bForce)
		{
			m_userData.ingameData.SetChanged (true);
			return;
		}
		else if (!m_userData.ingameData.IsChanged())
		{
			return;
		}
		m_delayNextSave = 1.0f;
		PlayerPrefs.SetString("ingameData", JsonConvert.SerializeObject(m_userData.ingameData));
		Debug.Log ("SaveIngameData");
		m_userData.ingameData.SetChanged (false);
	}


}
