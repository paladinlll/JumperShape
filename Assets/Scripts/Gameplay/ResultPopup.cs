using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultPopup : MonoBehaviour {
	[SerializeField]
	TextMeshProUGUI m_scoreText;

	// Use this for initialization
	void Start () {
		
	}

	public void Show(int score) {
		gameObject.SetActive (true);
		m_scoreText.text = score.ToString ();

		WWWForm form = new WWWForm ();
		form.AddField ("userName", OnlinePlayerProfile.Instance.dataInfo.m_localId);
		form.AddField ("score", score);
		GameServerAPI.Instance.PostAPI ("leaderboard", form, (err, data) => {
			if(err != null){
				Debug.LogError(err.errorMessage);
			}
		});
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
