using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public static bool isDeathPanelActive = false;
    public Slider slider;
    public GameObject loadingPanel;
    public GameObject deathPanel;
    public GameObject gameobject;
    public GameObject bossSliders;
    private bool isLoadingActivate = false;
    //Animator animator;


    void Update()
    {
        // If die, display death scene
        if (!isLoadingActivate && isDeathPanelActive)
        {
            if (SceneManager.GetActiveScene().name == "Level-Boss")
            {
                bossSliders.SetActive(false);
            }

            Player_Inputs.enabled = true;
            gameobject.SetActive(false);
            deathPanel.SetActive(true);

            Time.timeScale = 0f;
        }
    }

    // Back to the main menu
    public void menu()
    {
        deathPanel.SetActive(false);
        loadingPanel.SetActive(true);
        isLoadingActivate = true;

        PlayerInteractHealth.currentHealth = 4;
        Time.timeScale = 1f;
        isDeathPanelActive = false;

        StartCoroutine(LoadAsyncOperation("Menu_Scene"));
    }

    // Restart the game
    public void restart()
    {
        deathPanel.SetActive(false);
        loadingPanel.SetActive(true);

        PlayerInteractHealth.currentHealth = 4;
        Time.timeScale = 1f;
        isDeathPanelActive = false;

        //Remove weapon
        if (true)
        {
            const string have_weapon = "have_weapon";
            Data<bool, bool> data = (Data<bool, bool>)DataManager.Instance.GetData(have_weapon);
            if (data != null)
            {
                data.value1 = false;
                DataManager.Instance.SaveData(have_weapon, data);
            }
        }

        StartCoroutine(LoadAsyncOperation("Level-01"));
    }

    // Sync slider value with loading progression
    IEnumerator LoadAsyncOperation(string str)
    {
        yield return new WaitForSeconds(1.5f);

        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(str);

        while (gameLevel.progress < 1)
        {
            slider.value = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }

    }

    // Exit game
    public void exitApplication()
    {
        Application.Quit();
    }
}
