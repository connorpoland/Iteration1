using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDView : MonoBehaviour {

    public SpriteRenderer rendererTemplate;
    public Text textTemplate;
    public Vector2 coaStart;
    public Vector2 textStart;
	public Vector2 rankStart;
    public float coaOffset;
    public float textOffset;
	public float rankOffset;
    public float textSpacing;
	public Vector3 rankSize;
    public Canvas canvasParent;

    private SpriteRenderer[] spriteRendererCOAs;
    private Text[] shieldsTexts;
    private Text[] cardsTexts;
	private SpriteRenderer[] spriteRendererRanks;
    private Canvas canvas;

    public void SetUpHUDView(int numPlayers)
    {
        canvas = canvasParent.GetComponent<Canvas>();
        SpriteRenderer tempRenderer = Instantiate(rendererTemplate.GetComponent<SpriteRenderer>());
        Text tempText = Instantiate(textTemplate.GetComponent<Text>());
        spriteRendererCOAs = new SpriteRenderer[numPlayers];
		spriteRendererRanks = new SpriteRenderer[numPlayers];
        shieldsTexts = new Text[numPlayers];
        cardsTexts = new Text[numPlayers];
        for (int p = 0; p < numPlayers; p++)
        {
            float co = coaOffset * p;
            float to = textOffset * p;
			float ro = rankOffset * p;
            Vector2 temp = coaStart + new Vector2(0f, co);

            spriteRendererCOAs[p] = Instantiate(tempRenderer);
            spriteRendererCOAs[p].transform.position = temp;

			temp = rankStart + new Vector2 (0f, ro);
			spriteRendererRanks[p] = Instantiate(tempRenderer);
			spriteRendererRanks [p].transform.position = temp;
			spriteRendererRanks [p].transform.localScale = rankSize;

            temp = textStart + new Vector2(textSpacing, to);
            shieldsTexts[p] = Instantiate(tempText);
            shieldsTexts[p].transform.position = temp;
            shieldsTexts[p].transform.SetParent(canvas.transform, false);

            temp = textStart + new Vector2(2*textSpacing, to);
            cardsTexts[p] = Instantiate(tempText);
            cardsTexts[p].transform.position = temp;
            cardsTexts[p].transform.SetParent(canvas.transform, false);
        }
    }

	public void UpdateHUDView(Sprite[] coa, int[] numCards, int[] numShields, Sprite[] ranks)
    {
        for (int p = 0; p < coa.Length; p++)
        {
            spriteRendererCOAs[p].sprite = coa[p];
            spriteRendererCOAs[p].enabled = true;
            shieldsTexts[p].text = "Shields: " + numShields[p].ToString();
            cardsTexts[p].text = "Cards: " + numCards[p].ToString();
			spriteRendererRanks [p].sprite = ranks [p];
			spriteRendererRanks[p].enabled = true;
        }
    }

    public void ToggleShow(bool showHUD)
    {
        if (showHUD)
        {
            for (int p = 0; p < spriteRendererCOAs.Length; p++)
            {
                spriteRendererCOAs[p].enabled = true;
            }
        }
        else
        {
            for (int p = 0; p < spriteRendererCOAs.Length; p++)
            {
                spriteRendererCOAs[p].enabled = false;
                shieldsTexts[p].text = "";
                cardsTexts[p].text = "";
            }
        }
    }
}
