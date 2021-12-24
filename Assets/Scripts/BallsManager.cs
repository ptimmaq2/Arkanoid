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
        //Tarkistetaan, että on vain yksi instanssi. Jos löytyy jo olemassaoleva, se tuhotaan.
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
        if (!GameManager.Instance.IsGameStarted) //Jos peli ei ole alkanut, laitetaan pallo lähelle platformia.
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
        Vector3 platPos = Platform.Instance.gameObject.transform.position; //välivaihe, jolla saadaan koodista vähän siistimpi.
        Vector3 startPos = new Vector3(platPos.x, platPos.y + 0.27f, 0);
        startingBall = Instantiate(ballPreFab, startPos, Quaternion.identity);
        startingBallRb = startingBall.GetComponent<Rigidbody2D>();

        //Lisätään aloituspallo listaan.
        this.balls = new List<Ball>
        {
            startingBall
        };

    }

}
