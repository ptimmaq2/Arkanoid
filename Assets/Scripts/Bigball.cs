using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bigball : Collectable
{
    public Vector3 BigBallSize;
    public float time;
    protected override void ApplyEffect()
    {

            BallsManager.Instance.ChangeSize(BigBallSize, time);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
