using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Alustan hallinta
public class Platform : MonoBehaviour
{

    #region Singleton
    private static Platform _instance;
    public static Platform Instance => _instance;

    public bool IsTransforming { get; set; }

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

    private Camera mainCamera;

    private float platInitialY;
    public float defaultWidth;

    public GameObject gameOveri;
    public GameObject victory;
    public GameObject pauseScreen;

    public float defLeftClamp;
    public float defRightClamp;

    public bool platformIsTransforming { get; set; }

    public float extendShrinkDuration = 10;
    public float platformWidth = 2;
    public float platformHeight = 0.28f; //oletusarvoja, voidaan muuttaa editorissa.

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D bc;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        platInitialY = this.transform.position.y;
        spriteRenderer = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlatformMovement();
    }

    public void StartWidthAnimation(float newWidth)
    {
        StartCoroutine(AnimatePlatformWidth(newWidth));
    }

    public IEnumerator AnimatePlatformWidth(float width)
    {
        this.platformIsTransforming = true;
        this.StartCoroutine(ResetPlatformWidth(this.extendShrinkDuration));

        if (width > this.spriteRenderer.size.x)
        {
            //Kasvatetaan kokoa hitaasti, näyttää nätimmältä animoituna.
            float currentWidth = this.spriteRenderer.size.x;
            while (currentWidth < width)
            {
                currentWidth += Time.deltaTime * 2;
                this.spriteRenderer.size = new Vector2(currentWidth, platformHeight);
                //muutetaan myös boxcolliderin kokoa.
                bc.size = new Vector2(currentWidth, platformHeight);
                yield return null;
            }
        }
        else
        {
            //pienennys, sama periaate.
            float currentWidth = this.spriteRenderer.size.x;
            while(currentWidth > width)
            {
                currentWidth -= Time.deltaTime * 2;
                this.spriteRenderer.size = new Vector2(currentWidth, platformHeight);
                //muutetaan myös boxcolliderin kokoa.
                bc.size = new Vector2(currentWidth, platformHeight);
                yield return null;
            }
        }

        this.platformIsTransforming = false; 
    }

    private IEnumerator ResetPlatformWidth(float s)
    {
        yield return new WaitForSeconds(s);
        this.StartWidthAnimation(this.platformWidth);
    }

    void PlatformMovement()
    {
        //Tarkistetaan jos gameover screeni tai victory on päällä.
        if (!gameOveri.activeInHierarchy && !victory.activeInHierarchy && !pauseScreen.activeInHierarchy)
        {
            float sizeChange = (defaultWidth - ((defaultWidth / 2) * this.spriteRenderer.size.x)) / 2;
            //Arvot muutetaan dynaamisiksi myöhemmin.
            float leftClamp = defLeftClamp - sizeChange;
            float rightClamp = defRightClamp - sizeChange;

            float mousePosPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
            float mousePosX = mainCamera.ScreenToWorldPoint(new Vector3(mousePosPixels, 0, 0)).x;

            this.transform.position = new Vector3(mousePosX, platInitialY, 0);
        }
    }
    //Pallon kääntyminen.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 platformCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            ballRb.velocity = Vector2.zero;
            float difference = platformCenter.x - hitPoint.x; //matka keskiosan ja osuman välillä.

            if (hitPoint.x < platformCenter.x)
            {
                ballRb.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), BallsManager.Instance.initialSpeed));
            }
            else
            {
                ballRb.AddForce(new Vector2((Mathf.Abs(difference * 200)), BallsManager.Instance.initialSpeed));
            }
        }
    }
}
