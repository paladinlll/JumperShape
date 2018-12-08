using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDefines {	
	public static string[] ROOT_URLS =
	{
		"http://localhost:5000/",
	};

	public const int SAVE_VERSION = 1;
	public const int GAME_UNIT = 120;
	public const float HEIGHT_PAD = 1.2f;


	public const long LOCAL_ERROR_NOT_INITIALIZE = 1;
	public const long LOCAL_ERROR_TIME_OUT = 2;
}
