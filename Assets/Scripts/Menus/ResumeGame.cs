using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ResumeGame : MonoBehaviour
{

    public Image resumeBG;
    public Button resumeBTN;
    public GameObject textGO;
    public GameObject indicatorGO;
    public GameObject musicGO;
    public EventTrigger eventTrigger;
    public Button resumeBtn;
    public GameObject loadingPanel;
    public Slider slider;

    private Text resumeText;
    private Image indicatorIcon;
    private Animator indicatorAnim;
    private AudioSource clickAudio;

    void Awake()
    {
        if (PlayerPrefs.GetString("save_game_current_scene", "No Name") == "No Name")
        {
            resumeText = textGO.GetComponent<Text>();
            indicatorIcon = indicatorGO.GetComponent<Image>();
            indicatorAnim = indicatorGO.GetComponent<Animator>();
            clickAudio = musicGO.GetComponent<AudioSource>();

            resumeBG.color = new Color(0.58f, 0.58f, 0.58f, 0.31f);
            resumeText.color = new Color(0.50f, 0.50f, 0.50f, 1.0f);
            resumeBTN.interactable = false;
            indicatorIcon.enabled = false;
            indicatorAnim.enabled = false;
            clickAudio.enabled = false;
            eventTrigger.enabled = false;
        }
    }

    void Start()
    {
        // Add click event to the button
        resumeBtn.onClick.AddListener(resumeTheGame);
    }

    // Resume button clicked
    public void resumeTheGame()
    {
        //Remove weapon
        if (true)
        {
            const string have_weapon = "have_weapon";
            Data<bool, bool> data = (Data<bool, bool>)DataManager.Instance.GetData(have_weapon);
            if (data != null)
            {
                data.value1 = data.value2;
                DataManager.Instance.SaveData(have_weapon, data);
            }
        }

        // Save a bool to check whether player click resume button or new game button
        // 1 == Resume, 0 == newGame
        PlayerPrefs.SetInt("Resume_OR_newGame", 1);

        loadingPanel.SetActive(true);


        // Load to the target scene
        StartCoroutine(LoadAsyncOperation());
    }

    // Load to the main menu
    IEnumerator LoadAsyncOperation()
    {
        yield return new WaitForSeconds(1.5f);

        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(PlayerPrefs.GetString("save_game_current_scene", "No Name"));

        while (gameLevel.progress < 1)
        {
            slider.value = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
