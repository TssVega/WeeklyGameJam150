using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public static SoundManager audioManager;

    public Sound[] sounds;

    private void Awake() {
        audioManager = this;
    }
    private void Start() {
        for(int i = 0; i < sounds.Length; i++) {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].audioName);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
    }
    public void PlaySound(string _name) {
        for(int i = 0; i < sounds.Length; i++) {
            if(sounds[i].audioName == _name) {
                sounds[i].Play();
            }
        }
    }
    public void StopSound(string _name) {
        for(int i = 0; i < sounds.Length; i++) {
            if(sounds[i].audioName == _name) {
                sounds[i].Stop();
            }
        }
    }
}
