﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingLayer : MonoBehaviour {	
	// Use this for initialization
	void Start () {
		
	}

	public void Show()
	{
		gameObject.SetActive(true);

	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
