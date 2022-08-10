using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    #region Public Variables
    public AudioMixer audioMixer;
    #endregion

    # region Public Methods
    public void SetMasterVolume(float masterVolume)
    {
        Debug.Log("Current Volume: " + masterVolume);
        audioMixer.SetFloat("Master_volume", masterVolume);
    }
    #endregion
}
