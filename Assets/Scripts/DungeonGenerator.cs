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
	private List<DoorAndKeys> lockedDoors = new List<DoorAndKeys>();

	public NavMeshSurface surface;
	public GameObject lockedDoor;

	public int keyIterations = 2;
	public int preKeyIterations = 1;
	public int postKeyIterations = 2;
	public int preGrowthIterations = 2;
	public int postGrowthIterations = 3;
	public float oddsOfBranch = 1f/3f;
	public int fillIterations = 2;

	public bool lockAndKey = false;

	public float gridScale = 30f;
	public List<DungeonRoom> startRooms;
	public List<DungeonRoom> endRooms;
	public List<DungeonRoom> combatRooms;
	public List<DungeonRoom> treasureRooms;
	public List<DungeonRoom> gapRooms;
	public List<DungeonRoom> keyRooms;

	public enum RoomType {TBD, Start, End, Combat, Treasure, Gap, Key};

	public class RoomNode {
		public RoomType roomType;
		public List<Direction> openDoors = new List<Direction>();

		public void SetRoomType(RoomType type) {
			roomType = type;
		}
		public RoomType GetRoomType() {
			return roomType;
		}

		public void AddDoor(Direction direction) {
			if (!openDoors.Contains(direction)) {
				openDoors.Add(direction);
			}
		}

		public void ClearDoors() {
			openDoors.Clear();

		}
	}

	private class DoorAndKeys {
		public Direction doorFacing;
		public Vector2Int doorRoom;
		public List<RoomNode> keyRooms = new List<RoomNode>();
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
		for (int i = 0; i < keyIterations; i++) {

			for (int j = 0; j < preGrowthIterations; j++) {
				Grow();
			}

			List<Vector2Int> specialEnds = GetSpecialEnds();
			if (specialEnds.Count == 0) break;

			Vector2Int specialRoom = specialEnds[Random.Range(0, specialEnds.Count)];
			Vector2Int baseRoom = specialRoom;
			Direction lockedDoorDirection = Direction.UP;
			if (roomMap[baseRoom].openDoors.Count > 0) {
				lockedDoorDirection = GetOppositeDirection(roomMap[baseRoom].openDoors[0]);
			}

			for (int j = 0; j < preKeyIterations + 1; j++) {
				Vector2Int newRoom = AddRoom(specialRoom);
				if (newRoom == Vector2Int.zero) break;

				lockedDoorDirection = FindDirectionToRoom(newRoom, specialRoom);
				specialRoom = newRoom;
			}

			if (lockAndKey && FindOpenNeighbors(baseRoom).Count >= 1) {
				specialEnds = AddLockAndKey(baseRoom);

				DoorAndKeys newLockedDoor = new DoorAndKeys();
				newLockedDoor.doorFacing = lockedDoorDirection;
				newLockedDoor.doorRoom = specialRoom;
				foreach (Vector2Int keyRoom in specialEnds) {
					newLockedDoor.keyRooms.Add(roomMap[keyRoom]);
				}

				if (newLockedDoor.keyRooms.Count > 0) {
					lockedDoors.Add(newLockedDoor);
				}
			}

			for (int j = 0; j < postKeyIterations; j++) {
				for (int k = specialEnds.Count-1; k >= 0; k--) {
					specialEnds[k] = AddRoom(specialEnds[k]);
					if (specialEnds[k] == Vector2Int.zero) {
						specialEnds.RemoveAt(k);
					}
				}
			}


			for (int j = 0; j < postGrowthIterations; j++) {
				Grow();
			}

		}

		//Make furthest end room into end, if not using keys
		if (!lockAndKey) {
			// Clear ond end
			List<Vector2Int> specialEnds = GetSpecialEnds();
			if (specialEnds.Count > 0) {
				foreach(Vector2Int end in specialEnds) { 
					if (roomMap[end].GetRoomType() == RoomType.End) {
						roomMap[end].SetRoomType(RoomType.TBD);
						break;
					}
				}
			}

			// Find the furthest room
			Vector2Int furthestRoom = new Vector2Int();
			float distance = 0f;
			foreach (Vector2Int currentRoom in GetDeadEnds()) {
				float checkDistance = Vector2Int.Distance(Vector2Int.zero, currentRoom);
				if (checkDistance > distance) {
					furthestRoom = currentRoom;
					distance = checkDistance;
				}
			}

			// Assign new end
			roomMap[furthestRoom].SetRoomType(RoomType.End);
		}

		// Lower roomMap into world
		AssignRoomTypes();
		for (int i = 0; i < fillIterations; i++) {
			FillGaps();
		}
		LowerRoomMap();
		PrintRoomMap();

		StartCoroutine(BuildNavMesh());
	}

	public void Clear() {
		roomMap.Clear();
		lockedDoors.Clear();

		for (int i = transform.childCount - 1; i >= 0; i--) {
			DestroyImmediate(transform.GetChild(i).gameObject);
		}
	}

	private void Grow() {
		//Debug.Log("Grow");
		List<Vector2Int> ends = GetDeadEnds();
		if (ends.Count == 0) return;

		Vector2Int currentRoom = ends[Random.Range(0, ends.Count)];
		for (int i = oddsOfBranch < Random.Range(0f, 1f) ? 1 : 2; i > 0; i--) {
			AddRoom(currentRoom);
		}

	}

	private List<Vector2Int> AddLockAndKey(Vector2Int baseRoom) {
		List<Vector2Int> ends = new List<Vector2Int>();
		for (int i = oddsOfBranch < Random.Range(0f, 1f) ? 1 : 2; i > 0; i--) {
			Vector2Int newRoom = AddRoom(baseRoom);
			if (newRoom == Vector2Int.zero) continue;

			roomMap[newRoom].SetRoomType(RoomType.Key);
			ends.Add(newRoom);
		}

		return ends;
	}

	private Vector2Int AddRoom(Vector2Int startingRoom) {
		List<Vector2Int> openNeighbors = FindOpenNeighbors(startingRoom);
		if (openNeighbors.Count == 0) return Vector2Int.zero;

		RoomNode endRoom = roomMap[startingRoom];
		roomMap[startingRoom] = new RoomNode();
		roomMap[startingRoom].openDoors = new List<Direction>(endRoom.openDoors);
		endRoom.ClearDoors();

		Vector2Int newRoom = openNeighbors[Random.Range(0, openNeighbors.Count)];
		roomMap.Add(newRoom, endRoom);
		Direction doorDirection = FindDirectionToRoom(startingRoom, newRoom);
		roomMap[startingRoom].AddDoor(doorDirection);
		roomMap[newRoom].AddDoor(GetOppositeDirection(doorDirection));

		return newRoom;
	}

	private void FillGaps() {
		List<Vector2Int> gaps = new List<Vector2Int>();

		foreach (KeyValuePair<Vector2Int, RoomNode> currentRoom in roomMap) {
			List<Vector2Int> openNeighbors = FindOpenNeighbors(currentRoom.Key);
			foreach (Vector2Int neighbor in openNeighbors) {
				if (!gaps.Contains(neighbor)) {
					gaps.Add(neighbor);
				}
			}
		}

		foreach (Vector2Int gap in gaps) {
			roomMap.Add(gap, new RoomNode());
			roomMap[gap].SetRoomType(RoomType.Gap);
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

	private List<Vector2Int> GetSpecialEnds() {
		List<Vector2Int> ends = new List<Vector2Int>();

		foreach (KeyValuePair<Vector2Int, RoomNode> currentRoom in roomMap) {
			if (currentRoom.Value.GetRoomType() == RoomType.Start) continue;

			if (currentRoom.Value.GetRoomType() == RoomType.End || currentRoom.Value.GetRoomType() == RoomType.Key) {
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
				case RoomType.Gap:
					roomToSpawn = gapRooms[Random.Range(0, gapRooms.Count)];
					break;
				case RoomType.Key:
					roomToSpawn = keyRooms[Random.Range(0, keyRooms.Count)];
					break;
			}

			//Debug.Log(room.Value.GetRoomType());

			if (!roomToSpawn) continue;
			GameObject newRoom = Instantiate(roomToSpawn.gameObject);
			newRoom.transform.parent = transform;
			newRoom.gameObject.SetActive(true);
			newRoom.transform.position = new Vector3(room.Key.x, 0f, room.Key.y) * gridScale;
			newRoom.name = "<" + room.Key.x + "," + room.Key.y + "> " + room.Value.GetRoomType();
			newRoom.GetComponent<DungeonRoom>().SetWalls(room.Value.openDoors);
			newRoom.GetComponent<DungeonRoom>().roomNode = room.Value;
		}

		foreach (DoorAndKeys door in lockedDoors) {
			GameObject newDoor = Instantiate(lockedDoor);
			newDoor.transform.parent = transform;
			newDoor.gameObject.SetActive(true);
			Vector2Int enteranceDirection = directions[(int)door.doorFacing];
			newDoor.transform.position = (new Vector3(door.doorRoom.x, 0f, door.doorRoom.y) * gridScale) + (new Vector3(enteranceDirection.x, 0f, enteranceDirection.y) * (gridScale*0.5f));
			newDoor.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f * (int)door.doorFacing, 0f));
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

	void OnDrawGizmos() {
		return;

		foreach (KeyValuePair<Vector2Int, RoomNode> room in roomMap) {
			Vector3 pos = new Vector3(room.Key.x, 1f, room.Key.y) * gridScale;
			Gizmos.color = Color.black;

			Gizmos.DrawCube(pos, new Vector3(gridScale, 1f, gridScale));
		}
		foreach (KeyValuePair<Vector2Int, RoomNode> room in roomMap) {
			Vector3 pos = new Vector3(room.Key.x, 1f, room.Key.y) * gridScale;
			Gizmos.color = Color.blue;
			switch (room.Value.GetRoomType()) {
				case RoomType.Start:
				case RoomType.End:
					Gizmos.color = Color.cyan;
					break;
				case RoomType.Combat:
					Gizmos.color = Color.red;
					break;
				case RoomType.Treasure:
					Gizmos.color = Color.green;
					break;
				case RoomType.Gap:
					Gizmos.color = Color.black;
					break;
				case RoomType.Key:
					Gizmos.color = Color.yellow;
					break;
			}

			Gizmos.DrawWireSphere(pos, gridScale*0.5f);

			if (room.Value.openDoors == null) continue;
			for (int i = 0; i < room.Value.openDoors.Count; i++) {
				Vector2Int dir = directions[(int)room.Value.openDoors[i]];
				pos = (new Vector3(room.Key.x, 1f, room.Key.y) * gridScale) + (new Vector3(dir.x, 0f, dir.y) * (gridScale*0.55f));

				Gizmos.DrawWireCube(pos, new Vector3(5f, 5f, 5f));
			}
		}
		foreach (DoorAndKeys door in lockedDoors) {
			Vector2Int room = door.doorRoom;
			Vector2Int dir = directions[(int)door.doorFacing];
			Vector3 pos = (new Vector3(room.x, 1f, room.y) * gridScale) + (new Vector3(dir.x, 0f, dir.y) * (gridScale*0.55f));
			Gizmos.color = Color.magenta;

			Gizmos.DrawWireCube(pos, new Vector3(10f, 10f, 10f));
		}
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