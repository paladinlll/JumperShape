using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDTO;

public class Gameplay : MonoBehaviour {
	static Gameplay s_instance;
	public static Gameplay Instance
	{
		get
		{
			if (s_instance == null)
			{
				s_instance = FindObjectOfType<Gameplay>();
			}
			return s_instance;
		}
	}

	[SerializeField]
	Canvas m_mainCanvas;

	[SerializeField]
	ParallaxBkgController m_parallaxBackground;

	[SerializeField]
	GameObjectPool m_gameObjectPool;

	[SerializeField]
	RectTransform m_objectHolder;

	[SerializeField]
	GameObject m_sampleGround;

	List<GameObject> m_grounds = new List<GameObject>();

	float m_mapMaxView = 0;
	float m_mapWeight = 0;

	List<GameObject> m_activeObjects = new List<GameObject>();

	LevelDTO m_workingLevelDTO = new LevelDTO();
	int m_levelSubStep;

	bool m_bEnded;
	bool m_bPausing;

	// Use this for initialization
	void Start () {
		Reset ();
	}

	public void Reset()
	{
		m_bEnded = false;
		m_bPausing = false;
		m_workingLevelDTO.Clear ();
		m_levelSubStep = 0;
	}

	public void EndGame(bool bWin)
	{
		if (m_bEnded) {
			return;
		}
		m_bEnded = true;
	}

	private void UpdateInput()
	{
		#if UNITY_EDITOR || UNITY_STANDALONE
		if (Input.GetMouseButtonDown(0))
		{
			OnTapDown(0, Input.mousePosition);
		}
		else if (Input.GetMouseButton(0))
		{
			OnTapMove(0, Input.mousePosition);
		}
		if (Input.GetMouseButtonUp(0))
		{
			OnTapUp(0);
		}
		#else

		int nbTouches = Input.touchCount;

		if(nbTouches > 0)
		{
		//for (int i = 0; i < nbTouches; i++)
		{
		Touch touch = Input.GetTouch(0);

		TouchPhase phase = touch.phase;

		switch(phase)
		{
		case TouchPhase.Began:
		OnTapDown(touch.fingerId, touch.position);
		break;
		case TouchPhase.Moved:
		OnTapMove(touch.fingerId, touch.position);
		//print("Touch index " + touch.fingerId + " has moved by " + touch.deltaPosition);
		break;
		//					case TouchPhase.Stationary:
		//						print("Touch index " + touch.fingerId + " is stationary at position " + touch.position);
		//						break;
		case TouchPhase.Ended:
		//						Debug.Log("Touch index " + touch.fingerId + " ended at position " + touch.position);
		OnTapUp(touch.fingerId);
		break;
		//					case TouchPhase.Canceled:
		//						print("Touch index " + touch.fingerId + " cancelled");
		//						break;
		}
		}
		}
		#endif
	}

	public void OnTapDown(int touchId, Vector2 toughPos)
	{
		RectTransform mainRect = m_mainCanvas.transform as RectTransform;
		Vector2 canvasPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(mainRect, toughPos, m_mainCanvas.worldCamera, out canvasPos);

	}
	public void OnTapUp(int touchId)
	{
	}
	public void OnTapMove(int touchId, Vector2 toughPos)
	{
		RectTransform mainRect = m_mainCanvas.transform as RectTransform;
		Vector2 canvasPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(mainRect, toughPos, m_mainCanvas.worldCamera, out canvasPos);
	}

	public void OnQuit()
	{
		if (m_bPausing) {
			return;
		}
		m_bPausing = true;
		UIHelper.Instance.New<CommonPopup> (transform).ShowOkCancelPopup ("", "Are you want to quit?",
			() => {
				Main.Instance.LoadScene("MainMenu");
			},
			() => {
				m_bPausing = false;
			}
		);

	}

	private void LoadLevelMap() {;
		List<int> heightMaps = new List<int>();
		for (int i = 0; i < 12; i++)
		{
			heightMaps.Add(i == 4 ? -1 : 0);
		}

		m_workingLevelDTO.SetHeightMap (heightMaps);

		int maxGround = heightMaps.Count;
		int startGround = 0;
		while (startGround < maxGround)
		{
			int lineHeight = heightMaps[startGround];
			int endGround = startGround + 1;
			while (endGround < heightMaps.Count && heightMaps[startGround] == heightMaps[endGround])
			{
				endGround++;
			}
			int numSpace = (endGround - startGround);
			if (lineHeight >= 0)
			{
				PlacementObjectDTO groundInfo = new PlacementObjectDTO();
				groundInfo.ObjectType = ObjectType.Ground;
				groundInfo.SizeX = numSpace;
				groundInfo.SizeY = lineHeight * GameDefines.HEIGHT_PAD + (GameDefines.HEIGHT_PAD * 2);
				groundInfo.MapX = startGround;
				groundInfo.MapY = lineHeight;
				m_workingLevelDTO.PlacementObjects.Add(groundInfo);
			}				
			startGround = endGround;
		}
	}

	private GameObject NewGround(){
		int freeSlot = -1;
		for (int i = 0; i < m_activeObjects.Count; i++) {
			if (!m_activeObjects [i].activeSelf) {
				freeSlot = i;
				break;
			}
		}

		GameObject ground = null;
		if (freeSlot != -1) {
			ground = m_activeObjects [freeSlot];
		} else {
			ground = Instantiate (m_sampleGround, m_objectHolder);
			ground.transform.localScale = Vector2.one;
			m_activeObjects.Add (ground);
		}

		ground.SetActive (true);
		return ground;
	}

	private void GenerateZoneMap(float initDist)
	{
		if (m_levelSubStep >= m_workingLevelDTO.HeightMap.Length) {
			m_workingLevelDTO.Clear();
			m_levelSubStep = 0;
			LoadLevelMap();
			//GameObjectPreloader.PreloadLevel(m_workingLevelConfig, CountZombie());
		} else {
			initDist -= m_levelSubStep;
		}

		//int maxGround = 20;
		List<int> heightMaps = m_workingLevelDTO.GetHeightMap();
		int maxGround = Mathf.Min(10, heightMaps.Count - m_levelSubStep);

		List<PlacementObjectDTO> placementObjectDTOs = m_workingLevelDTO.PlacementObjects;
		for (int i = 0; i < placementObjectDTOs.Count; i++) {
			if (placementObjectDTOs [i].MapX < m_levelSubStep || placementObjectDTOs [i].MapX > m_levelSubStep + maxGround) {
				continue;
			}
			ObjectType objectType = placementObjectDTOs [i].ObjectType;

			Vector2 zoneSize = new Vector2(placementObjectDTOs[i].SizeX, placementObjectDTOs[i].SizeY);
			Vector2 spawnPos = new Vector2(initDist + placementObjectDTOs[i].MapX, placementObjectDTOs[i].MapY * GameDefines.HEIGHT_PAD);

			if (objectType == ObjectType.Ground)
			{
				GameObject ground = NewGround();
				ground.gameObject.GetComponent<RectTransform>().anchoredPosition = spawnPos * GameDefines.GAME_UNIT;
				ground.transform.localScale = zoneSize;
			}

		}
//		GameObject ground = NewGround();
//		ground.transform.localPosition = new Vector2(initDist, 0) * GameDefines.GAME_UNIT;

		m_levelSubStep += maxGround;
		m_mapMaxView += maxGround;
	}

	private void QuerryAndChop(float chopX)
	{
		float dtTime = Time.deltaTime;

		Debug.LogFormat ("QuerryAndChop {0} = {1} / {2}", chopX / dtTime, chopX, dtTime);

		for (int i = 0; i < m_activeObjects.Count; i++) {
			if (!m_activeObjects [i].activeSelf) {
				continue;
			}
			GameObject p = m_activeObjects [i];
			Vector2 nextPos = p.transform.localPosition;
			nextPos.x -= chopX * GameDefines.GAME_UNIT;
			p.transform.localPosition = nextPos;

			if (nextPos.x < -30 * GameDefines.GAME_UNIT) {
				p.gameObject.SetActive (false);
			}
		}
	}

	// Update is called once per frame
	void Update () {

		if (m_bEnded) {			
			return;
		}

		if (m_bPausing) {
			return;
		}

		UpdateInput ();

		float chopX = Time.deltaTime * 4;
		m_parallaxBackground.DoScroll (chopX * GameDefines.GAME_UNIT);

		m_mapWeight += chopX;
		QuerryAndChop (chopX);
		if (m_mapMaxView - m_mapWeight < 10) {			
			GenerateZoneMap(m_mapMaxView - m_mapWeight);
		}
	}
}
