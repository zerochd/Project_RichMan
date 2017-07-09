using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{

	public static List<Grid> openList = new List<Grid> ();
	public static List<Grid> closeList = new List<Grid> ();

	static Stack<Grid> resultGridStack = new Stack<Grid> ();


	public static Stack<Grid> CalcPath (Grid startGrid, Grid endGird, Map map)
	{
		if (startGrid == null || endGird == null || map == null)
			return resultGridStack;
		
		resultGridStack.Clear ();
		openList.Clear ();
		closeList.Clear ();

		//添加起始点
		openList.Add (startGrid);
		Grid _currentGrid = startGrid;

		//循环遍历路径上最小F的点
		while (openList.Count > 0 && _currentGrid != endGird) {

			_currentGrid = openList [0];

			if (_currentGrid == endGird) {
//				Debug.Log ("find endGrid");

				//生成结果
				GenerateResult (_currentGrid);

				//清空数据
				foreach (Grid gd in openList) {
					gd.ResetValue ();
				}

				foreach (Grid gd in closeList) {
					gd.ResetValue ();
				}

				return resultGridStack;
			}

			//只计算上下左右
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					
					//过滤斜角和自身
					if (i == j || i == -j)
						continue;
					
					//过滤自身
//					if (i == 0 && j == 0)
//						continue;

					//计算坐标
					int _x = _currentGrid.Vi.x + i;
					int _y = _currentGrid.Vi.y + j;

					//没有超出地图范围，不是空的，不是重复点
					if (_x >= 0 && _y >= 0
					    && _x <= map.row && _y <= map.column
					    && map.gridMat [_x, _y] != null
					    && map.gridMat [_x, _y].gameObject.activeSelf
					    && map.gridMat [_x, _y].owner == null
					    && !closeList.Contains (map.gridMat [_x, _y])) {

						//计算G值
						int _g = _currentGrid.g + (int)(Mathf.Sqrt ((Mathf.Abs (i) + Mathf.Abs (j))) * 10);

						//与原G值对照
						if (map.gridMat [_x, _y].g == 0 || map.gridMat [_x, _y].g > _g) {
							map.gridMat [_x, _y].g = _g;
							map.gridMat [_x, _y].gridParent = _currentGrid;
						}

						//计算H值
						map.gridMat [_x, _y].h = Manhattan (_x, _y, endGird.Vi.x, endGird.Vi.y);

						//计算F值
						map.gridMat [_x, _y].f = map.gridMat [_x, _y].g + map.gridMat [_x, _y].h;

						if (!openList.Contains (map.gridMat [_x, _y])) {
							openList.Add (map.gridMat [_x, _y]);
						}

						//重新排序
						openList.Sort ();
					}
				}
			}

			//完成遍历添加该点进入关闭列表
			closeList.Add (_currentGrid);
			//从开启列表删除
			openList.Remove (_currentGrid);

			//如果开启列表为空，未能找到路径
			if (openList.Count == 0) {
				Debug.Log ("Can not find");
			}
		}
		return resultGridStack;
	}

	/// <summary>
	/// 生成结果
	/// </summary>
	/// <param name="currentGrid">Current grid.</param>
	static void GenerateResult (Grid currentGrid)
	{

		if (currentGrid.gridParent != null) {

//			Debug.Log ("gridName:" + currentGrid.name);

			resultGridStack.Push (currentGrid);
			GenerateResult (currentGrid.gridParent);
		}

	}

	/// <summary>
	/// 曼哈顿计算H值
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="targetX">Target x.</param>
	/// <param name="targetY">Target y.</param>
	static int Manhattan (int x, int y, int targetX, int targetY)
	{
		return (int)(Mathf.Abs (targetX - x) + Mathf.Abs (targetY - y)) * 10;
	}

}
