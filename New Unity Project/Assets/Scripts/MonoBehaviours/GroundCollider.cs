using UnityEngine;
using System.Collections;

public class GroundCollider : MonoBehaviour {

	public void OnTriggerEnter2D(Collider2D collision) {
		if(collision.CompareTag("Projectile")) {
			GameObject explosion = ObjectPooler.objectPooler.GetPooledObject("LserExplosion");
			explosion.transform.position = collision.transform.position;
			explosion.transform.rotation = Quaternion.identity;
			explosion.gameObject.SetActive(true);
			collision.gameObject.SetActive(false);
		}
	}
}
