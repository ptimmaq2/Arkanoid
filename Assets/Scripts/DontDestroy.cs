using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        if (GameObject.Find("bg-music") != null)
        {


        }
        else
        {
            DontDestroyOnLoad(this); 
        }
    }
}