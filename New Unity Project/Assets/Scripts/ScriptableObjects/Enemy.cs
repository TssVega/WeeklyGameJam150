using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class Enemy : ScriptableObject {

	public Sprite sprite;
	public Sprite soldierWeaponSprite;
	public Sprite tankCannonSprite;
	public float movementSpeed;
	public float rateOfFire;        // How many times this enemy will shoot in a second
	public float defaultYPosition;  // Where the enemy will appear on y coordinate
	public float yPosOffset;        // If this is 0 the enemy will always come from the same y position
									// If more than 0 the enemy may come above or below by yPosOffset
	public bool moving;				// Only true for planes
}
