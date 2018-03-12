using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour {

    public Sprite[] coatOfArms;
    protected int coaIndex;

	public Sprite[] ranks;
	protected int currentRank;

	protected int battlePoints;

	protected CardModel[] weapons;

    protected CardModel[] hand;
    protected int cardsInHand;
    public int shields;

    private void Awake()
    {
        hand = new CardModel[17];
		weapons = new CardModel[6];
        cardsInHand = 0;
        shields = 0;
        coaIndex = 0;
		currentRank = 0;
		battlePoints = 5;
    }

    public void drawCard(CardModel newCard)
    {
        hand[cardsInHand] = newCard;
        cardsInHand++;
    }

    public CardModel Discard(int index)
    {
        CardModel temp = hand[index];
        for(int i = 0; i < cardsInHand; i++)
        {
            if(i > index && i < cardsInHand)
            {
                hand[i-1] = hand[i];
            }
        }
        cardsInHand--;
        return temp;
    }

    public void gainShields(int numShields)
    {
        shields = shields + numShields;
		if (currentRank == 0 && shields >= 5) {
			shields = shields - 5;
			currentRank++;
			battlePoints = 10;
		}
		if (currentRank == 1 && shields >= 10) {
			shields = shields - 10;
			currentRank++;
			battlePoints = 20;
		}
    }

    public int getShields()
    {
        return shields;
    }

    public int GetCardsInHand()
    {
        return cardsInHand;
    }

    public Sprite getCoatOfArms()
    {
        return coatOfArms[coaIndex];
    }

    public void setCoaIndex(int newIndex)
    {
        if (newIndex > 0 && newIndex < coatOfArms.Length)
        {
            coaIndex = newIndex;
        }
    }

    public Sprite[] getHand()
    {
        Sprite[] handSprites = new Sprite[cardsInHand];
        for (int c = 0; c < cardsInHand; c++)
        {
            handSprites[c] = hand[c].getFace();
        }
        return handSprites;
    }

	public int GetBattlePoints()
	{
		int weaponTotal = 0;
		for (int i = 0; i < 6; i++) {
			if (weapons [i] != null) {
				weaponTotal = weaponTotal + weapons [i].battlePoints;
			}
		}
		return battlePoints + weaponTotal;
	}

	public Sprite GetRank()
	{
		return ranks [currentRank];
	}

	public string AddWeapon(int index)
	{
		if (hand[index].name == "Dagger" && weapons[0] == null) {
			weapons [0] = Discard(index);
			return "Dagger";
		}
		else if (hand[index].name == "Horse" && weapons[1] == null) {
			weapons [1] = Discard(index);
			return "Horse";
		}
		else if (hand[index].name == "Sword" && weapons[2] == null) {
			weapons [2] = Discard(index);
			return "Sword";
		}
		else if (hand[index].name == "Lance" && weapons[3] == null) {
			weapons [3] = Discard(index);
			return "Lance";
		}
		else if (hand[index].name == "Battle-ax" && weapons[4] == null) {
			weapons [4] = Discard(index);
			return "Battle-ax";
		}
		else if (hand[index].name == "Excalibur"  && weapons[5] == null) {
			weapons [5] = Discard(index);
			return "Excalibur";
		}
		return "Invalid";
	}

	public CardModel[] DiscardWeapons(){
		CardModel[] temp = weapons;
		weapons = new CardModel[6];
		return temp;
	}

	public CardModel GetCard(int i)
	{
		return hand [i];
	}

	public int GetRankNum()
	{
		return currentRank;
	}
}
