using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScreen : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine(LoadLevelASync(1));
    }

    IEnumerator LoadLevelASync(int sceneToLoad)
    {
        float minDuration = 6f; // Minimum duration to wait (in seconds)
        
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        float startTime = Time.time;

        while (!loadOperation.isDone)
        {
            // Calculate the elapsed time
            float elapsedTime = Time.time - startTime;

            // If the loading is done and it's been less than 3 seconds, wait
            if (loadOperation.isDone && elapsedTime < minDuration)
            {
                float remainingTime = minDuration - elapsedTime;
                yield return new WaitForSeconds(remainingTime);
            }
            else
            {
                yield return null;
            }
        }
    }
}