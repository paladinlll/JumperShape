using System.Collections;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Security.Cryptography;


using System.Collections.Generic;
using System.Linq;

using System.Net;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Editor
{
    public class CustomMenu
    {        
		static void CheckPath(string path)
		{
			if (!Directory.Exists(path))
			{
				// This path is a directory
				Directory.CreateDirectory(path);
			}
		}

		static string GetProjectName()
		{
			return "Ludo2Dices";
		}

#if UNITY_ANDROID
		[MenuItem("Asset Bundles/Build Android APK", false, 51)]
		public static void BuildAndroidGame()
		{
			const string googleBundleIdentifier =  "com.larkgames.dancingtiles";
			//Set player setting
			PlayerSettings.applicationIdentifier = googleBundleIdentifier;

			//PlayerSettings.Android.keystoreName = "keystore.keystore";
			//PlayerSettings.Android.keystorePass = "123456";
			//PlayerSettings.Android.keyaliasName = "pianotiles4";
			//PlayerSettings.Android.keyaliasPass = "123456";

			string outputDir = "build/android/";
			CheckPath(outputDir);
			string[] scenes = { 
				"Assets/Scenes/MainMenu.unity",
				"Assets/Scenes/Gameplay.unity",
				"Assets/Scenes/Result.unity",
				"Assets/Scenes/ResultBattle.unity",

			};



			//HACK rename streamming asset before build ( Author cheat code master Tri )
			BuildPipeline.BuildPlayer(scenes, outputDir + GetProjectName() + "_" + Application.version.Replace(".", "") + ".apk", BuildTarget.Android, BuildOptions.Il2CPP);

		}
#endif
		[MenuItem("Test/ClearPrefs")]
		static void ClearPrefs()
		{
			
		}

		[MenuItem("Test/SetLocations")]
		static void SetLocations()
		{
			int mapW = 15;
			int mapH = 15;
			GameObject tableGO = GameObject.Find("table");
			if (tableGO == null)
			{
				Debug.LogError("Not found table");
			}
			foreach (Transform child in tableGO.transform) {
				GameObject.DestroyImmediate(child.gameObject);
			}

			Debug.Log("Found table");
			RectTransform tableRect = tableGO.GetComponent<RectTransform>();
			float cellW = 34;
			float cellPad = 2;

			for (int i = 0; i < mapH; i++)
			{
				for (int j = 0; j < mapW; j++)
				{
					if (Mathf.Abs(i - 7) <= 1 || Mathf.Abs(j - 7) <= 1)
					{
						GameObject cellGO = new GameObject(string.Format("{0}x{1}", j, i));
						Image cellImg = cellGO.AddComponent<Image>();
						cellGO.transform.SetParent(tableRect.transform);
						cellImg.rectTransform.sizeDelta = new Vector2(cellW - cellPad, cellW - cellPad);
						cellImg.rectTransform.anchoredPosition = new Vector2(j * cellW - cellW * 7.0f + cellPad, i * cellW - cellW * 7.0f - cellPad);
						cellImg.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);

						HorizontalLayoutGroup cellLayout = cellGO.AddComponent<HorizontalLayoutGroup>();
						cellLayout.childAlignment = TextAnchor.MiddleCenter;
						cellLayout.childControlWidth = true;
						cellLayout.childForceExpandWidth = true;
						cellLayout.childControlHeight = true;
						cellLayout.childForceExpandHeight = true;
						cellLayout.padding = new RectOffset(10, 10, 0, 0);
					}
				}
			}

			Vector2[] spotPos =
			{
					new Vector2(-cellW * 3.9f, -cellW * 6.3f),
					new Vector2(-cellW * 3.9f, cellW * 2.7f),
					new Vector2(cellW * 5.2f, cellW * 2.7f),
					new Vector2(cellW * 5.2f, -cellW * 6.3f),
			};
			cellW = 40;

			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					GameObject cellGO = new GameObject(string.Format("spot_{0}x{1}", i, j));
					Image cellImg = cellGO.AddComponent<Image>();
					cellGO.transform.SetParent(tableRect.transform);
					cellImg.rectTransform.sizeDelta = new Vector2(cellW - cellPad, cellW - cellPad);
					cellImg.rectTransform.anchoredPosition = spotPos[i] + new Vector2((j - 2.0f) * cellW, 0);
					cellImg.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
				}
			}
		}


    }
}
