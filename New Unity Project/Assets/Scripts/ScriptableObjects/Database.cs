using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class Database : ScriptableObject {

	public List<Sprite> trees;
	// 0 for ground, 1 for deep tiles, 2 for sky
	public List<Tile> grassTiles;
}
