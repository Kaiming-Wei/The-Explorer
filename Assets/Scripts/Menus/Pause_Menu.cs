using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause_Menu : MonoBehaviour
{

    public static bool IsInputEnabled = true;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject loadingPanel;
    public GameObject gamesavePanel;
    public Slider slider;

    void Start()
    {
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (!loadingPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape) && !DeathMenu.isDeathPanelActive)
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Pause_Menu.IsInputEnabled = true;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Pause_Menu.IsInputEnabled = false;
    }

    public void Menu()
    {
        loadingPanel.SetActive(true);
        pauseMenuUI.SetActive(false);

        //Stop WaitForSeconds goes forever
        Time.timeScale = 1f;

        StartCoroutine(LoadAsyncOperation());
    }

    // Load to the main menu
    IEnumerator LoadAsyncOperation()
    {
        yield return new WaitForSeconds(1.5f);

        AsyncOperation gameLevel = SceneManager.LoadSceneAsync("Menu_Scene");

        while (gameLevel.progress < 1)
        {
            slider.value = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }
    }

    // Exit the application
    public void exitApplication()
    {
        Application.Quit();
    }

    // Save the progress then resume the game
    public void saveGame()
    {
        // Get the player position
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        // Save position to the playerPrefs
        PlayerPrefs.SetFloat("current_player_position_x", playerPos.x);
        PlayerPrefs.SetFloat("current_player_position_y", playerPos.y);
        PlayerPrefs.SetFloat("current_player_position_z", playerPos.z);

        // Get current scene
        Scene scene = SceneManager.GetActiveScene();

        // Save current scene
        string currScene = scene.name;

        // Save current scene to the prefs
        PlayerPrefs.SetString("save_game_current_scene", currScene);

        // Save the status of key
        if (PlayerInteractKey.hasKey == true)
            PlayerPrefs.SetInt("Key", 1);
        else
            PlayerPrefs.SetInt("Key", 0);

        // Save current hp count
        PlayerPrefs.SetInt("Player_Hp_count", PlayerInteractHealth.currentHealth);

        // Save whether the current hp icon get picked by the player
        PlayerPrefs.SetInt("Player_Hp_Icon", PlayerInteractHealth.isHealthIcon);

        // Check whether the player got the weapon
        if (true)
        {
            const string have_weapon = "have_weapon";
            Data<bool, bool> data = (Data<bool, bool>)DataManager.Instance.GetData(have_weapon);
            if(data != null)
            {
                data.value2 = data.value1;
                DataManager.Instance.SaveData(have_weapon, data);
                Debug.Log("G");
            }
        }

        Resume();

        // Create a delay
        StartCoroutine(showSavedInfo());
    }

    // Make 2 seconds delay on game save notification
    IEnumerator showSavedInfo()
    {
        gamesavePanel.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        gamesavePanel.SetActive(false);
    }


}
