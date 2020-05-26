using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public string projectileName;
	private bool playerProjectile = false;
	private Vector3 destination;
	public float speed;
	public float lifetime;
	private float lifetimeCounter;

	public void SetProjectile(bool isPlayer, Vector3 destination) {
		lifetimeCounter = lifetime;				
		playerProjectile = isPlayer;
		if(!isPlayer) {
			this.destination = destination;
			transform.LookAt(destination, Vector3.up);
			transform.Rotate(new Vector3(0f, 90f, 0f));
		}
		else {
			transform.Rotate(new Vector3(0f, 0f, 90f));
		}
	}
	public void SetProjectile(Vector3 firePoint) {
		playerProjectile = false;
		lifetimeCounter = lifetime;
		this.destination = new Vector3(firePoint.x - 1, firePoint.y, firePoint.z) - firePoint;
	}
	private void Update() {
		if(gameObject.activeInHierarchy) {
			lifetimeCounter -= Time.deltaTime;
			transform.Translate(Vector3.left * speed * Time.deltaTime);
			if(lifetimeCounter <= 0) {
				gameObject.SetActive(false);
			}
			if(transform.position.y < 1) {
				GameObject explosion = ObjectPooler.objectPooler.GetPooledObject("LserExplosion");
				explosion.transform.position = transform.position;
				explosion.transform.rotation = Quaternion.identity;
				explosion.gameObject.SetActive(true);
				gameObject.SetActive(false);
			}
		}
	}
	public void OnTriggerEnter2D(Collider2D collision) {
		if(playerProjectile) {
			if(collision.CompareTag("Enemy")) {
				if(collision.GetComponent<EnemyStationary>()) {
					GameObject explosion = ObjectPooler.objectPooler.GetPooledObject("LserExplosion");
					explosion.transform.position = transform.position;
					explosion.transform.rotation = Quaternion.identity;
					explosion.gameObject.SetActive(true);
					collision.GetComponent<EnemyStationary>().Die();
					gameObject.SetActive(false);
				}
				else if(collision.GetComponent<EnemyMovement>()) {
					GameObject explosion = ObjectPooler.objectPooler.GetPooledObject("LserExplosion");
					explosion.transform.position = transform.position;
					explosion.transform.rotation = Quaternion.identity;
					explosion.gameObject.SetActive(true);
					collision.GetComponent<EnemyMovement>().Die();
					gameObject.SetActive(false);
				}
			}
			else if(collision.CompareTag("Prop")) {
				GameObject explosion = ObjectPooler.objectPooler.GetPooledObject("LserExplosion");
				explosion.transform.position = transform.position;
				explosion.transform.rotation = Quaternion.identity;
				explosion.gameObject.SetActive(true);
				gameObject.SetActive(false);
			}
		}
		else if(!playerProjectile) {
			if(collision.CompareTag("Player")) {
				// Kill the player
				collision.GetComponent<PlayerMovement>().Die();
				gameObject.SetActive(false);
			}
		}
	}
}
