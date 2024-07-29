using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSprite : MonoBehaviour
{
    private void Awake()
    {
        Sword.ChangeSpriteDel = ChangeSprite;
    }

    private void ChangeSprite(int index)
    {
        spriteRenderer.sprite = spArray[index];
    }


    // Member Variable //
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    [SerializeField]
    private Sprite[] spArray;
}
