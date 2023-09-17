using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour {
	public List<WallReferance> wallList;
	public DungeonGenerator.RoomNode roomNode;

	public void SetWalls(List<Direction> openWalls) {
		if (openWalls == null || openWalls.Count == 0) return;

		int rotation = Random.Range(0, 4);
		transform.rotation = Quaternion.Euler(new Vector3(0f, -90f * rotation, 0f));

		foreach (WallReferance wall in wallList) {
			foreach (Direction direction in openWalls) {
				Direction checkDirection = (Direction)(((int)direction + rotation) % 4);
				for (int i = 0; i < wall.directions.Count; i++) {
					if (wall.directions[i] == checkDirection) {
						wall.go.SetActive(false);
						break;
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
