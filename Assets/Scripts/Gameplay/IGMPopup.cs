using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGMPopup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void Show() {
		gameObject.SetActive (true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
