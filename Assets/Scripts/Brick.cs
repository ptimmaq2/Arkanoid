using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour

{
    private SpriteRenderer sr;

    public int hitpoints = 1;
    public ParticleSystem destroyEffect;

    public static event Action<Brick> OnBrickDestruction;

    private void Awake()
    {
       this.sr = this.GetComponent<SpriteRenderer>();
      //  this.sr.sprite = BricksManager.Instance.Sprites[this.hitpoints - 1]; 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball =  collision.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);
    }

    private void ApplyCollisionLogic(Ball ball)
    {
        //tuhotaan tiili.
        this.hitpoints--;
        if (this.hitpoints <= 0) 
        {
            OnBrickDestruction?.Invoke(this);
            SpawnDestroyEffect();
           Destroy(this.gameObject);
        }
        //vaihdetaan tiilen spriteä.
        else
        {
            this.sr.sprite = BricksManager.Instance.Sprites[this.hitpoints - 1];
        }
    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPos = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        GameObject effect = Instantiate(destroyEffect.gameObject, spawnPos, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;

        Destroy(effect, destroyEffect.main.startLifetime.constant);

            
    }

    public void Init(Transform containerTransform, Sprite sprite, Color color, int hitpoints)
    {
        this.transform.SetParent(containerTransform);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.hitpoints = hitpoints;
    }
}
