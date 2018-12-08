using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour {

	private RectTransform m_rectTransform;
	public RectTransform rectTransform
	{
		get
		{
			if (m_rectTransform == null)
			{
				m_rectTransform = gameObject.GetComponent<RectTransform> ();
			}
			return m_rectTransform;
		}
	}

	protected Vector2 m_mapPos;

	public Vector2 mapPos {
		get {
			return m_mapPos;
		}
	}



	public virtual void SetMapPos(Vector2 mapPos){
		m_mapPos = mapPos;
		rectTransform.anchoredPosition = m_mapPos * GameDefines.GAME_UNIT;
	}

	// Use this for initialization
	void Start () {
    
    }
	
	public virtual void OnGameStep (float dtTime) {		
	}
  
}
