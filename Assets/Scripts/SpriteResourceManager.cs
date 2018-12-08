using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[DisallowMultipleComponent]
public class SpriteResourceManager : MonoBehaviour {
	static SpriteResourceManager s_instance = null;
	private static SpriteResourceManager Instance
	{
		get
		{
			if (s_instance == null)
			{
				GameObject go = new GameObject("SpriteResourceManager");		
				DontDestroyOnLoad(go);

				s_instance = go.AddComponent<SpriteResourceManager>();
			}
			return s_instance;
		}
	}
	// Use this for initialization
	void Start () {
		
	}

	public static Sprite[] GetSprites(string folder)
	{
		return Instance.IGetSprites (folder);
	}

	public static Sprite GetSprite(string spriteName)
	{
		return Instance.IGetSprite (spriteName);
	}

	public static Sprite GetDiceSprite(int val)
	{
		string spriteName = string.Format ("play-screen/dice/{0}", val);
		return GetSprite (spriteName);
	}

	Dictionary<string, Sprite> m_cachedSprites = new Dictionary<string, Sprite>();
	private Sprite IGetSprite(string spriteName)
	{
		if (m_cachedSprites.ContainsKey (spriteName)) {
			return m_cachedSprites [spriteName];
		}
		Sprite spr = Resources.Load<Sprite> ("sprites/" + spriteName);
		m_cachedSprites.Add (spriteName, spr);
		return spr;
	}

	Dictionary<string, Sprite[]> m_cachedFolferSprites = new Dictionary<string, Sprite[]>();
	private Sprite[] IGetSprites(string folder)
	{
		if (m_cachedFolferSprites.ContainsKey (folder)) {
			return m_cachedFolferSprites [folder];
		}
		Sprite[] sprs = Resources.LoadAll<Sprite> ("sprites/" + folder);
		m_cachedFolferSprites.Add (folder, sprs);
		return sprs;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
