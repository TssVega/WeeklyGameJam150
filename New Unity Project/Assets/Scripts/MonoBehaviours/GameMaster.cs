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
	public bool spawning = false;
	private readonly int levelEndOffset = 20;
	private readonly int enemyEndOffset = 30;
	private PlayerMovement player;
	public GameObject nextLevelButton;
	public GameObject failLevelButton;
	private bool onLevelEnd = false;
	public Enemy jet;
	public Enemy soldier;
	public Enemy tank;
	public List<GameObject> currentlyAliveEnemies;

	private void Awake() {
		player = FindObjectOfType<PlayerMovement>();
	}
	private IEnumerator CheckForSpawns() {
		while(spawning) {
			if(!onLevelEnd) {
				float jetRandom = Random.Range(0f, 100f);
				if(jetRandom < level.planeProbability) {
					SpawnEnemy(jet);
				}
				yield return new WaitForSeconds(level.resendSpeed);
				float soldierRandom = Random.Range(0f, 100f);
				if(soldierRandom < level.soldierProbability) {
					SpawnEnemy(soldier);
				}
				yield return new WaitForSeconds(level.resendSpeed);
				float tankRandom = Random.Range(0f, 100f);
				if(tankRandom < level.tankProbability) {
					SpawnEnemy(tank);
				}
				yield return new WaitForSeconds(level.resendSpeed);
			}
		}		
	}
	private void SpawnEnemy(Enemy enemy) {
		GameObject enemyObject = ObjectPooler.objectPooler.GetPooledObject(enemy.enemyName);
		if(enemyObject && enemyObject.GetComponent<EnemyMovement>()) {
			currentlyAliveEnemies.Add(enemyObject);
			enemyObject.SetActive(true);
			enemyObject.GetComponent<EnemyMovement>().SetEnemy(enemy);			
		}
		else if(enemyObject && enemyObject.GetComponent<EnemyStationary>()) {
			currentlyAliveEnemies.Add(enemyObject);
			enemyObject.SetActive(true);
			enemyObject.GetComponent<EnemyStationary>().SetEnemy(enemy);			
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
		if(currentlyAliveEnemies.Count > 0) {
			for(int i = 0; i < currentlyAliveEnemies.Count; i++) {
				currentlyAliveEnemies[i].SetActive(false);
				currentlyAliveEnemies.RemoveAt(i);
			}
		}		
		spawning = true;		
		onLevelEnd = false;
		nextLevelButton.SetActive(false);
		failLevelButton.SetActive(false);
		ClearLevel();
		level = database.levels[currentLevel];
		if(level) {
			GenerateLevel();
		}
		player.StartLevel();
		StartCoroutine(CheckForSpawns());
	}
	public void CheckLevelCompletion() {
		if(currentlyAliveEnemies.Count <= 0) {
			EndLevel();
		}
		else {
			FailLevel();
		}
	}
	public void FailLevel() {
		spawning = false;
		onLevelEnd = true;
		player.StopLevel();
		failLevelButton.SetActive(true);
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
