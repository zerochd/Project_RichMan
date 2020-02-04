using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
	private static readonly List<Grid> OpenList = new List<Grid> ();
	private static readonly List<Grid> CloseList = new List<Grid> ();

	private static readonly Stack<Grid> ResultGridStack = new Stack<Grid> ();
	
	public static Stack<Grid> CalcPath (Grid startGrid, Grid endGird, Map map)
	{
		if (startGrid == null || endGird == null || map == null)
			return ResultGridStack;
		
		ResultGridStack.Clear ();
		
		//清空数据
		foreach (var gd in OpenList) {
			gd.ResetValue ();
		}

		foreach (var gd in CloseList) {
			gd.ResetValue ();
		}
		
		OpenList.Clear ();
		CloseList.Clear ();

		//添加起始点
		OpenList.Add (startGrid);
		var currentGrid = startGrid;

		//循环遍历路径上最小F的点
        //循环条件：
        //1.openlist > 0
        //2.当前点并不是终点
		while (OpenList.Count > 0 && currentGrid != endGird) {

            //设置当前查找的grid为openList[0]
            //由于查找上下左右邻居点后，都会sort（）
            //所以openList[0]为f值最小的点
			currentGrid = OpenList [0];

            //当前查找的grid == 最终的目标grid
			if (currentGrid == endGird) {

				//生成结果
				GenerateResult (currentGrid);

//				//清空数据
//				foreach (var gd in OpenList) {
//					gd.ResetValue ();
//				}
//
//				foreach (var gd in CloseList) {
//					gd.ResetValue ();
//				}

				return ResultGridStack;
			}

			//只计算上下左右方向的邻居点
			for (var i = -1; i <= 1; i++) {
				for (var j = -1; j <= 1; j++) {
					
					//过滤斜角和自身
					if (i == j || i == -j)
						continue;		

					//计算每个邻居的坐标
					var x = currentGrid.Vi.x + i;
					var y = currentGrid.Vi.y + j;

                    //没有超出地图范围
                    var mInMapArea = (x >= 0 && y >= 0 && x <= (map.Edge.x - 1) && y <= (map.Edge.y - 1));
                    if (!mInMapArea)
                        continue;

                    //点是合法的
                    var mGridValid = (map.gridMat[x, y] != null
                        && map.gridMat[x, y].gameObject.activeSelf
                        && map.gridMat[x, y].Owner == null);

                    //点未到达的
                    var mNotInCloseList = !CloseList.Contains(map.gridMat[x, y]);

                    if (mGridValid     
                        && mNotInCloseList) {

						//计算G值
						var _g = currentGrid.G + (int)(Mathf.Sqrt ((Mathf.Abs (i) + Mathf.Abs (j))) * 10);

						//与原G值对照
                        //如果小于原G值或者原G值 == 0
                        //覆盖该邻居点的G值，然后将该邻居点的GridParent设置为当前点
						if (map.gridMat [x, y].G == 0 || map.gridMat [x, y].G > _g) {
							map.gridMat [x, y].G = _g;
							map.gridMat [x, y].GridParent = currentGrid;
						}

						//计算该邻居点的H值
						map.gridMat [x, y].H = Manhattan (x, y, endGird.Vi.x, endGird.Vi.y);

						//计算该邻居点的F值
						map.gridMat [x, y].F = map.gridMat [x, y].G + map.gridMat [x, y].H;

                        //如果openList没有该邻居点的话
                        //将该邻居点加入到openList中去
						if (!OpenList.Contains (map.gridMat [x, y])) {
							OpenList.Add (map.gridMat [x, y]);
						}

						//重新排序
                        //可以优化
						OpenList.Sort ();
					}
				}
			}

			//完成遍历添加当前点进入关闭列表
			CloseList.Add (currentGrid);
			//并从openList中移除该grid点
			OpenList.Remove (currentGrid);

			//如果开启列表为空，未能找到路径
			if (OpenList.Count == 0) {
				Debug.LogError ("Can not find");
				
//				foreach (Grid gd in CloseList) {
//					gd.ResetValue ();
//				}
			}
		}
		return ResultGridStack;
	}
	
	
	/// <summary>
	/// 生成结果
	/// </summary>
	/// <param name="currentGrid">Current grid.</param>
	static void GenerateResult (Grid currentGrid)
	{

		if (currentGrid.GridParent != null) {
			
			ResultGridStack.Push (currentGrid);
			GenerateResult (currentGrid.GridParent);
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
