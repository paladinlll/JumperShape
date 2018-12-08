
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Reflection;


public class DataConfigMrg : MonoBehaviour
{
    static DataConfigMrg s_instance;

    public static DataConfigMrg Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<DataConfigMrg>();
                if (s_instance == null)
                {
                    GameObject newGO = new GameObject("DataConfigMrg");
                    newGO.AddComponent<DataConfigMrg>();
                    s_instance = newGO.GetComponent<DataConfigMrg>();
					s_instance.Init ();
                }
            }
            return s_instance;
        }
    }


    public void Init()
	{		

	}
}
