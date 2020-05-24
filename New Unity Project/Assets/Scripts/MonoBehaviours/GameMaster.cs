using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public Database database;
	public int yRange = 12;
	public Tilemap groundTilemap;
	public Tilemap skyBackgroundTilemap;
	public Tilemap skyStuffTilemap;
	public LevelTheme theme;
	public TimeOfDay time;
	public int levelLength = 100;
	//public float 

	private void Start() {
		GenerateLevel();
	}
	private void GenerateLevel() {
		GenerateSky();
		GenerateGround();
	}
	private void GenerateGround() {
		groundTilemap.BoxFill(Vector3Int.zero, database.grassTiles[1], -15, -yRange, levelLength, 0);
		groundTilemap.BoxFill(Vector3Int.zero, database.grassTiles[0], -15, 0, levelLength, 0);
		
	}
	private void GenerateSky() {
		// skyBackgroundTilemap.ResizeBounds(Vector3Int.zero, -15, -yRange, levelLength, yRange);
		skyBackgroundTilemap.BoxFill(Vector3Int.zero, database.grassTiles[2], -15, 0, levelLength, yRange);
	}
	private void FillSky() {
	
	}
	private void FillGround() {
	
	}
}
public enum LevelTheme {
	Grass, City, Desert
}
public enum TimeOfDay {
	Day, Sunset, Night
}
