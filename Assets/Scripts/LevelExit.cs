using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(LoadNextLevel());

    }
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        FindObjectOfType<Persist>().ResetGameSession();
        SceneManager.LoadScene(nextSceneIndex);

    }
}
