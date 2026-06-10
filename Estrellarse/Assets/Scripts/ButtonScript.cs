using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [Header("Optional UI References")]
    public GameObject settingsCanvas;

    public void LoadLevel(string p = "")
    {
        SceneManager.LoadScene(p);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void Settings()
    {
        settingsCanvas.SetActive(!settingsCanvas.activeSelf);
    }
}
