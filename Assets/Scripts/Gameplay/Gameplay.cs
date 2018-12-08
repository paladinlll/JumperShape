using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDTO;
using TMPro;

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
	LoadingLayer m_loadingLayer;

	[SerializeField]
	Canvas m_mainCanvas;

	public float CanvasScale {
		get {
			return m_mainCanvas.transform.localScale.x;	
		}
	}

	public float GameUnitScaled {
		get {
			return GameDefines.GAME_UNIT * CanvasScale;
		}
	}

	[SerializeField]
	ParallaxBkgController m_parallaxBackground;

	[SerializeField]
	RectTransform m_objectHolder;

	BallPlayer m_ballPlayer;

	[SerializeField]
	GameObject m_prepareLayer;

	[SerializeField]
	ResultPopup m_resultPopup;

	[SerializeField]
	TextMeshProUGUI m_scoreText;

	float m_mapMaxView = 0;
	float m_mapWeight = 0;

	int m_score;
	//List<GameObject> m_activeObjects = new List<GameObject>();

	LevelDTO m_workingLevelDTO = new LevelDTO();
	int m_levelSubStep;

	bool m_bEnded;
	bool m_bPausing;

	public float GetCurrenMoveXSpeed()
	{
		return 7;
	}

	// Use this for initialization
	void Start () {
		Physics2D.autoSimulation = false;
		Reset ();

		//Load minimium object. Will show loading screen until it has done.
		m_loadingLayer.Show();
		GameObjectPreloader.Fetch(() =>{
			m_loadingLayer.Hide();
		});


	}

	public void Reset()
	{
		GameObjectPreloader.ClearAll ();
		m_prepareLayer.SetActive (true);
		m_resultPopup.gameObject.SetActive (false);
		m_bEnded = true;
		m_bPausing = false;
		m_score = 0;
		m_scoreText.text = "";
	}

	public void GainScore(){
		m_score++;
		m_scoreText.text = m_score.ToString ();
	}

	public T SpawnGameObject<T>(string className) where T : BaseObject
	{
		BaseObject baseObject = null;
		//string className = typeof(T).FullName;
		switch (className) {
		case "BallPlayer":
		case "SquareObstacleObject":
			baseObject = GameObjectPreloader.SpawnGameObject (className);
			break;
		default:
			Debug.LogErrorFormat("no have [{0}] in preloads resource", className);
			break;
		};

		if (baseObject != null) {
			baseObject.Init ();
			baseObject.transform.SetParent (m_objectHolder.transform, false);
			return baseObject as T;
		}
		return null;
	}

	public void PlayGame(){
		m_prepareLayer.SetActive (false);
		m_bEnded = false;
		m_bPausing = false;

		m_workingLevelDTO.Clear ();
		m_levelSubStep = 0;

		m_ballPlayer = SpawnGameObject<BallPlayer> ("BallPlayer");
		m_ballPlayer.SetMapPos (Vector2.zero);

		m_scoreText.text = m_score.ToString ();
	}

	public void EndGame(bool bWin)
	{
		if (m_bEnded) {
			return;
		}
		m_bEnded = true;
		m_scoreText.text = "";
		m_resultPopup.Show (m_score);
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

		if (m_ballPlayer != null && m_ballPlayer.gameObject.activeSelf) {
			m_ballPlayer.SignalJump ();
		}
	}
	public void OnTapUp(int touchId)
	{
		if (m_ballPlayer != null && m_ballPlayer.gameObject.activeSelf) {
			m_ballPlayer.ReleaseJump ();
		}
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
		int maxJumpDist = (int)GetCurrenMoveXSpeed() - 2;

		List<int> heightMaps = LevelGenerator.RandomZoneMap (5, 5, 5, maxJumpDist);
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
			if (lineHeight < 0) //need jump
			{
				//To force player do jump over [numSpace] unit, We just need set 1 SquareObstacle at middle has enough tall.
				for (int i = 0; i < numSpace; i++) {
					int mid = (numSpace - 1) /2;
					if (i == (int)mid) {
						PlacementObjectDTO groundInfo = new PlacementObjectDTO ();
						groundInfo.ObjectType = ObjectType.SquareObstacle;
						//groundInfo.SizeX = numSpace;
						groundInfo.SizeX = 1;

						groundInfo.SizeY = 0.8f + 0.3f * (mid * mid - (mid - i) * (mid - i));
						groundInfo.MapX = startGround + i;
						groundInfo.MapY = 0;
						m_workingLevelDTO.PlacementObjects.Add (groundInfo);
					}
				}
			}				
			startGround = endGround;
		}
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
			if (placementObjectDTOs [i].MapX < m_levelSubStep || placementObjectDTOs [i].MapX >= m_levelSubStep + maxGround) {
				continue;
			}
			ObjectType objectType = placementObjectDTOs [i].ObjectType;

			Vector2 zoneSize = new Vector2(placementObjectDTOs[i].SizeX, placementObjectDTOs[i].SizeY);
			Vector2 spawnPos = new Vector2(initDist + placementObjectDTOs[i].MapX, placementObjectDTOs[i].MapY * GameDefines.HEIGHT_PAD);

			if (objectType == ObjectType.SquareObstacle)
			{
				BaseObject squareObstacle = SpawnGameObject<BaseObject> ("SquareObstacleObject");
				squareObstacle.SetMapPos(spawnPos);
				squareObstacle.transform.localScale = zoneSize;
			}

		}
		m_levelSubStep += maxGround;
		m_mapMaxView += maxGround;
	}

	private void QuerryAndChop(float chopX)
	{
		float dtTime = Time.deltaTime;

		GameObjectPreloader.QueryAllActivated(ref m_activeObjects);
		for (int i = 0; i < m_activeObjects.Count; i++) {
			if (!m_activeObjects [i].gameObject.activeSelf) {
				continue;
			}
			BaseObject p = m_activeObjects [i];
			Vector2 nextPos = p.mapPos;
			nextPos.x -= chopX;
			p.SetMapPos(nextPos);

//			Vector2 nextPos = p.transform.localPosition;
//			nextPos.x -= chopX * GameDefines.GAME_UNIT;
//			p.transform.localPosition = nextPos;

			if (nextPos.x < -30) {
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
		OnGameStep ();

		UpdateInput ();
	}

	List<BaseObject> m_activeObjects = new List<BaseObject>();
	private float m_dtGameTime = 0;
	private float m_pendingGameTime = 0;
	void OnGameStep()
	{
		m_pendingGameTime += Time.deltaTime;
		int numStep = (int)(m_pendingGameTime / 0.005f);
		if (numStep <= 0) {
			return;
		}
		float dtTime = numStep * 0.005f;
		m_pendingGameTime -= dtTime;

		//limit lag time
		if (dtTime > 0.5f) {
			dtTime = 0.5f;
		}

		m_dtGameTime = dtTime;

		//We just need Physics2D to check collision.
		Physics2D.Simulate(dtTime);

		//The movement will be hadled manual in this
		foreach (var p in m_activeObjects)
		{
			p.OnGameStep(dtTime);
		}

		float chopX = 0;//m_dtGameTime * GetCurrenMoveXSpeed();
		if (m_ballPlayer != null) {
			chopX = m_ballPlayer.mapPos.x;
		}
		m_parallaxBackground.DoScroll (chopX * GameDefines.GAME_UNIT);

		m_mapWeight += chopX;
		QuerryAndChop (chopX);
		if (m_mapMaxView - m_mapWeight < 10) {			
			GenerateZoneMap(m_mapMaxView - m_mapWeight);
		}
	}
}
