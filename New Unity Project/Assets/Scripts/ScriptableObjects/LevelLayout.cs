using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class LevelLayout : ScriptableObject {

	public int extraLength = 420;
	public int levelLength = 100;
	public LevelTheme theme;
	public TimeOfDay time;
	public float planeProbability = 10f;
	public float tankProbability = 10f;
	public float soldierProbability = 10f;
	public float resendSpeed = 0.4f;
}
public enum LevelTheme {
	Grass, City, Desert
}
public enum TimeOfDay {
	Day, Sunset, Night
}
