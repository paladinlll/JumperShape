using System;
using System.Collections.Generic;

namespace GameDTO
{
	public class LevelDTO
	{
		public string HeightMap;
		public List<PlacementObjectDTO> PlacementObjects;
		List<int> m_tmpHeightMaps = new List<int>();

		public LevelDTO ()
		{
			Clear ();
		}

		public void Clear()
		{
			HeightMap = "";
			m_tmpHeightMaps.Clear ();
			PlacementObjects = new List<PlacementObjectDTO> ();
		}

		public void SetHeightMap(List<int> heightMaps)
		{
			HeightMap = "";

			m_tmpHeightMaps.Clear ();
			for (int i = 0; i < heightMaps.Count; i++) {
				m_tmpHeightMaps.Add (heightMaps [i]);
			}

			for (int i = 0; i < heightMaps.Count; i++) {
				if (heightMaps [i] == 0) {
					HeightMap += '0';
				} else if (heightMaps [i] == 1) {
					HeightMap += '1';
				} else {
					HeightMap += '.';
				}
			}
		}

		public List<int> GetHeightMap()
		{
			if (m_tmpHeightMaps == null || m_tmpHeightMaps.Count != HeightMap.Length) {
				m_tmpHeightMaps.Clear ();
				for (int i = 0; i < HeightMap.Length; i++) {
					if (HeightMap [i] == '0') {
						m_tmpHeightMaps.Add (0);
					} else if (HeightMap [i] == '1') {
						m_tmpHeightMaps.Add (1);
					} else if (HeightMap [i] == '3') {
						m_tmpHeightMaps.Add (3);
					} else {
						m_tmpHeightMaps.Add (-1);
					}
				}
			}

			return m_tmpHeightMaps;
		}
	}
}

