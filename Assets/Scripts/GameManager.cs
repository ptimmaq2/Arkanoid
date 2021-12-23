using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        //Tarkistetaan, ett� on vain yksi instanssi. Jos l�ytyy jo olemassaoleva, se tuhotaan.
        if(_instance != null)
        {
            Destroy(gameObject);

        }
        else
        {
            _instance = this;
        }
    }



    #endregion

    public bool isGameStarted { get; set; } 
}
