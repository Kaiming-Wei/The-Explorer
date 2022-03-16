using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Back_btn : MonoBehaviour
{
    //Load to target scene
    public void ButtonToScene(string level)
    {
        SceneManager.LoadScene(level);
    }
}
