using UnityEngine;
using System.Collections;

public class EnemyStationary : MonoBehaviour {

	public Enemy enemy;
	public Transform gunOrBarrel;
	public Transform firePos;
	private CameraMovement cam;
	private SpriteRenderer spriteRen;
	private readonly float xPosOffset = 15f;
	private PlayerMovement player;
	private bool alive;
	private GameMaster gameMaster;

	public void SetEnemy(Enemy enemy) {
		gameMaster = FindObjectOfType<GameMaster>();
		alive = true;
		player = FindObjectOfType<PlayerMovement>();
		this.enemy = enemy;
		if(!spriteRen) {
			spriteRen = GetComponent<SpriteRenderer>();
		}
		spriteRen.sprite = enemy.sprite;
		if(!cam) {
			cam = FindObjectOfType<CameraMovement>();
		}
		transform.position = new Vector3(cam.transform.position.x + xPosOffset, cam.transform.position.y + enemy.defaultYPosition, 0f);
		StartCoroutine(CheckFire());
	}
	private IEnumerator CheckFire() {
		while(alive) {
			Fire();
			yield return new WaitForSeconds(1f / enemy.rateOfFire);
		}
	}
	private void Fire() {
		Vector3 target = player.targets[Mathf.FloorToInt(Random.Range(0f, 2.999999f))].position;
		// Debug.Log(target);
		// target.z = 0;
		// gunOrBarrel.rotation = Quaternion.LookRotation(target);
		gunOrBarrel.LookAt(target, Vector3.up);
		gunOrBarrel.Rotate(new Vector3(0, 90, 0));
		GameObject proj = ObjectPooler.objectPooler.GetPooledObject(enemy.projectile.GetComponent<Projectile>().projectileName);
		if(proj) {
			proj.transform.position = firePos.position;
			proj.transform.rotation = firePos.rotation;
			proj.SetActive(true);
			proj.GetComponent<Projectile>().SetProjectile(false, target);
		}
		else {
			Debug.LogWarning("null projectile");
		}
		// gunOrBarrel.LookAt(target, Vector3.right);
	}
	public void Die() {
		gameMaster.currentlyAliveEnemies.Remove(this.gameObject);
		alive = false;
		gameObject.SetActive(false);
	}
}
