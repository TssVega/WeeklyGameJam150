using UnityEngine;
using System.Collections;

public class EnemyEnder : MonoBehaviour {

	private GameMaster gameMaster;

	private void Start() {
		gameMaster = FindObjectOfType<GameMaster>();
	}
	public void OnTriggerEnter2D(Collider2D collision) {
		if(collision.CompareTag("Player")) {
			gameMaster.spawning = false;
		}		
	}
}
