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
	public GameObject startGamePanel;
	public GameObject sun;
	public GameObject moon;
	private bool onLevelEnd = false;
	private bool onStartMenu = true;
	public Enemy jet;
	public Enemy soldier;
	public Enemy tank;
	public List<GameObject> currentlyAliveEnemies;
	private Coroutine spawnChecker;

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
		startGamePanel.SetActive(true);
	}
	private void Update() {
		if(onLevelEnd) {
			if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
				SetLevel();
			}
		}
		else if(onStartMenu) {
			if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
				StartGame();
				onStartMenu = false;
			}
		}
	}
	public void StartGame() {
		SetLevel();
		startGamePanel.SetActive(false);
	}
	public void SetLevel() {
		// ClearEnemies();
		spawning = true;		
		onLevelEnd = false;
		ClearEnemies();
		nextLevelButton.SetActive(false);
		failLevelButton.SetActive(false);
		ClearLevel();
		level = database.levels[currentLevel];
		if(level) {
			GenerateLevel();
		}
		player.StartLevel();
		spawnChecker = StartCoroutine(CheckForSpawns());
	}
	private void ClearEnemies() {
		if(currentlyAliveEnemies.Count > 0) {
			for(int i = 0; i < currentlyAliveEnemies.Count; i++) {
				currentlyAliveEnemies[i].SetActive(false);				
			}
			currentlyAliveEnemies.Clear();
		}
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
		if(spawning) {
			StopCoroutine(spawnChecker);
		}
		spawning = false;
		onLevelEnd = true;
		player.StopLevel();
		failLevelButton.SetActive(true);
	}
	public void EndLevel() {
		// Go to new level here
		if(spawning) {
			StopCoroutine(spawnChecker);
		}		
		spawning = false;
		currentLevel++;
		onLevelEnd = true;
		player.StopLevel();
		nextLevelButton.SetActive(true);
	}
	private void ClearLevel() {
		//groundTilemap.ClearAllTiles();
		//skyBackgroundTilemap.ClearAllTiles();
		//groundTilemap.ClearAllEditorPreviewTiles();
		//skyBackgroundTilemap.ClearAllEditorPreviewTiles();
		foreach(Trees t in FindObjectsOfType<Trees>()) {
			t.gameObject.SetActive(false);
		}
		foreach(Building b in FindObjectsOfType<Building>()) {
			b.gameObject.SetActive(false);
		}
		foreach(Projectile p in FindObjectsOfType<Projectile>()) {
			p.gameObject.SetActive(false);
		}
		foreach(Explosion e in FindObjectsOfType<Explosion>()) {
			e.gameObject.SetActive(false);
		}
	}
	private void GenerateLevel() {
		GenerateSky();
		GenerateGround();
		FillGround();
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
		groundTilemap.BoxFill(Vector3Int.zero, tiles[1], -15, -yRange, level.extraLength, 0);
		groundTilemap.BoxFill(Vector3Int.zero, tiles[0], -15, 0, level.extraLength, 0);
		levelEnder.transform.position = new Vector3(level.levelLength - levelEndOffset, 0, 0);
		enemyEnder.transform.position = new Vector3(level.levelLength - levelEndOffset - enemyEndOffset, 0, 0);
	}
	private void GenerateSky() {
		if(level.time == TimeOfDay.Day || level.time == TimeOfDay.Sunset) {
			sun.SetActive(true);
			moon.SetActive(false);
		}
		else {
			sun.SetActive(false);
			moon.SetActive(true);
		}
		skyBackgroundTilemap.BoxFill(Vector3Int.zero, database.timeTiles[(int)level.time], -15, 0, level.extraLength, yRange);
	}
	private void FillGround() {
		if(level.theme == LevelTheme.City) {
			for(int i = 0; i < level.levelLength / 5; i++) {
				Vector3 pos = new Vector3(transform.position.x + i * 5, transform.position.y, 0);
				Instantiate(database.buildings[(int)Random.Range(0f, 3.99999f)], pos, Quaternion.identity);
			}
		}
		else if(level.theme == LevelTheme.Grass) {
			for(int i = 0; i < level.levelLength / 5; i++) {
				Vector3 pos = new Vector3(transform.position.x + i * 5, transform.position.y + 1.8f, 0);
				GameObject tree = ObjectPooler.objectPooler.GetPooledObject("Tree");
				tree.transform.position = pos;
				tree.transform.rotation = Quaternion.identity;
				tree.GetComponent<SpriteRenderer>().sprite = database.trees[(int)Random.Range(0f, 13.99999f)];
				tree.SetActive(true);
			}
		}
	}
}
