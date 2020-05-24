using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed = 1f;
	// +1 is up, 0 is middle and -1 is down
	public int currentPosition = 0;
	public float positionChange = 1.5f;
	public float defaultPosition = 5.5f;
	public float positionChangeSpeed = 5f;
	private Coroutine positionShifter;
	private bool changingPosition = false;
	private bool moving = true;

	public void StopLevel() {
		moving = false;
	}
	public void StartLevel() {
		transform.position = new Vector3(-6f, defaultPosition, 0f);
		moving = true;
	}
	private void Update() {
		if(moving) {
			transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
			if(Input.GetKeyDown(KeyCode.UpArrow) && currentPosition < 1) {
				currentPosition++;
				ShiftPosition();
			}
			else if(Input.GetKeyDown(KeyCode.DownArrow) && currentPosition > -1) {
				currentPosition--;
				ShiftPosition();
			}
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
}
