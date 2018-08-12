using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T ms_instance;

    public static T Instance
    {
        get
        {
            ms_instance = GameObject.FindObjectOfType<T>();

            if (ms_instance == null)
            {
                GameObject go = new GameObject("Singleton " + typeof(T).ToString());
                ms_instance = go.AddComponent<T>();
            }

            return ms_instance;
        }
    }
}
