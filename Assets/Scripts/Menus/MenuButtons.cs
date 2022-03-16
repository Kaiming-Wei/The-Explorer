using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    public Transform Indicator_1;
    public GameObject gameplayPanel;
    public GameObject gameoverPanel;


    // Start is called before the first frame update
    void Start()
    {
        Indicator_1.gameObject.SetActive(false);
    }


    //Display indicator icon
    public void mouseIn()
    {
        Indicator_1.gameObject.SetActive(true);
    }

    //Hide indicator icon
    public void mouseOut()
    {
        Indicator_1.gameObject.SetActive(false);
    }

    //Load to target scene
    public void ButtonToScene(string level)
    {
        // Save a bool to check whether player click resume button or new game button
        // 1 == Resume, 0 == newGame
        PlayerPrefs.SetInt("Resume_OR_newGame", 0);
        PlayerInteractHealth.currentHealth = 4;

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

        SceneManager.LoadScene(level);
    }

    //Exit Button
    public void ExitApplication()
    {
        Application.Quit();
    }

    //Click game play button to display game play instruction
    public void showGameplay()
    {
        gameplayPanel.SetActive(true);
    }

    //Hide game play instruction
    public void hideGameplay()
    {
        gameplayPanel.SetActive(false);
    }

    //Click game play button to display game over information
    public void showGameover()
    {
        gameoverPanel.SetActive(true);
    }

    //Hide game over
    public void hideGameover()
    {
        gameoverPanel.SetActive(false);
    }
}
