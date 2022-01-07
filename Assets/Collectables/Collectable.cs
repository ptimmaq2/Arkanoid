using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Tarkistetaan osutaanko "pelaajaan" ja k‰ytet‰‰n powerup.
        if(collision.CompareTag("Platform"))
        {
            this.ApplyEffect();
        }

        //Tuhotaan poweruppi, jos se osuu pelaajaan tai kuolemaan.
        if(collision.CompareTag("Platform") || collision.CompareTag("DeathWall")){
            Destroy(this.gameObject);
        }

    }

    protected abstract void ApplyEffect();
}
