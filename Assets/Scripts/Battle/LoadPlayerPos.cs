using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerPos : MonoBehaviour
{

    public GameObject player;
    public static int isResume;

    const int max_int = int.MaxValue;

    void Awake()
    {
        isResume = PlayerPrefs.GetInt("Resume_OR_newGame", max_int);

        // If position exist in the player prefs, set new position to the player
        if (isResume == 1 && PlayerPrefs.GetFloat("current_player_position_x", max_int) != max_int)
        {
            // Set isResume to 0 
            PlayerPrefs.SetInt("Resume_OR_newGame", 0);

            float x, y, z;
            x = PlayerPrefs.GetFloat("current_player_position_x", max_int);
            y = PlayerPrefs.GetFloat("current_player_position_y", max_int);
            z = PlayerPrefs.GetFloat("current_player_position_z", max_int);

            player.transform.position = new Vector3(x, y, z);
        }
    }
}
