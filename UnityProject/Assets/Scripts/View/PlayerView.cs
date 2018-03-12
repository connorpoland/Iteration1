using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour {

    public SpriteRenderer rendererTemplate;
    public AdventureCardView advCardTemplate;
    public Text textTemplate;
    public Vector2 handStart;
    public float handOffset;

    private SpriteRenderer spriteRendererCOA;
    public AdventureCardView[] advCardHand;
    public int advCardHandSize;
    private Text battlePointsText;

    public void SetUpPlayerView()
    {
        spriteRendererCOA = rendererTemplate.GetComponent<SpriteRenderer>();
        Vector2 coaVector = new Vector2(-50, -22);
        spriteRendererCOA.transform.position = coaVector;

		battlePointsText = textTemplate.GetComponent<Text>();


        advCardHand = new AdventureCardView[17];
        for (int c = 0; c < 17; c++)
        {
            advCardHand[c] = Instantiate(advCardTemplate.GetComponent<AdventureCardView>());
            advCardHand[c].ToggleShow(false);
        }  
    }

    public void UpdatePlayerView(Sprite[] hand, Sprite coatOfArms, int numShields)
    {
        advCardHandSize = hand.Length;
        for (int c = 0; c < advCardHandSize; c++)
        {
            float co = handOffset * c;
            Vector2 temp = handStart + new Vector2(co, 0f);
            advCardHand[c].transform.position = temp;
            //CHANGE THIS SO IT ACTUALLY HAS THE RIGHT BACK
            advCardHand[c].SetCard(hand[c], hand[c]);
            advCardHand[c].ToggleShow(true);
            advCardHand[c].ToggleFace(true);
            advCardHand[c].SetIndex(c);
        }
        for (int d = advCardHandSize; d < 17; d++)
        {
            advCardHand[d].ToggleShow(false);
        }
        spriteRendererCOA.sprite = coatOfArms;
		battlePointsText.text = "Current Battle Points: " + numShields.ToString();
    }

    public void ToggleShow(bool showPlayer)
    {
        if(showPlayer)
        {
            for (int c = 0; c < advCardHandSize; c++)
            {
                advCardHand[c].ToggleShow(true);
            }
            spriteRendererCOA.enabled = true;
        }
        else
        {
            for (int c = 0; c < advCardHandSize; c++)
            {
                advCardHand[c].ToggleShow(false);
            }
            spriteRendererCOA.enabled = false;
			battlePointsText.text = null;
        }
    }

    public void HighlightCard(int i)
    {
        advCardHand[i].transform.Translate(Vector3.up * 5.0F);
        advCardHand[i].cardBoundry.enabled = false;
    }
}
