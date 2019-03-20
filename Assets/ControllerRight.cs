using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControllerRight : MonoBehaviour
{

	// Use this for initialization
	public GameObject Base;
	public GameObject BallPrefab;
	public GameObject WallPrefab;
	public GameObject FloorPrefab;
	public GameObject GoalPrefab;

	const String WALL = "■";
	const String SPACE = "　";
	const String NOW = "Ｘ";
	const String ROUTE = "○";

	static String[,] maze;
	static List<int[]> start;
	static int m, n;
	static System.Random r;
	static Stack<int[]> stack;

	// Use this for initialization
	void Start()
	{
		r = new System.Random(1000);
		m = 9;
		n = 9;
		maze = new String[m, n];
		start = new List<int[]>();
		stack = new Stack<int[]>();
		//迷路ベース生成
		for (int i = 0; i < maze.GetLength(0); i++)
		{
			for (int j = 0; j < maze.GetLength(1); j++)
			{
				if (i == 0 || j == 0 || i == m - 1 || j == n - 1)
				{
					maze[i, j] = WALL;
				}
				else
				{
					if (i % 2 == 0 && j % 2 == 0)
					{
						maze[i, j] = SPACE;
						start.Add(new[] { i, j });
					}
					else
					{
						maze[i, j] = SPACE;
					}
				}
			}
		}
		//開始位置
		int[] point = Enumerable.Range(0, start.Count).OrderBy(i => Guid.NewGuid()).ToArray();
		//伸ばす
		foreach (var item in point)
		{
			int i = start[item][0];
			int j = start[item][1];

			if (maze[i, j] == WALL) continue;

			maze[i, j] = NOW;
			stack.Push(new[] { i, j });
			bool breakFlag = false;
			while (!breakFlag)
			{
				int[,] a = { { -2, 0 }, { 0, 2 }, { 2, 0 }, { 0, -2 } };
				int[] s = Enumerable.Range(0, 4).OrderBy(z => Guid.NewGuid()).ToArray();

				for (int k = 0; k < 4; k++)
				{
					//作ってる壁に当たる→方向を変える
					//全方向だめなら→一つ前に戻る
					if (maze[i + a[s[k], 0], j + a[s[k], 1]] == NOW)
					{
						if (k == 3)
						{
							int[] pre = stack.Pop();
							i = pre[0];
							j = pre[1];
							break;
						}
						else continue;
					}
					//壁に当たる→つなぐ
					else if (maze[i + a[s[k], 0], j + a[s[k], 1]] == WALL)
					{
						maze[i + (a[s[k], 0] / 2), j + (a[s[k], 1] / 2)] = NOW;
						breakFlag = true;
						for (int x = 0; x < maze.GetLength(0); x++)
							for (int y = 0; y < maze.GetLength(1); y++)
								if (maze[x, y] == NOW) maze[x, y] = WALL;

						break;
					}
					maze[i + (a[s[k], 0] / 2), j + (a[s[k], 1] / 2)] = NOW;
					i += a[s[k], 0];
					j += a[s[k], 1];
					maze[i, j] = NOW;
					stack.Push(new[] { i, j });
					break;
				}
			}
		}

		Vector3 p = new Vector3(0, 0, 0);
		Quaternion q = new Quaternion();
		q = Quaternion.identity;

		for (int i = 0; i < maze.GetLength(0); i++)
		{
			for (int j = 0; j < maze.GetLength(1); j++)
			{
				GameObject g;
				float y;
				if (maze[i, j] == WALL)
				{
					g = WallPrefab;
					y = WallPrefab.transform.localScale.y / 2;
				}
				else if (i == m - 2 && j == n - 2)
				{
					g = GoalPrefab;
					y = 0;
				}
				else
				{
					g = FloorPrefab;
					y = 0;
				}
				p.x = i * 0.1f;
				p.z = j * 0.1f;
				p.y = y;
				Instantiate(g, p, q, Base.transform);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		var trackedObject = GetComponent<SteamVR_TrackedObject>();
		var device = SteamVR_Controller.Input((int)trackedObject.index);
		var p = Base.transform.position;
		var r = Base.transform.rotation;
		p.x += 0.1f;
		p.z += 0.1f;
		p.y += 0.4f;
		if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
		{
			Instantiate(BallPrefab, p, r);
		}

	}
}
