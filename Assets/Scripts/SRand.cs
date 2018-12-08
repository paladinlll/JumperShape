using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRand {
	long s_rndx;
	long s_rndy;
	long s_rndz;
	long s_rndw;

	public long Seek
	{
		get {
			return s_rndx;
		}
	}

	public SRand()
	{
		s_rndx = Utils.ToUnixTimeMS(System.DateTime.Now);
		s_rndy = 842502087L;
		s_rndz = 3579807591L;   //unsigned only in ISO C90
		s_rndw = 273326509L;
	}

	public SRand(int seed)
	{
		s_rndx = seed;
		s_rndy = 842502087L;
		s_rndz = 3579807591L;   //unsigned only in ISO C90
		s_rndw = 273326509L;
	}


	public int Rand()
	{
		long t =(s_rndx^(s_rndx<<11));
		s_rndx = s_rndy; s_rndy = s_rndz; s_rndz = s_rndw;
		s_rndw =(s_rndw^(s_rndw>>19))^(t^(t>>8));

		long rtn = s_rndw&0x7FFFFFFF;
		return (rtn == 0x7FFFFFFF ? Rand() : (int)rtn);
	}

	public int Rand(int lowerBound, int upperBound)
	{
		if (lowerBound > upperBound)
		{
			int t      = lowerBound;
			lowerBound = upperBound;
			upperBound = t;
		}

		long rnd = Rand() >> 2;
		int  range    = upperBound - lowerBound + 1;
		return (range != 0 ? ((int)(lowerBound + (rnd % range))) : lowerBound);
	}

	public int Rand2()
	{
		long t =(s_rndx^(s_rndx<<11));
		s_rndx = s_rndy; s_rndy = s_rndz; s_rndz = s_rndw;
		s_rndw =(s_rndw^(s_rndw>>19))^(t^(t>>8));

		long rtn = s_rndw&0x7FFFFFFF;
		return (int)rtn;
	}

	public int Rand2(int lowerBound, int upperBound) // max range - 32767
	{
		uint rnd = (uint)((Rand2() >> 2) & 0xFFFF);
		int range = upperBound - lowerBound;
		return ((int)(lowerBound + ((rnd * range) >> 16)));
	}

	public float Randf()
	{
		int rand = Rand();
		rand = (rand & 0xFFFF) - 0x7FFF;
		return ((float)rand) * 0.000030517578125f;
	}

}
