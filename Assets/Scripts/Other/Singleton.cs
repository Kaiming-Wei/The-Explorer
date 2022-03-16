using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton <T>
{
    public static T _instance;

    public static T Instance {
        get {
            if (_instance == null) {
                _instance = System.Activator.CreateInstance<T>();
            }
            return _instance;
        }
    }
}
