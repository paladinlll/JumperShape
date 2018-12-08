using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class JsonHelper
{
    public static string GetString(JToken node, string key, string defaultVal = "")
    {
        if (node == null || node[key] == null)
        {
            return defaultVal;        
        }
		if (node[key].Type == JTokenType.String)
		{
			return (string)node[key];
		}
        return node[key].ToString();
    }

    public static float GetFloat(JToken node, string key, float defaultVal = 0)
    {
        if (node == null || node[key] == null)
        {
            return defaultVal;        
        }
		string strVal = "";
		if (node[key].Type == JTokenType.String)
		{
			strVal = (string)node[key];
		}
		else
		{
			strVal = node[key].ToString();
		}

        int off = 0;
        for (; off < strVal.Length; off++)
        {
            if (char.IsDigit(strVal[off]))
            {
                break;
            }
        }
        if (off >= strVal.Length)
        {
            return defaultVal;
        }
        else if (off > 0)
        {
            strVal = strVal.Substring(off);
        }
        float val = defaultVal;
        float.TryParse(strVal, out val);
        return val;
    }

    public static int GetInt(JToken node, string key, int defaultVal = 0)
    {
        if (node == null || node[key] == null)
        {
            return defaultVal;        
        }
		if (node[key].Type == JTokenType.Integer)
        {
            return (int)node[key];
        }

		string strVal = "";
		if (node[key].Type == JTokenType.String)
		{
			strVal = (string)node[key];
		}
		else
		{
			strVal = node[key].ToString();
		}
		
        int off = 0;
        for (; off < strVal.Length; off++)
        {
            if (char.IsDigit(strVal[off]) || strVal[off] == '-')
            {
                break;
            }
        }
        if (off >= strVal.Length)
        {
            return defaultVal;
        }
        else if (off > 0)
        {
            strVal = strVal.Substring(off);
        }
        int val = defaultVal;
        int.TryParse(strVal, out val);
        return val;
    }

	public static long GetLong(JToken node, string key, long defaultVal = 0)
	{
		if (node == null || node[key] == null)
		{
			return defaultVal;        
		}
		if (node[key].Type == JTokenType.Integer)
		{
			return (long)node[key];
		}

		string strVal = node[key].ToString();
		int off = 0;
		for (; off < strVal.Length; off++)
		{
			if (char.IsDigit(strVal[off]) || strVal[off] == '-')
			{
				break;
			}
		}
		if (off >= strVal.Length)
		{
			return defaultVal;
		}
		else if (off > 0)
		{
			strVal = strVal.Substring(off);
		}
		long val = defaultVal;
		long.TryParse(strVal, out val);
		return val;
	}

    public static bool GetBool(JToken node, string key, bool defaultVal = false)
    {
        if (node == null || node[key] == null)
        {
            return defaultVal;        
        }
		if (node[key].Type == JTokenType.Integer)
		{
			return (int)node[key] != 0;
		}
        return (bool)(node[key]);
    }

	public static Color GetColor(JToken node, string key, Color defaultVal)
	{
		string hexStr = GetString(node, key);
		if (hexStr.Length != 6)
		{
			return defaultVal;
		}
		try
		{
			byte r = byte.Parse(hexStr.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hexStr.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hexStr.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			return new Color(r / 255.0f,g / 255.0f,b / 255.0f, 1.0f);
		}
		catch(Exception)
		{
		}
		return defaultVal;
	}
}


