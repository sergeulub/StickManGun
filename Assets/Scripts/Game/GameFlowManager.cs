using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameFlowManager : MonoBehaviour
{
    [Header("Pause")]
    public GameObject pauseUI;
    public bool isPaused;

    [Header("Audio")]
    public AudioMixer masterMixer;
    private bool soundEnabled = true;

    private const string MASTER_VOLUME = "MasterVolume";

    public void _ToggleSound()
    {
        ToggleSound();
    }
    public void _ExitToMenu()
    {
        ExitToMenu();
    }

    public void _TogglePause()
    {
        TogglePause();
    }
    // ---------------- PAUSE ----------------

    private void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseUI != null)
            pauseUI.SetActive(true);
    }

    private void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseUI != null)
            pauseUI.SetActive(false);
    }

    // ---------------- AUDIO ----------------

    private void ToggleSound()
    {
        soundEnabled = !soundEnabled;
        //SetSound(soundEnabled);
        Debug.Log($"Audio - {soundEnabled}");
    }

    private void SetSound(bool enabled)
    {
        //soundEnabled = enabled;

        //// 0 dB = включено, -80 dB = мут
        //masterMixer.SetFloat(MASTER_VOLUME, enabled ? 0f : -80f);
    }

    // ---------------- SCENES ----------------
    private void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

}
