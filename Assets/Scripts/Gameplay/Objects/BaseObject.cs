using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour {

	private bool m_spriteRendererChecked = false;
	private SpriteRenderer m_spriteRenderer;
	public SpriteRenderer spriteRenderer
	{
		get
		{
			if (!m_spriteRendererChecked)
			{
				m_spriteRendererChecked = true;
				m_spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
			}
			return m_spriteRenderer;
		}
	}

	// Use this for initialization
	void Start () {
    
    }
	

  
}
