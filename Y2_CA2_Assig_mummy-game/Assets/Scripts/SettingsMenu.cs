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
    #endregion
}
