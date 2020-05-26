using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class Database : ScriptableObject {

	public List<Sprite> trees;
	// 0 for ground, 1 for deep tiles
	public List<Tile> grassTiles;
	public List<Tile> cityTiles;
	public List<Tile> desertTiles;
	// 0 for day, 1 for sunset, 2 and 3 for night
	public List<Tile> timeTiles;
	public List<LevelLayout> levels;
	public List<Building> buildings;
}
