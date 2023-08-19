using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum Direction { UP, RIGHT, DOWN, LEFT };

public class DungeonGenerator : MonoBehaviour {

	private List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
	private Dictionary<Vector2Int, RoomNode> roomMap = new Dictionary<Vector2Int, RoomNode>();

	public NavMeshSurface surface;

	public int growthIterations = 3;
	public float oddsOfBranch = 1f/3f;

	public float gridScale = 30f;
	public List<DungeonRoom> startRooms;
	public List<DungeonRoom> endRooms;
	public List<DungeonRoom> combatRooms;
	public List<DungeonRoom> treasureRooms;

	public enum RoomType {TBD, Start, End, Combat, Treasure};

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
		Clear();
		// Initialize with first and last room;
		{
			Vector2Int randomStartDirection = directions[Random.Range(0, directions.Count)];
			roomMap.Add(Vector2Int.zero, new RoomNode());
			roomMap.Add(randomStartDirection, new RoomNode());

			roomMap[Vector2Int.zero].SetRoomType(RoomType.Start);
			roomMap[randomStartDirection].SetRoomType(RoomType.End);

			Direction doorDirection = FindDirectionToRoom(Vector2Int.zero, randomStartDirection);
			roomMap[Vector2Int.zero].AddDoor(doorDirection);
			roomMap[randomStartDirection].AddDoor(GetOppositeDirection(doorDirection));
			PrintRoomMap();
		}
		
		// Apply rules to build the maps
		for (int i = 0; i < growthIterations; i++) {
			//Debug.Log("Groth Iteraton " + i);
			Grow();
		}

		// Lower roomMap into world
		AssignRoomTypes();
		LowerRoomMap();
		PrintRoomMap();

		StartCoroutine(BuildNavMesh());
	}

	public void Clear() {
		roomMap.Clear();

		for (int i = transform.childCount - 1; i >= 0; i--) {
			DestroyImmediate(transform.GetChild(i).gameObject);
		}
	}

	private void Grow() {
		//Debug.Log("Grow");
		List<Vector2Int> ends = GetDeadEnds(); ;
		if (ends.Count == 0) return;

		int currentRoomIndex = Random.Range(0, ends.Count);
		for (int i = 0; i < ends.Count; i++) {
			Vector2Int currentRoom = ends[currentRoomIndex];

			for (int j = oddsOfBranch < Random.Range(0f, 1f) ? 1 : 2; j > 0; j--) {
				List<Vector2Int> openNeighbors = FindOpenNeighbors(currentRoom);
				if (openNeighbors.Count == 0) continue;

				//expand
				RoomNode endRoom = roomMap[currentRoom];
				roomMap[currentRoom] = new RoomNode();
				roomMap[currentRoom].openDoors = new List<Direction>(endRoom.openDoors);
				endRoom.openDoors.Clear();

				Vector2Int newRoom = openNeighbors[Random.Range(0, openNeighbors.Count)];

				roomMap.Add(newRoom, endRoom);
				Direction doorDirection = FindDirectionToRoom(currentRoom, newRoom);
				roomMap[currentRoom].AddDoor(doorDirection);
				roomMap[newRoom].AddDoor(GetOppositeDirection(doorDirection));
			}

			break;
		}
		
	}

	private List<Vector2Int> GetDeadEnds() {
		List<Vector2Int> ends = new List<Vector2Int>();

		foreach (KeyValuePair<Vector2Int, RoomNode> currentRoom in roomMap) {
			if (currentRoom.Value.GetRoomType() == RoomType.Start) continue;

			if (currentRoom.Value.openDoors != null && currentRoom.Value.openDoors.Count == 1) {
				ends.Add(currentRoom.Key);
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

	private List<Vector2Int> FindOpenNeighbors(Vector2Int startingRoom) {
		List<Vector2Int> foundNeighbors = new List<Vector2Int>();

		foreach (Vector2Int direction in directions) {
			Vector2Int checkRoom = direction + startingRoom;
			if (!roomMap.ContainsKey(checkRoom)) {
				foundNeighbors.Add(checkRoom);
			}
		}

		return foundNeighbors;
	}

	private void AssignRoomTypes() {
		List<Vector2Int> ends = GetDeadEnds();

		foreach (KeyValuePair<Vector2Int, RoomNode> room in roomMap) {
			if (room.Value.GetRoomType() == RoomType.TBD) {
				if (ends.Contains(room.Key)) {
					roomMap[room.Key].SetRoomType(RoomType.Treasure);
				} else {
					roomMap[room.Key].SetRoomType(RoomType.Combat);
				}
			}
		}
	}

	private void LowerRoomMap() {
		//Debug.Log("LowerRoomMap");

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

	private void PrintRoomMap() {
		foreach (KeyValuePair<Vector2Int, RoomNode> room in roomMap) {
			//Debug.Log("<" + room.Key.x + "," + room.Key.y + "> " + room.Value.GetRoomType());
		}
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