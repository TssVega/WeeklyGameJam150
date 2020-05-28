using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	private Transform playerTransform;
	private readonly float lookAhead = 7f;

	private void Start() {
		playerTransform = FindObjectOfType<PlayerMovement>().transform;
	}
	private void LateUpdate() {
		transform.position = new Vector3(playerTransform.position.x + lookAhead, transform.position.y, -10f);
	}
}
