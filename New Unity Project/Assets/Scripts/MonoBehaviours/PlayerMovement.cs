using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed = 1f;
	// +1 is up, 0 is middle and -1 is down
	public int currentPosition = 0;
	public float positionChange = 1.5f;
	public float defaultPosition = 5.5f;
	public float positionChangeSpeed = 5f;
	private Coroutine positionShifter;
	private bool changingPosition = false;
	private bool moving = false;
	public List<Transform> targets;
	public List<Transform> firePositions;
	public List<Transform> forwardFirePositions;
	public GameObject projectile;
	public float fireRate = 10f;
	private float cooldownTimer = 0f;

	public void StopLevel() {
		moving = false;
	}
	public void StartLevel() {
		gameObject.SetActive(true);
		cooldownTimer = 0f;
		currentPosition = 0;
		transform.position = new Vector3(-6f, defaultPosition, 0f);
		moving = true;
	}
	private void Update() {
		if(moving) {
			if(cooldownTimer > 0) {
				cooldownTimer -= Time.deltaTime;
			}
			transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
			if(Input.GetKeyDown(KeyCode.UpArrow) && currentPosition < 1) {
				currentPosition++;
				ShiftPosition();
			}
			else if(Input.GetKeyDown(KeyCode.DownArrow) && currentPosition > -1) {
				currentPosition--;
				ShiftPosition();
			}
			if(Input.GetKey(KeyCode.Z) && cooldownTimer <= 0) {
				Fire(forwardFirePositions[0].position - firePositions[0].position, firePositions[0]);
				cooldownTimer = 1f / fireRate;
			}
			else if(Input.GetKey(KeyCode.X) && cooldownTimer <= 0) {
				Fire(forwardFirePositions[1].position - firePositions[1].position, firePositions[1]);
				cooldownTimer = 1f / fireRate;
			}
			else if(Input.GetKey(KeyCode.C) && cooldownTimer <= 0) {
				Fire(forwardFirePositions[2].position - firePositions[2].position, firePositions[2]);
				cooldownTimer = 1f / fireRate;
			}
		}		
	}
	private void Fire(Vector3 fireDirection, Transform fireTransform) {
		GameObject proj = ObjectPooler.objectPooler.GetPooledObject(projectile.GetComponent<Projectile>().projectileName);
		if(proj) {
			proj.transform.position = fireTransform.position;
			proj.transform.rotation = fireTransform.rotation;
			proj.GetComponent<Projectile>().SetProjectile(true, transform.position - fireDirection);
			proj.SetActive(true);			
		}
	}
	private void ShiftPosition() {
		if(changingPosition) {
			CancelShifting();
		}
		else {
			positionShifter = StartCoroutine(ChangeVerticalPosition());
		}
	}
	private void CancelShifting() {
		StopCoroutine(positionShifter);
		positionShifter = StartCoroutine(ChangeVerticalPosition());	
	}
	private IEnumerator ChangeVerticalPosition() {
		changingPosition = true;
		bool arrivedDesiredPosition = false;
		while(!arrivedDesiredPosition) {
			Vector3 targetPos = new Vector3(transform.position.x, defaultPosition + (currentPosition * positionChange), 0f);
			transform.position = Vector3.Lerp(transform.position, targetPos, positionChangeSpeed * Time.deltaTime);
			if(transform.position == targetPos) {
				arrivedDesiredPosition = true;
			}
			yield return null;
		}
		changingPosition = false;
	}
	public void Die() {
		FindObjectOfType<GameMaster>().FailLevel();
		GameObject explosion = ObjectPooler.objectPooler.GetPooledObject("ShipExplode");
		explosion.transform.position = transform.position;
		explosion.transform.rotation = Quaternion.identity;
		explosion.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}
}
