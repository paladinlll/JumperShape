using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CommonPopup : MonoBehaviour {
	[SerializeField]
	TextMeshProUGUI m_headerText;

	[SerializeField]
	TextMeshProUGUI m_msgText;

	[SerializeField]
	Button m_okButton;

	[SerializeField]
	Button m_cancelButton;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}


	public void ShowOkPopup(string header, string msg, Action okCB = null)
	{
		m_cancelButton.gameObject.SetActive(false);
		m_okButton.gameObject.SetActive(true);
		m_headerText.text = header;
		m_msgText.text = msg;

		m_okButton.onClick.RemoveAllListeners();
		m_okButton.onClick.AddListener(() =>
			{
				if (okCB != null)
				{
					okCB();
				}
				DestroyObject(gameObject);
			}
		);			
	}

	public void ShowOkCancelPopup(string header, string msg, Action okCB = null, Action cancelCB = null)
	{
		m_cancelButton.gameObject.SetActive(true);
		m_okButton.gameObject.SetActive(true);
		m_headerText.text = header;
		m_msgText.text = msg;

		m_okButton.onClick.RemoveAllListeners();
		m_okButton.onClick.AddListener(() =>
			{
				if (okCB != null)
				{
					okCB();
				}
				DestroyObject(gameObject);
			}
		);

		m_cancelButton.onClick.RemoveAllListeners();
		m_cancelButton.onClick.AddListener(() =>
			{
				if (cancelCB != null)
				{
					cancelCB();
				}
				DestroyObject(gameObject);
			}
		);
	}
}
