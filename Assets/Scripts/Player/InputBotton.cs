using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBotton
{
    public KeyCode keyCode;
    public bool Down;
    public bool Up;
    public bool Hold;

    public InputBotton (KeyCode keycode) {
        this.keyCode = keycode;
    }

    public void Get() {
        Down = Input.GetKeyDown(this.keyCode);
        Up = Input.GetKeyUp(this.keyCode);
        Hold = Input.GetKey(this.keyCode);
    }
    
}
