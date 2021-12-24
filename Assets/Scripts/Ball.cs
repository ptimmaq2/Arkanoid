using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pallon hallinta
public class Ball : MonoBehaviour
{
    public static Action<Ball> onBallDeath;
    public void Die()
    {
        onBallDeath?.Invoke(this);
        Destroy(gameObject, 1);
    }
}
