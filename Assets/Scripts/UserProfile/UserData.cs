using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class BaseData{
	public int version;
	protected bool m_bHasChanged = false;

	public bool IsChanged()
	{
		return m_bHasChanged;
	}

	public void SetChanged(bool b)
	{
		m_bHasChanged = b;
	}	
}

public class CommonData : BaseData{
	public int gold;
	public int gem;

	public CommonData(){
		Reset ();
	}

	public void Reset()
	{
		version = GameDefines.SAVE_VERSION;

		gold = 100;
		gem = 10;
	}
}

public class IngameData : BaseData{

	int Nothing;
	public IngameData(){
		Reset ();
	}

	public void Reset()
	{

	}
}

public class UserData
{
	private int m_version;
	private CommonData m_commonData;


	private IngameData m_ingameData;

	public CommonData commonData {
		get { return m_commonData; }
	}

	public IngameData ingameData {
		get { return m_ingameData; }
	}

	public void Load()
	{
		try
		{			 
			string saveStr = PlayerPrefs.GetString("commonData");
			Debug.Log(saveStr);
			CommonData cd = JsonConvert.DeserializeObject<CommonData>(saveStr);
			if(cd.version == GameDefines.SAVE_VERSION)
			{
				this.m_commonData = cd;
			}
		}
		catch(Exception)
		{
			this.m_commonData = null;
		}	
		try
		{			 
			string saveStr = PlayerPrefs.GetString("ingameData");
			Debug.Log(saveStr);
			IngameData id = JsonConvert.DeserializeObject<IngameData>(saveStr);
			if(id.version == GameDefines.SAVE_VERSION)
			{
				this.m_ingameData = id;
			}
		}
		catch(Exception)
		{
			this.m_ingameData = null;
		}
	}
		
	public int gold {
		get { return  m_commonData.gold; }
		set { m_commonData.gold = value; m_commonData.SetChanged (true); }
	}

	public int gem {
		get { return m_commonData.gem; }
		set { m_commonData.gem = value; m_commonData.SetChanged(true); }
	}

	public UserData()
	{
		m_commonData = new CommonData ();
		m_ingameData = new IngameData ();
	}

	public void Verify()
	{	
		if (m_commonData == null) {
			m_commonData = new CommonData ();
		}
		if (m_ingameData == null) {
			m_ingameData = new IngameData ();
		}

	}

	public static bool CorrectArray<T>(ref T[] src, int total, T defaultValue)
	{
		if (src == null) {
			src = new T[total];
			for (int i = 0; i < total; i++) {
				src [i] = defaultValue;
			}
			return false;
		} else if (src.Length != total) {
			T[] tmp = src;
			src = new T[total];
			for (int i = 0; i < total; i++) {
				if (i < tmp.Length) {
					src [i] = tmp [i];
				} else {
					src [i] = defaultValue;
				}
			}
			return false;
		}
		return true;
	}	

	public static bool CorrectArrayClass<T>(ref T[] src, int total) where T : new() 
	{
		if (src == null) {
			src = new T[total];
			for (int i = 0; i < total; i++) {
				src [i] = new T ();
			}
			return false;
		} else if (src.Length != total) {
			T[] tmp = src;
			src = new T[total];
			for (int i = 0; i < total; i++) {
				if (i < tmp.Length) {
					src [i] = tmp [i];
				} else {
					src [i] = new T ();
				}
			}
			return false;
		} else {
			for (int i = 0; i < total; i++) {
				if (src [i] == null) {
					src [i] = new T ();
				}
			}
		}
		return true;
	}		
}