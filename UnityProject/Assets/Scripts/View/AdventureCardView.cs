using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AdventureCardView : CardView {
    private int indexInHand;

    public event EventHandler playCardEvent = delegate { };

    public void SetIndex(int i)
    {
        indexInHand = i;
    }

    public int GetIndex()
    {
        return indexInHand;
    }

    private void Awake()
    {
        spriteRenderer = spriteRendererTemplate.GetComponent<SpriteRenderer>();
        cardBoundry = cardBoundryTemplate.GetComponent<BoxCollider>();
    }

    private void OnMouseDown()
    {
        playCardEvent(this, indexInHand);
    }
}
