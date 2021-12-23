using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Alustan hallinta
public class Platform : MonoBehaviour
{

    #region Singleton
    private static Platform _instance;
    public static Platform Instance => _instance;

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

    public float defLeftClamp;
    public float defRightClamp;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        platInitialY = this.transform.position.y;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        PlatformMovement();
    }

    void PlatformMovement()
    {

        float sizeChange = (defaultWidth - ((defaultWidth / 2) * this.spriteRenderer.size.x)) / 2;
        //Arvot muutetaan dynaamisiksi myöhemmin.
        float leftClamp = defLeftClamp - sizeChange;
        float rightClamp = defRightClamp - sizeChange;

        float mousePosPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        float mousePosX = mainCamera.ScreenToWorldPoint(new Vector3(mousePosPixels, 0, 0)).x;

        this.transform.position = new Vector3(mousePosX, platInitialY, 0);
    }
    //Pallon kääntyminen.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
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
