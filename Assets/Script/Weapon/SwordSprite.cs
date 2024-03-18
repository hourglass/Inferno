using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField] Sprite[] spArray = null;

    void Start()
    {
        Sword.ChangeSpriteDel = ChangeSprite;
    }

    void ChangeSprite(int index)
    {
        spriteRenderer.sprite = spArray[index];
    }
}
