using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloaderInfo{
	public string ObjectName;
	public List<BaseObject> Entries;
	public BaseObject Sample;
	public int NumberToPreload; 

	public PreloaderInfo(string objectName, BaseObject sample){
		ObjectName = objectName;
		Sample = sample;
		NumberToPreload = 0;
		Entries = new List<BaseObject> ();
	}
};

public class GameObjectPreloader : MonoBehaviour {	
	static GameObjectPreloader s_instance = null;
	private static GameObjectPreloader Instance
	{
		get
		{
			if (s_instance == null)
			{
				GameObject go = new GameObject("GameObjectPreloader");		
				DontDestroyOnLoad(go);

				s_instance = go.AddComponent<GameObjectPreloader>();
				Instance.Init ();
			}
			return s_instance;
		}
	}

	Dictionary<string, PreloaderInfo> m_objectsToPreload;
	// Use this for initialization
	void Start () {
		
	}

	private void Init(){
		if (m_objectsToPreload != null) {
			return;
		}

		//m_gameObjectPool.SerDefaultHolder (transform);


		m_objectsToPreload = new Dictionary<string, PreloaderInfo> ();

		BaseObject[] baseObjects = Resources.LoadAll<BaseObject> ("preloads/");
		foreach (var p in baseObjects) {
			Debug.LogFormat ("preload {0}", p.gameObject.name);
			PreloaderInfo preloaderInfo = new PreloaderInfo (p.gameObject.name, p);
			m_objectsToPreload.Add (p.gameObject.name, preloaderInfo);
		}
		m_bBusing = false;
	}


	public static void PreloadObject(string objectName, int poolSize = 1)
	{
		if (!Instance.m_objectsToPreload.ContainsKey (objectName)) {
			Debug.LogErrorFormat ("[{0}] no exits to preload", objectName);
			return;
		}
		PreloaderInfo preloaderInfo = Instance.m_objectsToPreload[objectName];
		if (preloaderInfo.NumberToPreload < poolSize) {
			preloaderInfo.NumberToPreload = poolSize;
		}
	}


	public static bool AllObjectsLoaded{
		get{
			if (Instance.m_bBusing) {
				return false;
			}
			foreach (var p in Instance.m_objectsToPreload) {
				if (p.Value.Entries.Count < p.Value.NumberToPreload) {
					return false;
				}
			}
			return true;
		}
	}

	public static BaseObject SpawnGameObject(string objectName) {
		return Instance.GetNextObject (objectName);
	}

	public static void Fetch(System.Action cb){
		PreloadObject ("BallPlayer");

		if (cb != null) {
			Instance.WaitFinish (cb);
		}
	}

	public void WaitFinish(System.Action cb)
	{
		StartCoroutine(IWaitFinish(cb));
	}

	IEnumerator IWaitFinish(System.Action cb)
	{
		while (!AllObjectsLoaded)
		{
			yield return new WaitForSeconds(0.01f);
		}

		if (cb != null)
		{
			cb();
		}
	}

	public static void ClearAll(){
		foreach (var p in Instance.m_objectsToPreload) {
			PreloaderInfo preloaderInfo = p.Value;
			for (int i = 0; i < preloaderInfo.Entries.Count; i++) {
				if (preloaderInfo.Entries [i].gameObject.activeSelf) {
					preloaderInfo.Entries [i].gameObject.SetActive (false);
					preloaderInfo.Entries [i].transform.SetParent (Instance.transform, false);
				}
			}
		}
	}


	public static void QueryAllActivated(ref List<BaseObject> list){
		list.Clear ();
		foreach (var p in Instance.m_objectsToPreload) {
			PreloaderInfo preloaderInfo = p.Value;
			for (int i = 0; i < preloaderInfo.Entries.Count; i++) {
				if (preloaderInfo.Entries [i].gameObject.activeSelf) {
					list.Add (preloaderInfo.Entries [i]);
				}
			}
		}
	}

	public static void QueryActivated<T>(ref List<T> list) where T : BaseObject{
		list.Clear ();
		string objectName = typeof(T).FullName;
		if (!Instance.m_objectsToPreload.ContainsKey (objectName)) {
			return;
		}
		PreloaderInfo preloaderInfo = Instance.m_objectsToPreload[objectName];
		for (int i = 0; i < preloaderInfo.Entries.Count; i++) {
			if (preloaderInfo.Entries [i].gameObject.activeSelf) {
				list.Add (preloaderInfo.Entries [i] as T);
			}
		}
	}

	private BaseObject GetNextObject(string objectName){
		if (!m_objectsToPreload.ContainsKey (objectName)) {
			Debug.LogErrorFormat ("no exits objectName [{0}] ", objectName);
			return null;
		}
		PreloaderInfo preloaderInfo = m_objectsToPreload[objectName];
		for (int i = 0; i < preloaderInfo.Entries.Count; i++) {
			if (!preloaderInfo.Entries [i].gameObject.activeSelf) {
				preloaderInfo.Entries [i].gameObject.SetActive (true);
				return preloaderInfo.Entries [i];
			}
		}

		return ForceLoadNextObject (objectName);
	}

	private BaseObject GetNextObjectWithAnySkin(string objectName){
		int minUsed = -1;
		int minCount = 0;
		int skinIdx = 1;
		string objectNameSkin = string.Format ("{0}_{1}", objectName, skinIdx);
		while (m_objectsToPreload.ContainsKey (objectNameSkin)) {
			PreloaderInfo preloaderInfo = m_objectsToPreload[objectNameSkin];
			for (int i = 0; i < preloaderInfo.Entries.Count; i++) {
				if (!preloaderInfo.Entries [i].gameObject.activeSelf) {
					preloaderInfo.Entries [i].gameObject.SetActive (true);
					return preloaderInfo.Entries [i];
				}
			}

			if (minUsed == -1 || preloaderInfo.Entries.Count < minCount) {
				minUsed = skinIdx;
				minCount = preloaderInfo.Entries.Count;
			}

			skinIdx++;
			objectNameSkin = string.Format ("{0}_{1}", objectName, skinIdx);
		}

		if (minUsed == -1) {
			Debug.LogErrorFormat ("no exits objectName with skin [{0}] ", objectName);
			return null;
		}


		return ForceLoadNextObject (string.Format ("{0}_{1}", objectName, minUsed));
	}

	IEnumerator ILoadNextObject(string objectName)
	{
		yield return new WaitForSeconds(0.01f);
		PreloaderInfo preloaderInfo = m_objectsToPreload[objectName];

		BaseObject clone = Instantiate (preloaderInfo.Sample, transform);
		preloaderInfo.Entries.Add (clone);
		clone.gameObject.SetActive (false);
		Debug.LogFormat ("ILoadNextObject [{0}] {1}/{2}", objectName, preloaderInfo.Entries.Count, preloaderInfo.NumberToPreload);
		m_bBusing = false;
	}

	BaseObject ForceLoadNextObject(string objectName)
	{				
		PreloaderInfo preloaderInfo = m_objectsToPreload[objectName];
		Debug.LogFormat ("ForceLoadNextObject [{0}] when pool {1}/{2}", objectName, preloaderInfo.Entries.Count, preloaderInfo.NumberToPreload);

		BaseObject clone = Instantiate (preloaderInfo.Sample, transform);
		preloaderInfo.Entries.Add (clone);
		return clone;
	}

	bool m_bBusing;
	// Update is called once per frame
	void Update () {
		if (m_bBusing) {
			return;
		}
		foreach (var p in m_objectsToPreload) {
			if (p.Value.Entries.Count < p.Value.NumberToPreload) {
				m_bBusing = true;
				StartCoroutine (ILoadNextObject (p.Key));
				break;
			}
		}
	}
}
