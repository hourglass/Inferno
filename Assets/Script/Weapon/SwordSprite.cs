using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSprite : MonoBehaviour
{
    void Awake()
    {
        Sword.ChangeSpriteDel = ChangeSprite;
    }

    void ChangeSprite(int index)
    {
        spriteRenderer.sprite = spArray[index];
    }


    //Member Variable//
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] spArray;
}
