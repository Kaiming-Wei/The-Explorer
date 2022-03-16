using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inputs : MonoBehaviour
{

    public static Player_Inputs instance;
    public static bool enabled = true;

    //Input event 
    public InputBotton Pause = new InputBotton(KeyCode.Escape);
    public InputBotton Attack = new InputBotton(KeyCode.K);
    public InputBotton Shoot = new InputBotton(KeyCode.O);
    public InputBotton Jump = new InputBotton(KeyCode.Space);

    public InputAxis Horizontal = new InputAxis(KeyCode.A, KeyCode.D);
    public InputAxis Vertival = new InputAxis(KeyCode.S, KeyCode.W);

    private void Awake() {
        if (instance != null) {
            throw new System.Exception("Exsting more than one Player_input Instance");
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            Pause.Get();
            Attack.Get();
            Shoot.Get();
            Jump.Get();

            Horizontal.Get();
            Vertival.Get();
        }
        else
        {
            Horizontal.value = 0;
            Vertival.value = 0;
        }
    }
}
