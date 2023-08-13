using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour {
	public List<WallReferance> wallList;

	public void SetWalls(List<Direction> openWalls) {
		if (openWalls == null || openWalls.Count == 0) return;

		foreach (WallReferance wall in wallList) {
			foreach (Direction direction in openWalls) {
				for (int i = 0; i < wall.directions.Count; i++) {
					if (wall.directions[i] == direction) {
						DestroyImmediate(wall.go);
					}
				}
			}
		}
	}
}

[System.Serializable]
public class WallReferance {
	public GameObject go;
	public List<Direction> directions;
}
