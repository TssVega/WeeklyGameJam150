using UnityEngine;
using System.Collections;

[System.Serializable]
public class Sound {

    public string audioName;                                    // Name of the audio
    public AudioClip audioClip;                                 // Audio file

    [Range(0f, 1f)] public float volume = 1f;                   // Set the normal volume
    [SerializeField] private float currentVolume;
    [Range(0.2f, 1.8f)] public float pitch = 1f;                // Set the normal pitch
    [SerializeField] private float currentPitch;
    // public float soundEndingOffset = 0.3f;

    public bool looping;                                        // Set true if the audio is supposed to loop

    public bool randomizeVolume;                                // Set true if you want randomized volume
    public bool randomizePitch;                                 // Set true if you want randomized pitch
    [Range(0f, 0.5f)] public float volumeRandomness = 0.1f;     // Randomness of the volume
    [Range(0f, 0.5f)] public float pitchRandomness = 0.1f;      // Randomness of the pitch

    private AudioSource audioSource;

    private bool currentVolumeSet = false;
    private bool currentPitchSet = false;

    // Set the audio source
    public void SetSource(AudioSource _source) {
        _source.loop = looping;
        audioSource = _source;
        audioSource.clip = audioClip;
    }

    private void ChangeVolume() {
        // If the current values are not the default values, set them
        if(currentVolume != volume && !currentVolumeSet) {
            currentVolume = volume;
            currentVolumeSet = true;
        }
        // Randomize current value
        currentVolume =
            randomizeVolume ? currentVolume *= 1 +
            (Random.Range(-volumeRandomness / 2, volumeRandomness / 2)) : currentVolume *= 1;
        // These statemens are keeping the current volume between randomness offsets
        if(currentVolume > volume + volumeRandomness && randomizeVolume) {
            currentVolume = volume + volumeRandomness;
        }
        if(currentVolume < volume - volumeRandomness && randomizeVolume) {
            currentVolume = volume - volumeRandomness;
        }
    }

    private void ChangePitch() {
        // If the current values are not the default values, set them
        if(currentPitch != pitch && !currentPitchSet) {
            currentPitch = pitch;
            currentPitchSet = true;
        }
        // Randomize current value
        currentPitch =
            randomizePitch ? currentPitch *= 1 +
            (Random.Range(-pitchRandomness / 2, pitchRandomness / 2)) : currentPitch *= 1;
        // These statemens are keeping the current volume between randomness offsets
        if(currentPitch > pitch + pitchRandomness && randomizePitch) {
            currentPitch = pitch + pitchRandomness;
        }
        if(currentPitch < pitch - pitchRandomness && randomizePitch) {
            currentPitch = pitch - pitchRandomness;
        }
    }

    public void Play() {
        // Change the randomness
        ChangeVolume();
        ChangePitch();
        // Set the values
        audioSource.volume = currentVolume;
        audioSource.pitch = currentPitch;
        // Play
        audioSource.Play();
    }

    public void Stop() {
        audioSource.Stop();
    }
}
