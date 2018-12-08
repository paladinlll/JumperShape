using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareObstacleObject : BaseObject {
	bool m_bHasScore;
	// Use this for initialization
	void Start () {
		
	}

	public override void Init(){
		m_bHasScore = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnGameStep (float dtTime) {
		if (m_bHasScore &&
		   (m_mapPos.x + 1 + transform.localScale.x) <= 0.0f) {
			m_bHasScore = false;
			Gameplay.Instance.GainScore ();
		}
	}
}
