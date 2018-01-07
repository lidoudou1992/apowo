using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour
{
    //void Start()
    //{

    //}

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
