﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPlayer : BaseObject {
	public RectTransform m_rect;

	protected Rigidbody2D m_rigidbody2D;

	public Rigidbody2D rigidbody2D
	{
		get
		{
			if (m_rigidbody2D == null)
			{
				m_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
			}
			return m_rigidbody2D;
		}
	}

	protected bool m_canKeepJump;
	protected bool m_bJumping;

	protected float m_maxYJump;
	protected float m_jumpTime;
	protected float m_beginJumpY;
	protected float m_releaseJumpTime;

	public Transform m_dir;

	protected bool m_pendingTap;

	// Use this for initialization
	void Start () {

	}

	public virtual void Init()
	{		
		m_bJumping = false;
		m_canKeepJump = false;
		m_jumpTime = 0.0f;
		m_releaseJumpTime = 0.0f;
		rigidbody2D.gravityScale = Gameplay.Instance.GameUnitScaled;
	}

//	public void OnTriggerEnter2D(Collider2D other){
//		Debug.LogFormat ("{0}", other.gameObject.name);
//		Vector2 vec = (Vector2)other.transform.position - (Vector2)transform.position;
//		m_dir.localRotation = Quaternion.AngleAxis (Mathf.Atan2 (vec.y, vec.x) * Mathf.Rad2Deg, Vector3.forward);
//
//	}

	public void OnCollisionEnter2D(Collision2D coll) {
//		if (coll.contacts.Length == 1) {
//			Vector2 vec = coll.contacts [0].point - (Vector2)transform.position;
//			m_dir.localRotation = Quaternion.AngleAxis (Mathf.Atan2 (vec.y, vec.x) * Mathf.Rad2Deg, Vector3.forward);
//			Debug.LogFormat ("{0} - {1}", coll.gameObject.name, vec);
//		} else {
//			Debug.LogFormat ("{0} - {1}", coll.gameObject.name, coll.contacts.Length);
//		}

		//BaseObject baseObject = coll.gameObject.GetComponent<BaseObject> ();
		if (coll.gameObject.tag == "Ground") {
			if (m_jumpTime > 0.1f) {
				EndJump ();
			}
		}			
	}

	public void SignalJump(){
		m_pendingTap = true;
	}

	public void ReleaseJump(){
		if (m_pendingTap) {
			m_pendingTap = false;
		}
	}

	public void BreakJump()
	{		
		if (m_canKeepJump) {			
			m_canKeepJump = false;
		}
	}

	public void EndJump()
	{
		if (m_bJumping) {					
			m_bJumping = false;
			rigidbody2D.gravityScale = Gameplay.Instance.GameUnitScaled;
		}
	}

	public bool DoJump()
	{
		if (m_bJumping) {
			if (m_jumpTime < 0.45f) {
				if (m_canKeepJump) {
					m_releaseJumpTime = m_jumpTime * 1.1f;
				} else {
					return false;
				}
			} else {
				return !m_canKeepJump;
			}
		} else {
			rigidbody2D.gravityScale = 0;
			m_releaseJumpTime = 0.01f;
			m_canKeepJump = true;
			m_bJumping = true;
			m_jumpTime = 0.0f;
			m_beginJumpY = m_mapPos.y;
		}
		return true;
	}

	public void ProcessJump ( ) {		
		if (m_pendingTap) {				
			m_pendingTap = DoJump ();
		} else if(m_bJumping) {
			BreakJump ();
		}
	}

	// Update is called once per frame
	void Update () {
//		float dt = Time.deltaTime;
//		Vector2 nextPos = m_rect.anchoredPosition;
//		if (Input.GetKey (KeyCode.DownArrow)) {
//			nextPos.y -= 500 * dt;
//		} else if (Input.GetKey (KeyCode.UpArrow)) {
//			nextPos.y += 500 * dt;
//		} else if (Input.GetKey (KeyCode.LeftArrow)) {
//			nextPos.x -= 500 * dt;
//		} else if (Input.GetKey (KeyCode.RightArrow)) {
//			nextPos.x += 500 * dt;
//		}
//		m_rect.anchoredPosition = nextPos;

	}

	public override void OnGameStep (float dtTime) {		
		Vector2 vel = rigidbody2D.velocity;
		if (m_bJumping) {	
			m_jumpTime += dtTime;
			float t = m_jumpTime;
			float durarion = 2 * (1.1f + m_releaseJumpTime);
			float maxYFactor = 2 * durarion;
			durarion /= Gameplay.Instance.GetCurrenMoveXSpeed ();
			float nextY = m_beginJumpY + maxYFactor * (0.25f - (t / durarion - 0.5f) * (t / durarion - 0.5f));


			vel.y = ((nextY - m_mapPos.y) / dtTime) * Gameplay.Instance.GameUnitScaled;
			rigidbody2D.velocity = vel;

			m_maxYJump = Mathf.Max (m_maxYJump, mapPos.y);
		} else {
			vel.x = Gameplay.Instance.GameUnitScaled * Gameplay.Instance.GetCurrenMoveXSpeed ();
			rigidbody2D.velocity = vel;
		}

		Vector2 nextMapPos = this.mapPos + rigidbody2D.velocity * dtTime / Gameplay.Instance.GameUnitScaled;
		this.SetMapPos(nextMapPos);

		ProcessJump ();
	}
}