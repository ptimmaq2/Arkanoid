using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//S‰‰t‰‰ pelaajan kokoa
public class ExtendAndShrink : Collectable
{
    public float newWidth = 2.5f;

    protected override void ApplyEffect()
    {
        if (Platform.Instance != null && !Platform.Instance.IsTransforming)
        {
            Platform.Instance.StartWidthAnimation(newWidth);

        }   
    }
}
