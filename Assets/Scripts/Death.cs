using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ball"))
        {
            Ball ball = collision.GetComponent<Ball>();
            BallsManager.Instance.balls.Remove(ball);

            ball.Die();
        }
    }

}

