using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieObject : MonoBehaviour {
	public RectTransform m_rect;

	public Transform m_dir;
	// Use this for initialization
	void Start () {

	}

	public void OnTriggerEnter2D(Collider2D other){
		Debug.LogFormat ("{0}", other.gameObject.name);
		Vector2 vec = (Vector2)other.transform.position - (Vector2)transform.position;
		m_dir.localRotation = Quaternion.AngleAxis (Mathf.Atan2 (vec.y, vec.x) * Mathf.Rad2Deg, Vector3.forward);

	}

//	public void OnCollisionEnter2D(Collision2D coll) {
//		if (coll.contacts.Length == 1) {
//			Vector2 vec = coll.contacts [0].point - (Vector2)transform.position;
//			m_dir.localRotation = Quaternion.AngleAxis (Mathf.Atan2 (vec.y, vec.x) * Mathf.Rad2Deg, Vector3.forward);
//			Debug.LogFormat ("{0} - {1}", coll.gameObject.name, vec);
//		} else {
//			Debug.LogFormat ("{0} - {1}", coll.gameObject.name, coll.contacts.Length);
//		}
//	}


	// Update is called once per frame
	void Update () {
		float dt = Time.deltaTime;
		Vector2 nextPos = m_rect.anchoredPosition;
		if (Input.GetKey (KeyCode.DownArrow)) {
			nextPos.y -= 500 * dt;
		} else if (Input.GetKey (KeyCode.UpArrow)) {
			nextPos.y += 500 * dt;
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			nextPos.x -= 500 * dt;
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			nextPos.x += 500 * dt;
		}
		m_rect.anchoredPosition = nextPos;

	}
}
