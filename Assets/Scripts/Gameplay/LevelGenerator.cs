using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
	enum JumpType{
		SHORT_JUMP,
		MEDIUM_JUMP,
		LONG_JUMP

	}

	/// <summary>
	/// Base on number of each jump type. We will permutate it.
	/// </summary>
	/// <returns>The permutation jumps.</returns>
	/// <param name="shortJump">Short jump.</param>
	/// <param name="mediumJump">Medium jump.</param>
	/// <param name="longJump">Long jump.</param>
	private static List<JumpType> RandomJumpList(int shortJump, int mediumJump,int longJump){		
		List<JumpType> jumpLine = new List<JumpType> ();
		for (int i = 0; i < shortJump; i++) {
			jumpLine.Add (JumpType.SHORT_JUMP);
		}
		for (int i = 0; i < mediumJump; i++) {
			jumpLine.Add (JumpType.MEDIUM_JUMP);
		}
		for (int i = 0; i < longJump; i++) {
			jumpLine.Add (JumpType.LONG_JUMP);
		}

		for (int i = 0; i < jumpLine.Count; i++) {
			int minRange = Mathf.Max (0, i - jumpLine.Count / 3);
			int maxRange = Mathf.Min (jumpLine.Count, i + jumpLine.Count / 3);
			int ran = UnityEngine.Random.Range (minRange, maxRange);
			JumpType tmp = jumpLine [i];
			jumpLine [i] = jumpLine [ran];
			jumpLine [ran] = tmp;
		} 
		return jumpLine;
	}

	public static List<int> RandomZoneMap(int shortJump, int mediumJump,int longJump, int maxJumpDist){
		int minJumpDist = (maxJumpDist + 1) / 2;
		int totalJump = shortJump + mediumJump + longJump;

		//Random jump list with number of each jump type was been given.
		List<JumpType> jumpLine = RandomJumpList (shortJump, mediumJump, longJump);

		//Then slice it to some small group. For player have a break time. 
		List<List<JumpType>> jumpGroupLine = new List<List<JumpType>> ();
		int minJumpGroup = totalJump / 2;
		int jumpGroupCount = minJumpGroup +  (int)Mathf.Sqrt(UnityEngine.Random.Range (0, minJumpGroup * minJumpGroup));

		//Give [totalJump] jump to [jumpGroupCoun]t group ([totalJump] > [jumpGroupCount])
		//First give [jumpGroupCount] first jump to [jumpGroupCount] group
		for (int i = 0; i < jumpGroupCount; i++) {			
			jumpGroupLine.Add (new List<JumpType> ());
			jumpGroupLine [i].Add (jumpLine [i]);
		}

		//The rest will be delivered random
		for (int i = jumpGroupCount; i < jumpLine.Count; i++) {
			int ranGroup = UnityEngine.Random.Range (0, jumpGroupCount);
			jumpGroupLine [ranGroup].Add (jumpLine [i]);
		}

		string singleLine = "";
		List<int> heightMap = new List<int> ();

		for (int i = 0; i < jumpGroupCount; i++) {
			int pad = UnityEngine.Random.Range (maxJumpDist, maxJumpDist * 3);
		
			for (int p = 0; p < pad; p++) {
				singleLine += '_';
				heightMap.Add (0);
			}
			for (int j = 0; j < jumpGroupLine [i].Count; j++) {
				JumpType jumpType = jumpGroupLine [i] [j];

				int space = 0;
				if (jumpType == JumpType.SHORT_JUMP) {
					space += minJumpDist;
				} else if (jumpType == JumpType.MEDIUM_JUMP) {
					space += (maxJumpDist + minJumpDist) / 2;
				} else if (jumpType == JumpType.LONG_JUMP) {
					space += maxJumpDist;
				}

				for (int p = 0; p < space; p++) {
					singleLine += '.';
					heightMap.Add (-1);
				}

				//int shortPad = UnityEngine.Random.Range(minJumpDist, minJumpDist * 2);
				int shortPad = minJumpDist;
				if (j == jumpGroupLine [i].Count - 1) {
					shortPad = 0;
				}
				for (int p = 0; p < shortPad; p++) {
					singleLine += '_';
					heightMap.Add (0);
				}
			}
		}
		singleLine += "__";
		heightMap.Add (0);
		heightMap.Add (0);

		Debug.Log (singleLine);
		return heightMap;
	}
}