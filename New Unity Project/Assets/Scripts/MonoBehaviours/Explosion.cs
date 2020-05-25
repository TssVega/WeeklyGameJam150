using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	private float lifetime = 1f;
	private float lifetimeCounter;

	private void OnEnable() {
		lifetimeCounter = lifetime;
	}
	private void Update() {
		lifetimeCounter -= Time.deltaTime;
		if(lifetimeCounter <= 0f) {
			gameObject.SetActive(false);
		}
	}
}
