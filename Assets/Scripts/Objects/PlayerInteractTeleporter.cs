using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInteractTeleporter : MonoBehaviour
{

    public GameObject loadingPanel;
    public Slider slider;
    public string nextLevel;
    public GameObject noKeyInfo;

    public new AudioSource audio;
    float musicSound = 0.0f;


    void Start()
    {
        // Retrieve the sound value from prefab
        musicSound = PlayerPrefs.GetFloat("Sound value");
        audio.volume = musicSound;
    }

    // Collide with teleporter.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Teleporter"))
        {
            // Get current scene
            Scene scene = SceneManager.GetActiveScene();

            // If the current scene is the last level then remove the record
            if(scene.name == "Level-Boss")
            {
                PlayerPrefs.DeleteKey("save_game_current_scene");
            }

            // If the key doesn't collect yet, then show the notification else load the next scene
            if (!PlayerInteractKey.hasKey)
            {
                StartCoroutine(showNoKeyInfo());
            }
            else
            {
                audio.Play();

                loadingPanel.SetActive(true);

                StartCoroutine(LoadAsyncOperation());
            }
        }
    }

    // Display no key notification for 2 seconds then hide again
    IEnumerator showNoKeyInfo()
    {
        noKeyInfo.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        noKeyInfo.SetActive(false);
    }

    // Sync slider value with loading progression
    IEnumerator LoadAsyncOperation()
    {
        yield return new WaitForSeconds(1.5f);

        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(nextLevel);

        while (gameLevel.progress < 1)
        {
            slider.value = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }

    }
}
