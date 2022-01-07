using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Perii collectable luokan
public class MultiBall : Collectable
{
    protected override void ApplyEffect()
    {
        //Luodaan jokaiselle pallolle kopio. Jos pelaajalla on 1 pallo kent‰lle
        //tulee yksi lis‰‰, jos kaksi, kaksi lis‰‰ jne.
        foreach (Ball ball in BallsManager.Instance.balls.ToList())
        {
            BallsManager.Instance.SpawnBalls(ball.gameObject.transform.position, 2);
        }
    }
}
