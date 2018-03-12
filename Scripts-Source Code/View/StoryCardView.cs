using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoryCardView : CardView {
    public event EventHandler doStoryCardEvent = delegate { };
	public bool viewEnabled;

    private void Awake()
    {
        spriteRenderer = spriteRendererTemplate.GetComponent<SpriteRenderer>();
        cardBoundry = cardBoundryTemplate.GetComponent<BoxCollider>();
		viewEnabled = true;
    }

    private void OnMouseDown()
    {
		if (viewEnabled) {
			spriteRenderer.sprite = cardFace;
			doStoryCardEvent (this, 0);
		}
    }
}
