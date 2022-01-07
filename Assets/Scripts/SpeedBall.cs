using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpeedBall : Collectable
{
    public float ballSpeed;
    public bool isSpeed;
    public float time;
    protected override void ApplyEffect()
    {
        BallsManager.Instance.ChangeSpeed(ballSpeed, isSpeed, time);
    }
}
