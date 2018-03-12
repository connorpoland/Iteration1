using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CardView : MonoBehaviour {

    public delegate void EventHandler(object sender, int i);

    public SpriteRenderer spriteRendererTemplate;
    public SpriteRenderer spriteRenderer;
    public Sprite cardFace;
    public Sprite cardBack;
    public BoxCollider cardBoundryTemplate;
    public BoxCollider cardBoundry;


    private void Awake()
    {
    }

    public void SetCard(Sprite face, Sprite back)
    {
        cardFace = face;
        cardBack = back;
    }

    public void ToggleFace(bool showFace)
    {
        if (showFace)
        {
            spriteRenderer.sprite = cardFace;
        }
        else
        {
            spriteRenderer.sprite = cardBack;
        }
    }

    public void ToggleShow(bool showCard)
    {
        spriteRenderer.enabled = showCard;
        cardBoundry.enabled = showCard;
    }
}
