using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    public AudioSource bgm;

    public void PlayGame() {
        bgm.Stop();
        LoadingScript.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ToMainMenu() {
        bgm.Stop();
        PlayerPrefs.DeleteKey("CurrentScore");
        PlayerPrefs.Save();
        LoadingScript.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void QuitGame() {
        bgm.Stop();
        Application.Quit();
    }
}