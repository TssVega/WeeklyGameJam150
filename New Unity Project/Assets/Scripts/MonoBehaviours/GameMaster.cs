using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public Database database;
	private int currentLevel = 0;
	private readonly int yRange = 12;
	public LevelEnder levelEnder;
	public EnemyEnder enemyEnder;
	public Tilemap groundTilemap;
	public Tilemap skyBackgroundTilemap;
	public Tilemap skyStuffTilemap;
	public LevelLayout level;
	[HideInInspector] public bool spawning = false;
	private readonly int levelEndOffset = 20;
	private readonly int enemyEndOffset = 20;
	private PlayerMovement player;
	public GameObject nextLevelButton;
	private bool onLevelEnd = false;
	public Enemy jet;
	public Enemy soldier;
	public Enemy tank;

	private void Awake() {
		player = FindObjectOfType<PlayerMovement>();
	}
	private IEnumerator CheckForSpawns() {
		//int rotor = 0;      // Goes between 0, 1 and 2 to determine the next spawn type
		while(spawning) {
			if(!onLevelEnd) {
				float random = Random.Range(0f, 100f);
				if(random < level.planeProbability) {
					SpawnEnemy(jet);
					//rotor = 1;
				}
				if(random < level.soldierProbability) {
					SpawnEnemy(soldier);
					//rotor = 2;
				}
				if(random < level.tankProbability) {
					SpawnEnemy(tank);
					//rotor = 0;
				}
				yield return new WaitForSeconds(0.4f);
			}
		}		
	}
	private void SpawnEnemy(Enemy enemy) {
		GameObject enemyObject = ObjectPooler.objectPooler.GetPooledObject("Enemy");
		if(enemyObject) {
			enemyObject.GetComponent<EnemyMovement>().SetEnemy(enemy);
			enemyObject.SetActive(true);
		}
		else {
			Debug.LogWarning("null enemy");
		}
	}
	private void Start() {
		SetLevel();	
	}
	private void Update() {
		if(onLevelEnd) {
			if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
				SetLevel();
			}
		}
	}
	public void SetLevel() {
		spawning = true;		
		onLevelEnd = false;
		nextLevelButton.SetActive(false);
		ClearLevel();
		level = database.levels[currentLevel];
		if(level) {
			GenerateLevel();
		}
		player.StartLevel();
		StartCoroutine(CheckForSpawns());
	}
	public void EndLevel() {
		// Go to new level here
		spawning = false;
		currentLevel++;
		onLevelEnd = true;
		player.StopLevel();
		nextLevelButton.SetActive(true);
	}
	private void ClearLevel() {
		//groundTilemap.ClearAllTiles();
		//skyBackgroundTilemap.ClearAllTiles();
		groundTilemap.ClearAllEditorPreviewTiles();
		skyBackgroundTilemap.ClearAllEditorPreviewTiles();
	}
	private void GenerateLevel() {
		GenerateSky();
		GenerateGround();
	}
	private void GenerateGround() {
		List<Tile> tiles = new List<Tile>();
		if(level.theme == LevelTheme.City) {
			tiles = database.cityTiles;
		}
		else if(level.theme == LevelTheme.Grass) {
			tiles = database.grassTiles;
		}
		else if(level.theme == LevelTheme.Desert) {
			tiles = database.desertTiles;
		}
		groundTilemap.BoxFill(Vector3Int.zero, tiles[1], -15, -yRange, level.levelLength, 0);
		groundTilemap.BoxFill(Vector3Int.zero, tiles[0], -15, 0, level.levelLength, 0);
		levelEnder.transform.position = new Vector3(level.levelLength - levelEndOffset, 0, 0);
		enemyEnder.transform.position = new Vector3(level.levelLength - levelEndOffset - enemyEndOffset, 0, 0);
	}
	private void GenerateSky() {
		skyBackgroundTilemap.BoxFill(Vector3Int.zero, database.timeTiles[(int)level.time], -15, 0, level.levelLength, yRange);
	}
	private void FillSky() {
	
	}
	private void FillGround() {
	
	}
}
