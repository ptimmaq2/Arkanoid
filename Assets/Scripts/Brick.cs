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
    private AudioSource juu;

    public static event Action<Brick> OnBrickDestruction;

    private void Awake()
    {
        Camera cam = Camera.main;
        juu = cam.GetComponent<AudioSource>();
       this.sr = this.GetComponent<SpriteRenderer>();
      //  this.sr.sprite = BricksManager.Instance.Sprites[this.hitpoints - 1]; 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball =  collision.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);
        DestroySound();
    }

    private void ApplyCollisionLogic(Ball ball)
    {
        //tuhotaan tiili ja poistetaan se remainingbricksistä.

        this.hitpoints--;
        if (this.hitpoints <= 0) 
        {
            BricksManager.Instance.remainingBricks.Remove(this);
            OnBrickDestruction?.Invoke(this);
            AfterDestructionSpawnBuffs();
            SpawnDestroyEffect();
           
           Destroy(this.gameObject);
        }
        //vaihdetaan tiilen spriteä.
        else
        {
            this.sr.sprite = BricksManager.Instance.Sprites[this.hitpoints - 1];
        }
    }

    private void DestroySound()
    {
       juu.Play(0);
    }

    private void AfterDestructionSpawnBuffs()
    {
        float buffSpawnChance = UnityEngine.Random.Range(0, 100f);
        float debuffSpawnChance = UnityEngine.Random.Range(0, 100f);
        bool alreadySpawned = false;

        if(buffSpawnChance <= CollectablesManager.Instance.buffChance)
        {
            alreadySpawned = true;
            Collectable newBuff = this.spawnCollectable(true);
        }
        if(debuffSpawnChance <= CollectablesManager.Instance.debuffChance && !alreadySpawned)
        {
            Collectable newDeBuff = this.spawnCollectable(false);
        }
    }

    private Collectable spawnCollectable(bool isBuff)
    {
        List<Collectable> collection;
        if (isBuff)
        {
            collection = CollectablesManager.Instance.AvailableBuffs;
        }
        else
        {
            collection = CollectablesManager.Instance.AvailableDebuffs;
        }
        //haetaan satunnainen buffi/debuffi listasta.
        int buffIndex = UnityEngine.Random.Range(0, collection.Count);
        Collectable prefab = collection[buffIndex];
        Collectable newCollectable = Instantiate(prefab, this.transform.position, Quaternion.identity) as Collectable;
        
        return newCollectable;
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
