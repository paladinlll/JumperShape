using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingLayer : MonoBehaviour {
	
	[SerializeField]
	Image m_loadingFillImage;

	// Use this for initialization
	void Start () {
		
	}


	public void Show()
	{
		gameObject.SetActive(true);

	}

	public void UpdateProcessStep(float process)
	{
		m_loadingFillImage.fillAmount = process;
	}

	public void UpdateProcessStep(int step, int max)
	{
		m_loadingFillImage.fillAmount = step * 1.0f / max;
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
