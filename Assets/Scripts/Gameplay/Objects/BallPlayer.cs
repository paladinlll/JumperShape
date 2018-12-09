using System.Collections;
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

	public override void Init()
	{		
		m_bJumping = false;
		m_canKeepJump = false;
		m_jumpTime = 0.0f;
		m_releaseJumpTime = 0.0f;
		rigidbody2D.gravityScale = Gameplay.Instance.GameUnitScaled;
	}

	public void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Ground") {
			if (m_jumpTime > 0.1f) {
				EndJump ();
			}
		} else if (coll.gameObject.tag == "Obstacle") {
			Gameplay.Instance.EndGame (false);
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
			if (m_jumpTime < 0.6f) {
				if (m_canKeepJump) {
					m_releaseJumpTime = m_jumpTime * 1.1f;
				} else {
					return !m_canKeepJump;
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


	}

	public override void OnGameStep (float dtTime) {		
		Vector2 vel = rigidbody2D.velocity;
		if (m_bJumping) {	
			m_jumpTime += dtTime;
			float t = m_jumpTime;
			float durarion = 0.45f + m_releaseJumpTime;
			float maxYFactor = 10 * durarion;
			float nextY = m_beginJumpY + maxYFactor * (0.25f - (t / durarion - 0.5f) * (t / durarion - 0.5f));


			vel.y = ((nextY - m_mapPos.y) / dtTime) * Gameplay.Instance.GameUnitScaled;
			rigidbody2D.velocity = vel;

			m_maxYJump = Mathf.Max (m_maxYJump, mapPos.y);

			//Keep force x when not falling back.
			if (vel.x >= 0.0f) {
				vel.x = Gameplay.Instance.GameUnitScaled * Gameplay.Instance.GetCurrenMoveXSpeed ();
			}
		} else {
			vel.x = Gameplay.Instance.GameUnitScaled * Gameplay.Instance.GetCurrenMoveXSpeed ();
		}

		rigidbody2D.velocity = vel;
		Vector2 nextMapPos = this.mapPos + rigidbody2D.velocity * dtTime / Gameplay.Instance.GameUnitScaled;
		this.SetMapPos(nextMapPos);

		ProcessJump ();
	}
}
