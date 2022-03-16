using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data {

}

public class Data<T> : Data {
    public T value1;
}

public class Data<T1, T2> : Data {
    public T1 value1;
    public T2 value2;
}

public class Data<T1, T2, T3> : Data {
    public T1 value1;
    public T2 value2;
    public T3 value3;
}

public class DataManager : Singleton<DataManager> 
{
    Dictionary<string, Data> datas = new Dictionary<string, Data>();

    public void SaveData (string key, Data value) {
        datas[key] = value;
    }

    public Data GetData(string key) {
        if (datas.ContainsKey(key)) {
            return datas[key];
        }
        return null;
    }
}
