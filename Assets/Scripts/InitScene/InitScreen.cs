using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class InitScreen : MonoBehaviour {
	[SerializeField]
	private Image m_tapImage;

	// Use this for initialization
	void Start () {
		//LeanTween.value(m_tapImage.gameObject, 0, 2, 0.5f).setLoopPingPong().setOnUpdate((val) => {
		//	m_tapImage.color = new Color(1, 1, 1, val > 1 ? 1 : val);
		//});
		GameServerAPI.Instance.Init();
	}

	void OnDestroy() {
		//LeanTween.cancel(m_tapImage.gameObject);
	}


	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			Main.Instance.LoadScene("MainMenu");
		}	
	}
}
