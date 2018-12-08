using System;
using UnityEngine;
using System.Collections.Generic;

public class GameObjectPool : MonoBehaviour
{	
	List<GameObject> m_samples;

	public List<GameObject> samples {
		get {
			if (m_samples == null) {
				m_samples = new List<GameObject> ();
				foreach (Transform p in transform) {
					m_samples.Add (p.gameObject);
				}
			}
			return m_samples;
		}
	}
		
	Transform m_defaultHolder;
	public void SerDefaultHolder(Transform defaultHolder){
		m_defaultHolder = defaultHolder;
	}

	class PoolEntry
	{
		public GameObject gameObject;
		public string spawnName;

		public PoolEntry(GameObject _go, string _name){
			gameObject = _go;
			spawnName = _name;
		}
	};

	List<PoolEntry> m_pools = new List<PoolEntry>();

	void Start () {
		foreach (Transform p in transform) {
			p.gameObject.SetActive (false);
		}
	}

	public void QueryActivated<T>(ref List<T> list)
	{
		list.Clear ();
		for (int i=0;i<m_pools.Count;i++)
		{
			if (m_pools[i].gameObject.activeSelf)
			{
				T t = m_pools [i].gameObject.GetComponent<T> ();
				if (t != null) {
					list.Add (t);
				}
			}
		}
	}

	public void ClearAll()
	{
		foreach (var entry in m_pools)
		{
			entry.gameObject.SetActive(false);
		}
	}

	public T New<T>(string objectName, Transform pr = null) where T : MonoBehaviour
	{		
		GameObject sampleGO = null;
		foreach (var p in samples) {
			if (p.name == objectName) {
				sampleGO = p;
				break;
			}
		}
		return New<T> (sampleGO, pr);
	}

	public T New<T>(Transform pr = null) where T : MonoBehaviour
	{
		string className = typeof(T).FullName;
		GameObject sampleGO = null;
		foreach (var p in samples) {
			if (p.name == className) {
				sampleGO = p;
				break;
			}
		}
		return New<T> (sampleGO, pr);
	}

	private T New<T>(GameObject sampleGO, Transform pr = null) where T : MonoBehaviour
	{		
		if (sampleGO == null) {
			return null;
		}
		int freeSlot = -1;
		for (int i=0;i<m_pools.Count;i++)
		{
			if (!m_pools[i].gameObject.activeSelf)
			{
				if (m_pools [i].spawnName == sampleGO.name) {
					freeSlot = i;
					break;
				}
			}
		}

		GameObject customShape = null;
		if (freeSlot != -1)
		{
			customShape = m_pools[freeSlot].gameObject;
		}
		else
		{
			customShape = Instantiate(sampleGO, pr != null ? pr : m_defaultHolder);

			m_pools.Add(new PoolEntry(customShape, sampleGO.name));
		}
		customShape.transform.localScale = Vector3.one;
		customShape.gameObject.SetActive(true);
		return customShape.GetComponent<T>();
	}

	public int CountActivated
	{
		get {
			int num = 0;
			for (int i=0;i<m_pools.Count;i++)
			{
				if (m_pools[i].gameObject.activeSelf)
				{
					num++;
				}
			}
			return num;
		}
	}
}