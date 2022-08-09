using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RecordController : MonoBehaviour
{
    // Control the platter to get music
    [SerializeField] XRSocketInteractor platter;

    // Control the handle to get state
    [SerializeField] Transform handle;

    // Control the music player
    [SerializeField] PlayContinuousSound player;


    // Keep track if it is playing already
    bool playing = false;

    // Update is called once per frame
    void Update()
    {
        AudioClip audio = getAudioClip();
        if (!playing) {
            if ((audio != null) && handleActivated()) {
                playing = true;
                // Play audio
                player.sound = audio;
                player.Play();
            }   
        }
        else {
            // Spin platter
            platter.transform.Rotate(Vector3.up * Time.deltaTime * 10, Space.Self);
            if ((audio == null) || !handleActivated()) {
                playing = false;
                // Stop audio
                player.Pause();
            }
        }
    }

    bool handleActivated() {
        // Debug.Log($"Handle is active: {handle.localEulerAngles.y}");
        return handle.localEulerAngles.y >= 45;
    }

    AudioClip getAudioClip() {
        if (platter.hasSelection) {
            Transform disk = platter.firstInteractableSelected.transform;
            AudioClip clip = disk.GetComponent<DiskController>().clip;
            // Debug.Log($"Got clip: {clip}");
            return clip;
        }
        return null;
    }
}
