using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Pallojen hallinta
public class BallsManager : MonoBehaviour
{

    #region Singleton
    private static BallsManager _instance;
    public static BallsManager Instance => _instance;

    private void Awake()
    {
        //Tarkistetaan, ett� on vain yksi instanssi. Jos l�ytyy jo olemassaoleva, se tuhotaan.
        if (_instance != null)
        {
            Destroy(gameObject);

        }
        else
        {
            _instance = this;
        }
    }


    #endregion
    [SerializeField]
    private Ball ballPreFab;
    private Ball startingBall;
    private Rigidbody2D startingBallRb;

    public float initialSpeed;
    public List<Ball> balls { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        InitializeBall();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted) //Jos peli ei ole alkanut, laitetaan pallo l�helle platformia.
        {
            Vector3 platPos = Platform.Instance.gameObject.transform.position;
            Vector3 ballPos = new Vector3(platPos.x, platPos.y + .27f, 0);

            startingBall.transform.position = ballPos;

            //Pelin aloittaminen.
            if (Input.GetMouseButtonDown(0))
            {
                startingBallRb.isKinematic = false;
                startingBallRb.AddForce(new Vector2(0, initialSpeed));

                GameManager.Instance.IsGameStarted = true; //Asetetaan peli alkaneeksi.
            }
        }

    }

    public void ResetBalls()
    {
        foreach (var ball in this.balls.ToList())
        {
            Destroy(ball.gameObject);
        }

        InitializeBall();
    }


    private void InitializeBall()
    {
        //Haetaan platformin alkusijainti.
        Vector3 platPos = Platform.Instance.gameObject.transform.position; //v�livaihe, jolla saadaan koodista v�h�n siistimpi.
        Vector3 startPos = new Vector3(platPos.x, platPos.y + 0.27f, 0);
        startingBall = Instantiate(ballPreFab, startPos, Quaternion.identity);
        startingBallRb = startingBall.GetComponent<Rigidbody2D>();

        //Lis�t��n aloituspallo listaan.
        this.balls = new List<Ball>
        {
            startingBall
        };

    }

    public void SpawnBalls(Vector3 position, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Ball spawnedBall = Instantiate(ballPreFab, position, Quaternion.identity) as Ball;
            Rigidbody2D spawnedBallRb = spawnedBall.GetComponent<Rigidbody2D>();
            spawnedBallRb.isKinematic = false;
            spawnedBallRb.AddForce(new Vector2(0, initialSpeed));
            this.balls.Add(spawnedBall); //Lis�t��n luotu pallo pallolistaan.
        }
    }

    public void ChangeSize(Vector3 vector3, float time)
    {
        StartCoroutine(ChangingSize(vector3, time));

    }

    //K�ytet��n pallon kasvatusbuffiin sek� kutistus debuffiin.
    public IEnumerator ChangingSize(Vector3 vector3, float time)
    {

        foreach (var ball in this.balls.ToList())
        {
            if (ball != null)
            {
                ball.transform.localScale = vector3;
                yield return new WaitForSeconds(time);
            }

        }

        foreach (var ball in this.balls.ToList())
        {

            if (ball != null)
            {
                ball.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
        }
    }
    public void ChangeSpeed(float ballSpeed, bool isSpeed, float time)
    {
        StartCoroutine(ChangingSpeed(ballSpeed, isSpeed, time));
    }

    public IEnumerator ChangingSpeed(float ballSpeed, bool isSpeed, float time)
    {
        //Tarkistetaan aluksi ett� pallolla ei ole jo nopeusbuffia tai hitausdebuffia,
        //etteiv�t ne stackkaa ja pallo saa j�rjett�m�n suurta tai negatiivista nopeutta.
        if (initialSpeed == 250)
        {
            //Tarkistetaan onko nopeus jo k�yt�ss�, ei voi saada monta kertaa p��llekk�in.
            if (isSpeed == true)
            {
                foreach (var ball in this.balls.ToList())
                {
                    if (ball != null)
                    {
                        initialSpeed = initialSpeed + ballSpeed;
                        yield return new WaitForSeconds(time);
                    }
                }
                //Pilkoin foreachit erikseen, koska muuten yritet��n muuttaa my�s tuhoutuneiden pallojen arvoja.
                foreach (var ball in this.balls.ToList())
                {
                    if (ball != null)
                    {
                        //initialSpeed = initialSpeed - ballSpeed;
                        initialSpeed = 250; //joutui kovakoodaamaan muuten menee v�lill� jostain syyst� negatiiviseksi...
                    }
                }
            }
            //Pallon hidastaminen debuffi.
            if (isSpeed == false)
            {
                foreach (var ball in this.balls.ToList())
                {
                    if (ball != null)
                    {
                        //Palloa hidastetaan
                        initialSpeed = initialSpeed - ballSpeed;
                        yield return new WaitForSeconds(time);
                    }
                }
                foreach (var ball in this.balls.ToList())
                {
                    if (ball != null)
                    {
                        initialSpeed = initialSpeed + ballSpeed;
                    }
                }
            }
        }
    }
}
