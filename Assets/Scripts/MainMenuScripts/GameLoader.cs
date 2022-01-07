using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public AudioSource audioSource;
    bool mute;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void LoadScene(string sceneName)
    {
        Screen.SetResolution(540, 960, false);
        SceneManager.LoadScene("Game");
    }

    //Pit‰‰ yhdist‰‰ yhdeksi methodiksi lopuksi.
    public void LoadTutorial()
    {
        Screen.SetResolution(540, 960, false);
        SceneManager.LoadScene("HowToPlay");
    }

    public void Mute()
    {
        mute = !mute;
        AudioListener.volume = mute ? 0 : 1;
    }

}
