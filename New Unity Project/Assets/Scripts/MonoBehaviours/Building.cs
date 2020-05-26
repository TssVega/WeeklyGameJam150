﻿using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

	public float defaultYPos;
	public float collapseSpeed = 5f;
	private readonly float collapseTime = 2f;
	private readonly float squeezeFactor = 0.04f;
	private readonly float shakeFactor = 0.8f;
	private readonly float shakeSpeed = 2f;
	private float currentYScale = 2f;
	// private float 
	private float collapseTimer;
	private bool collapsing = false;

	private void Start() {
		transform.position = new Vector3(transform.position.x, defaultYPos, transform.position.z);
	}
	public void OnTriggerEnter2D(Collider2D collision) {
		if(collision.CompareTag("Projectile")) {
			collapseTimer = collapseTime;
			collapsing = true;
			GameObject smoke = ObjectPooler.objectPooler.GetPooledObject("Collapse");
			smoke.transform.position = new Vector3(transform.position.x, 1, 0); //	transform.position + defaultYPos;
			smoke.transform.rotation = Quaternion.identity;
			smoke.gameObject.SetActive(true);
			StartCoroutine(ShakeBuilding());
			// transform.localScale = new Vector3(2, 1.9f, 2);
		}
		if(collision.CompareTag("Player")) {
			collision.GetComponent<PlayerMovement>().Die();
			collapseTimer = collapseTime;
			collapsing = true;
			GameObject smoke = ObjectPooler.objectPooler.GetPooledObject("Collapse");
			smoke.transform.position = new Vector3(transform.position.x, 1, 0); //	transform.position + defaultYPos;
			smoke.transform.rotation = Quaternion.identity;
			smoke.gameObject.SetActive(true);
			StartCoroutine(ShakeBuilding());
		}
	}
	private void Update() {
		if(collapsing) {
			collapseTimer -= Time.deltaTime;
			transform.localScale = new Vector3(2f, currentYScale - squeezeFactor, 2f);
			currentYScale -= squeezeFactor;
			transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * collapseSpeed, transform.position.z);
			if(collapseTimer <= 0f) {
				collapsing = false;
				gameObject.SetActive(false);
			}
		}
	}
	private IEnumerator ShakeBuilding() {
		int rotor = 0;
		while(collapsing) {
			Vector3 targetPos = new Vector3();
			if(rotor == 0) {
				targetPos = new Vector3(transform.position.x + shakeFactor, transform.position.y, 0f);
				rotor = 1;
			}
			else if(rotor == 1) {
				targetPos = new Vector3(transform.position.x - shakeFactor, transform.position.y, 0f);
				rotor = 0;
			}
			transform.position = Vector3.Lerp(transform.position, targetPos, shakeSpeed * Time.deltaTime);
			yield return null;
		}
	}
}