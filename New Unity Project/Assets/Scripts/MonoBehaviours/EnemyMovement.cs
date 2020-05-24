using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMovement : MonoBehaviour {

	public Enemy enemy;
	private CameraMovement cam;
	private SpriteRenderer spriteRen;
	private readonly float xPosOffset = 15f;

	private void Awake() {
		cam = FindObjectOfType<CameraMovement>();
		spriteRen = GetComponent<SpriteRenderer>();
	}
	public void SetEnemy(Enemy enemy) {
		this.enemy = enemy;
		if(!spriteRen) {
			spriteRen = GetComponent<SpriteRenderer>();
		}
		spriteRen.sprite = enemy.sprite;
		if(!cam) {
			cam = FindObjectOfType<CameraMovement>();
		}
		int extraYPosition = (int)Random.Range(0f, 2.999999f) - 1;
		if(extraYPosition > 1) {
			extraYPosition = 1;
		}
		if(extraYPosition < -1) {
			extraYPosition = -1;
		}
		transform.position = new Vector3(cam.transform.position.x + xPosOffset, cam.transform.position.y + enemy.defaultYPosition + (extraYPosition * enemy.yPosOffset), 0f);
	}
	private void Update() {
		if(enemy.moving) {
			transform.Translate(Vector3.left * Time.deltaTime * enemy.movementSpeed);
		}
	}
}
