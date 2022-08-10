using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    #region Public Variables
    public float loadingDelay;
    #endregion

    #region Public Methods
    public void Start()
    {
        StartCoroutine(LoadScene(loadingDelay));
    }
    #endregion

    #region Private Methods
    private IEnumerator LoadScene(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    #endregion
}
