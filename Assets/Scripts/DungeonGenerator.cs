using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum Direction { UP, RIGHT, DOWN, LEFT };

public class DungeonGenerator : MonoBehaviour {

	private List<Vector2Int> direction = new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

	public NavMeshSurface surface;

	public int iterations = 3;
	public List<RuleSet> ruleSets = new List<RuleSet>();

	public float gridScale = 30f;
	public List<DungeonRoom> startRooms;
	public List<DungeonRoom> endRooms;
	public List<DungeonRoom> combatRooms;
	public List<DungeonRoom> treasureRooms;

	public enum RoomType {TBD, Start, End, Combat, Treasure};

	[System.Serializable]
	public struct RuleNode {
		public int id;
		public List<int> connections;
	}
	[System.Serializable]
	public struct RuleGraph {
		public List<RuleNode> nodes;
	}
	[System.Serializable]
	public struct RuleSet {
		public List<RuleGraph> graphs;
	}
	public class RoomNode {
		public RoomType roomType;
		public List<Direction> openDoors;

		public void SetRoomType(RoomType type) {
			roomType = type;
		}
		public RoomType GetRoomType() {
			return roomType;
		}

		public void AddDoor(Direction direction) {
			if (openDoors == null) openDoors = new List<Direction>();

			if (!openDoors.Contains(direction)) {
				openDoors.Add(direction);
			}
		}

		public void removeDoor(Direction direction) {
			if (openDoors == null) openDoors = new List<Direction>();

			if (openDoors.Contains(direction)) {
				openDoors.Remove(direction);
			}
		}
	}

	void Start() {
		Generate();
	}

	public void Generate() {
		//Debug.Log("Generate");
		Dictionary<Vector2Int, RoomNode> roomMap = new Dictionary<Vector2Int, RoomNode>();
		List<Vector2Int> ends = new List<Vector2Int>();

		// Initialize with first and last room;
		{
			Vector2Int randomStartDirection = direction[Random.Range(0, direction.Count)];
			roomMap.Add(Vector2Int.zero, new RoomNode());
			roomMap.Add(randomStartDirection, new RoomNode());

			roomMap[Vector2Int.zero].SetRoomType(RoomType.Start);
			roomMap[randomStartDirection].SetRoomType(RoomType.End);

			Direction doorDirection = FindDirectionToRoom(Vector2Int.zero, randomStartDirection);
			roomMap[Vector2Int.zero].AddDoor(doorDirection);
			roomMap[randomStartDirection].AddDoor(GetOppositeDirection(doorDirection));
			PrintRoomMap(roomMap);
		}
		
		// Apply rules to build the map
		for (int i = 0; i < iterations; i++) {
			//Debug.Log("Iteraton " + i);

			// Cycle over RuleSets
			for (int j = 0; j < ruleSets.Count; j++) {
				if (ruleSets[j].graphs.Count <= 0) continue;

				RuleSet ruleSet = ruleSets[j];

				//Select initial ruleGraph
				int ruleIndex = Random.Range(0, ruleSet.graphs.Count);
				RuleGraph ruleGraph = ruleSet.graphs[ruleIndex];

				ends = GetDeadEnds(roomMap);

				//Cycle over RuleGraphs in ruleSet until you find a valid rule
				for (int k = 0; k < ruleSet.graphs.Count; k++) {
					// Check if valid (dfs compairing roomMap to ruleGraph)
					if (IsValidRule(roomMap, ruleGraph)) {
						// If valid, apply ruleGraph to roomMap
						roomMap = ApplyRuleToMap(roomMap, ruleGraph);

						break;
					}

					// Move to next RuleGraph
					ruleIndex = (ruleIndex + 1) % ruleSet.graphs.Count;
					ruleGraph = ruleSet.graphs[ruleIndex];
				}

			}
		}

		// Lower roomMap into world
		roomMap = AssignRoomTypes(roomMap);
		LowerRoomMap(roomMap);
		PrintRoomMap(roomMap);

		StartCoroutine(BuildNavMesh());
	}

	public void Clear() {
		for (int i = transform.childCount - 1; i >= 0; i--) {
			DestroyImmediate(transform.GetChild(i).gameObject);
		}
	}

	private List<Vector2Int> GetDeadEnds(Dictionary<Vector2Int, RoomNode> roomMap) {
		//Debug.Log("GetDeadEnds");
		List<Vector2Int> ends = new List<Vector2Int>();

		foreach (KeyValuePair<Vector2Int, RoomNode> room in roomMap) {
			if (room.Value.GetRoomType() == RoomType.Start) continue;

			if (room.Value.openDoors != null && room.Value.openDoors.Count == 1) {
				ends.Add(room.Key);
			}
		}

		return ends;
	}

	private Direction FindDirectionToRoom(Vector2Int from, Vector2Int to) {
		if (from.y < to.y) return Direction.UP;
		if (from.x < to.x) return Direction.RIGHT;
		if (from.y > to.y) return Direction.DOWN;
		if (from.x > to.x) return Direction.LEFT;
		return Direction.UP;
	}

	private Direction GetOppositeDirection(Direction direction) {
		return (Direction)(((int)direction + 2) % 4);
	}

	private bool IsValidRule(Dictionary<Vector2Int, RoomNode> roomMap, RuleGraph ruleGraph) {
		return false;
	}

	private Dictionary<Vector2Int, RoomNode> ApplyRuleToMap(Dictionary<Vector2Int, RoomNode> roomMap, RuleGraph ruleGraph) {
		return roomMap;
	}

	private Dictionary<Vector2Int, RoomNode> AssignRoomTypes(Dictionary<Vector2Int, RoomNode> roomMap) {
		//Debug.Log("AssignRoomTypes");
		List<Vector2Int> ends = new List<Vector2Int>();
		ends = GetDeadEnds(roomMap);

		foreach (KeyValuePair<Vector2Int, RoomNode> room in roomMap) {
			if (room.Value.GetRoomType() == RoomType.TBD) {
				if (ends.Contains(room.Key)) {
					roomMap[room.Key].SetRoomType(RoomType.Treasure);
				} else {
					roomMap[room.Key].SetRoomType(RoomType.Combat);
				}
			}
		}
		
		return roomMap;
	}

	private void LowerRoomMap(Dictionary<Vector2Int, RoomNode> roomMap) {
		//Debug.Log("LowerRoomMap");
		Clear();

		DungeonRoom roomToSpawn = null;

		foreach (KeyValuePair<Vector2Int, RoomNode> room in roomMap) {
			switch (room.Value.GetRoomType()) {
				case RoomType.Start:
					roomToSpawn = startRooms[Random.Range(0, startRooms.Count)];
					break;
				case RoomType.End:
					roomToSpawn = endRooms[Random.Range(0, endRooms.Count)];
					break;
				case RoomType.Combat:
					roomToSpawn = combatRooms[Random.Range(0, combatRooms.Count)];
					break;
				case RoomType.Treasure:
					roomToSpawn = treasureRooms[Random.Range(0, treasureRooms.Count)];
					break;
			}

			//Debug.Log(room.Value.GetRoomType());

			if (!roomToSpawn) continue;
			GameObject newRoom = Instantiate(roomToSpawn.gameObject);
			newRoom.gameObject.SetActive(true);
			newRoom.transform.position = new Vector3(room.Key.x, 0f, room.Key.y) * gridScale;
			newRoom.transform.parent = transform;
			newRoom.name = "<" + room.Key.x + "," + room.Key.y + "> " + room.Value.GetRoomType();
			newRoom.GetComponent<DungeonRoom>().SetWalls(room.Value.openDoors);
		}
	}

	private void PrintRoomMap(Dictionary<Vector2Int, RoomNode> roomMap) {
		foreach (KeyValuePair<Vector2Int, RoomNode> room in roomMap) {
			//Debug.Log("<" + room.Key.x + "," + room.Key.y + "> " + room.Value.GetRoomType());
		}
	}

	private Dictionary<Vector2Int, RoomNode> GetCopyOfRoomMap(Dictionary<Vector2Int, RoomNode> roomMap) {
		Dictionary<Vector2Int, RoomNode> newRoomMap = new Dictionary<Vector2Int, RoomNode>();

		foreach (KeyValuePair<Vector2Int, RoomNode> room in roomMap) {
			newRoomMap.Add(room.Key, room.Value);
		}

		return newRoomMap;
	}

	IEnumerator BuildNavMesh() {
		yield return null;

		surface.BuildNavMesh();
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		if (GUILayout.Button("Generate")) {
			(target as DungeonGenerator).Generate();
		}

		if (GUILayout.Button("Clear")) {
			(target as DungeonGenerator).Clear();
		}
	}
}
#endif