using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVSheet
{
	List<List<string>> m_data = new List<List<string>>();

	char m_fieldDelimiter;
	char m_textDelimiter;
	public CSVSheet()
	{
		m_fieldDelimiter = ',';
		m_textDelimiter = '"';
	}

	public void LoadContent(string[] lines)
	{
		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];
			List<string> lineData = new List<string>();

			int nbtextDelimiter = 0;
			string colValue = "";
			for (int j = 0; j < line.Length; j++)
			{
				if (nbtextDelimiter == 0)
				{
					if (line[j] == m_textDelimiter)
					{
						nbtextDelimiter++;
					}

					if (line[j] == m_fieldDelimiter)
					{
						if (colValue.Length >= 2 && colValue[0] == m_textDelimiter && colValue[colValue.Length - 1] == m_textDelimiter)
						{
							colValue = colValue.Substring(1, colValue.Length - 2);
						}


						lineData.Add(colValue.Replace(m_textDelimiter.ToString() + m_textDelimiter.ToString(), m_textDelimiter.ToString()));
						colValue = "";
					}
					else
					{
						colValue += line[j];
					}
				}
				else
				{
					if (line[j] == m_textDelimiter)
					{
						nbtextDelimiter--;
					}
					colValue += line[j];
				}
			}
			if (colValue.Length >= 2 && colValue[0] == m_textDelimiter && colValue[colValue.Length - 1] == m_textDelimiter)
			{
				colValue = colValue.Substring(1, colValue.Length - 2);
			}
			lineData.Add(colValue.Replace(m_textDelimiter.ToString() + m_textDelimiter.ToString(), m_textDelimiter.ToString()));

			if (colValue == "1.000e5")
			{
				int aaa = 0;
				aaa++;
			}

			m_data.Add(new List<string>(lineData));
		}
	}

	public void Load(string filename)
	{
		string[] lines = File.ReadAllLines(filename);
		LoadContent(lines);
	}

	public void LoadResource(string filename)
	{
		string str = Resources.Load<TextAsset>(filename).text;
		string[] lines = System.Text.RegularExpressions.Regex.Split(str , @"\r?\n|\r");
		LoadContent(lines);
	}

	public void Save(string filename)
	{
		List<string> lines = new List<string>();

		for (int i = 0; i < m_data.Count; i++)
		{
			string line = "";
			List<string> lineData = m_data[i];
			if (lineData.Count > 0)
			{
				for (int j = 0; j < lineData.Count; j++)
				{
					string colValue = "";
					if (!string.IsNullOrEmpty(lineData[j]))
					{
						colValue = string.Format("{0}{1}{2}", m_textDelimiter, lineData[j].Replace(m_textDelimiter.ToString(), m_textDelimiter.ToString() + m_textDelimiter.ToString()), m_textDelimiter);
					}
					if (j < lineData.Count - 1)
					{
						colValue += m_fieldDelimiter;
					}
					line += colValue;
				}
				lines.Add(line);
			}
		}

		File.WriteAllLines(filename, lines.ToArray());
	}

	public int NumberRow
	{
		get
		{
			return m_data.Count;
		}
	}

	public void AddRow(params object[] list)
	{
		List<string> lineData = new List<string>();
		for (int i = 0; i < list.Length; i++)
		{
			lineData.Add(list[i].ToString());
		}
		m_data.Add(lineData);
	}

	public string GetValue(int row, int col)
	{
		if (row >= 0 && row < m_data.Count && col >= 0 && col < m_data[row].Count)
		{
			return m_data[row][col];
		}
		return "";
	}

	public string GetValue(int row, string colName)
	{
		if (m_data.Count > 0)
		{
			for (int i = 0; i < m_data[0].Count; i++)
			{
				if (m_data[0][i] == colName)
				{
					return GetValue(row, i);
				}
			}

		}
		return "";
	}

	public int GetValueInt(int row, string colName, int defaultVal = 0)
	{
		int no = defaultVal;
		string strVal = GetValue (row, colName);
		if (string.IsNullOrEmpty (strVal)) {
			return defaultVal;
		}
		if (!int.TryParse (strVal, out no)) {
			return defaultVal;
		}
		return no;
	}

	public float GetValueFloat(int row, string colName, float defaultVal = 0.0f)
	{
		float no = defaultVal;
		string strVal = GetValue (row, colName);
		if (string.IsNullOrEmpty (strVal)) {
			return defaultVal;
		}
		if (!float.TryParse (strVal, out no)) {
			return defaultVal;
		}
		return no;
	}
}