using UnityEngine;
using UnityEngine.Audio;
//---------------------------------------------------------------------------------
// Author		: Wee Heng
// Date  		: 2022-08-09
// Modified By	: Wee Heng
// Modified Date: 2022-08-09
// Description	: Script to handle Setting Menu Canvas UI events
//---------------------------------------------------------------------------------
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

    public void SetMusicVolume(float musicVolume)
    {
        Debug.Log("Current Volume: " + musicVolume);
        audioMixer.SetFloat("Music_volume", musicVolume);
    }

    public void SetSFXVolume(float sfxVolume)
    {
        Debug.Log("Current Volume: " + sfxVolume);
        audioMixer.SetFloat("SFX_volume", sfxVolume);
    }
    #endregion
}
