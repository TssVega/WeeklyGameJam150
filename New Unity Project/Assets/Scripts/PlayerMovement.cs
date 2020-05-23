using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed = 1f;
	// +1 is up, 0 is middle and -1 is down
	public int currentPosition = 0;
	public float positionChange = 1.5f;

	private void Update() {
		transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
		if(Input.GetKeyDown(KeyCode.UpArrow) && currentPosition < 1) {
			currentPosition++;
		}
		else if(Input.GetKeyDown(KeyCode.DownArrow) && currentPosition > -1) {
			currentPosition--;
		}
	}
}
