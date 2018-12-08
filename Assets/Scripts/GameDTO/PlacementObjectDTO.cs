using System;

namespace GameDTO
{
	public enum ObjectType{		
		None = -1,
		Ground = 0,
		Player,
		SquareObstacle,
	};

	public class PlacementObjectDTO
	{
		public ObjectType ObjectType;
		public float SizeX;
		public float SizeY;
		public int MapX;
		public int MapY;

		public PlacementObjectDTO ()
		{
		}

		public PlacementObjectDTO Clone(){
			PlacementObjectDTO clone = new PlacementObjectDTO ();
			clone.ObjectType = this.ObjectType;
			clone.SizeX = this.SizeX;
			clone.SizeY = this.SizeY;
			clone.MapX = this.MapX;
			clone.MapY = this.MapY;
			return clone;
		}
	}
}

