using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadingScript : MonoBehaviour
{
    public static LoadingScript Instance;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject currentScreen;
    [SerializeField] private Slider loadingSlider;

    public TMP_Text progressPercentage;

    private void Awake()
    {
        // Set the instance to this script when it's first loaded
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // If there is already an instance, destroy the new one
            Destroy(gameObject);
        }
    }

    public void LoadScene(int sceneToLoad)
    {
        currentScreen.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelASync(sceneToLoad));
    }

    IEnumerator LoadLevelASync(int sceneToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            progressPercentage.text = Mathf.Floor(progressValue * 100f) + "%";
            yield return null;
        }
    }
}