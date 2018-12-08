using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
	public static DateTime FromUnixTimeMS(long unixTime)
	{
		var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return epoch.AddMilliseconds(unixTime);
	}

	public static long ToUnixTimeMS(DateTime date)
	{
		var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return Convert.ToInt64((date - epoch).TotalMilliseconds);
	}

	public static float FindDistanceToSegment(Vector2 start, Vector2 end, Vector2 point)
	{
		Vector2 diff = end - start;
		if ((diff.x == 0) && (diff.y == 0))
		{
			diff = point - start;
			return Vector2.SqrMagnitude(diff);
		}

		float t = ((point.x - start.x) * diff.x + (point.y - start.y) * diff.y) / (diff.x * diff.x + diff.y * diff.y);

		if (t < 0)
		{
			//point is nearest to the first point i.e x1 and y1
			diff = point - start;
		}
		else if (t > 1)
		{
			//point is nearest to the end point i.e x2 and y2
			diff = point - end;
		}
		else
		{
			//if perpendicular line intersect the line segment.
			diff = point - (start + t * diff);
		}

		//returning shortest distance
		return diff.magnitude;
	}

	public static Vector2 GetClosestPoint(Vector2 start, Vector2 end, Vector2 point)
	{
		Vector2 diff = end - start;
		if ((diff.x == 0) && (diff.y == 0))
		{
			diff = point - start;
			return start;
		}

		float t = ((point.x - start.x) * diff.x + (point.y - start.y) * diff.y) / (diff.x * diff.x + diff.y * diff.y);

		if (t < 0)
		{
			//point is nearest to the first point i.e x1 and y1
			diff = point - start;
			return start;
		}
		else if (t > 1)
		{
			//point is nearest to the end point i.e x2 and y2
			diff = point - end;
			return end;
		}
		else
		{
			//if perpendicular line intersect the line segment.
			//diff = point - (start + t * diff);
			return (start + t * diff);
		}
	}
}