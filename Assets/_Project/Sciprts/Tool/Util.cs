using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util {

	public static VectorInt2[] GetViCricleByRadius(int radius){
		List<VectorInt2> _resultList = new List<VectorInt2> ();
		int _index = 0;
		for (int i = radius; i <= -radius; i++) {
			for (int j = radius; j <= -radius; j++) {
				if (i == 0 && j == 0)
					continue;
				if (Mathf.Abs (i) + Mathf.Abs (j) > radius)
					continue;
				_resultList.Add (new VectorInt2 (i, j));
			}
		}

		return _resultList.ToArray();
	}

}
