using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMovement : MonoBehaviour {

	public Enemy enemy;
	private CameraMovement cam;
	private SpriteRenderer spriteRen;
	private readonly float xPosOffset = 15f;
	private bool alive;
	public Transform firePoint;

	public void SetEnemy(Enemy enemy) {
		alive = true;
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
		StartCoroutine(CheckFire());
	}
	private void Update() {
		if(enemy.moving) {
			transform.Translate(Vector3.left * Time.deltaTime * enemy.movementSpeed);
		}
	}
	public void Die() {
		alive = false;
		gameObject.SetActive(false);
	}
	private IEnumerator CheckFire() {
		while(alive) {
			Fire();
			yield return new WaitForSeconds(1f / enemy.rateOfFire);
		}
	}
	private void Fire() {
		GameObject proj = ObjectPooler.objectPooler.GetPooledObject(enemy.projectile.GetComponent<Projectile>().projectileName);
		if(proj) {
			proj.transform.position = firePoint.position;
			proj.transform.rotation = firePoint.rotation;
			proj.SetActive(true);
			proj.GetComponent<Projectile>().SetProjectile(firePoint.position);
		}
	}
}
