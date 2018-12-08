using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelper : MonoBehaviour {
	static UIHelper s_instance = null;
	public static UIHelper Instance
	{
		get
		{
			if (s_instance == null)
			{
				GameObject go = new GameObject("UIHelper");		
				DontDestroyOnLoad(go);

				s_instance = go.AddComponent<UIHelper>();
			}
			return s_instance;
		}
	}

	// Use this for initialization
	void Start () {
		
	}

	public T New<T>(Transform parentHolder) where T : MonoBehaviour
	{
		string className = typeof(T).FullName;

		GameObject commonPopupGO = Instantiate(Resources.Load(string.Format("prefabs/{0}", className) , typeof(GameObject))) as GameObject;
		if (commonPopupGO == null)
		{
			return null;
		}
		commonPopupGO.gameObject.SetActive(true);
		T commonPopup = commonPopupGO.GetComponent<T>();
		commonPopup.transform.SetParent(parentHolder, false);
		commonPopup.transform.localScale = Vector3.one;
		RectTransform rt = commonPopup.GetComponent<RectTransform> ();
		if (rt != null) {
			rt.anchoredPosition = Vector3.zero;
		} else {
			commonPopup.transform.localPosition = Vector3.zero;
		}
		return commonPopup;
	}
//
//	public CommonPopup OpenCommonPopup(Transform parent)
//	{
//		GameObject commonPopupGO = Instantiate(Resources.Load("CommonPopup", typeof(GameObject))) as GameObject;
//		CommonPopup commonPopup = commonPopupGO.GetComponent<CommonPopup>();
//		commonPopup.transform.SetParent(transform, false);
//		commonPopup.transform.localScale = Vector3.one;
//		commonPopup.transform.localPosition = Vector3.zero;
//
//		return commonPopup;
//	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
