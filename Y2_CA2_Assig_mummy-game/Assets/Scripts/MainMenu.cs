using UnityEngine;
using UnityEngine.SceneManagement;
//---------------------------------------------------------------------------------
// Author		: Wee Heng
// Date  		: 2022-08-09
// Modified By	: Wee Heng
// Modified Date: 2022-08-09
// Description	: Script to handle Main Menu Canvas UI events
//---------------------------------------------------------------------------------
public class MainMenu : MonoBehaviour
{
    #region  Public Variables
    public string sceneName;
    #endregion

    #region Public Methods
    public void ExitApp() // Exits game app
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Debug.Log("Exiting Application...");
            Application.Quit();
    }// End of ExitApp

    public void LoadGameScene() // Loads the game scene
    {
        Debug.Log("Loading Game...");

        // If time permits might do a transition

        SceneManager.LoadScene(sceneName);
    }// End of LoadGameScene
    #endregion
}
