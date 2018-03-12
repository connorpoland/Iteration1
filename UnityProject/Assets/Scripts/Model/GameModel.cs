using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : MonoBehaviour {

    public Sprite background;
	public bool fast;

    public QuestCardModel questCardTemplate;
    public EventCardModel eventCardTemplate;
    public CardModel adventureCardTemplate;
    public PlayerModel playerTemplate;

    private Stack<CardModel> storyDeck;
    private Stack<CardModel> storyDiscard;
    private Stack<CardModel> adventureDeck;
    private Stack<CardModel> adventureDiscard;

    private PlayerModel[] allPlayers;
    private int currentPlayer;
    public int numberOfPlayers;

    //a list of indexes into the current players hand - may or may not be played later
    private int[] pendingCards;
    private int numPendingCards;

    public CardModel currentStoryCard;

    private void Awake()
    {
        currentPlayer = 0;
		fast = false;
    }

    public void CreateStoryDeck()
    {
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\StoryDeckLog.txt", true))
			file.WriteLine ("Story Deck Distrobution - Log");
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\StoryDeckLog.txt", true))
			file.WriteLine ("----------------------------------");
        storyDeck = new Stack<CardModel>();
        storyDiscard = new Stack<CardModel>();
        for (int i = 0; i < 18; i++)
        {
			if ((i%3) + 1 == 2 && (!fast || i != 16))
            {
                EventCardModel newCard = Instantiate(eventCardTemplate.GetComponent<EventCardModel>());
                newCard.Construct("Prosperity Throughout the Realm", 0, 1);
                storyDeck.Push(newCard);
				using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\StoryDeckLog.txt", true))
					file.WriteLine (i+": "+newCard.name);
            }
			else if ((i%3) + 1 == 1 && (!fast || i != 16))
            {
                EventCardModel newCard = Instantiate(eventCardTemplate.GetComponent<EventCardModel>());
                newCard.Construct("Chivalrous Deed", 1, 1);
                storyDeck.Push(newCard);
				using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\StoryDeckLog.txt", true))
					file.WriteLine (i+": "+newCard.name);
            }
			else if ((i % 3) + 1 == 3 || (fast && i == 16))
            {
                QuestCardModel newCard = Instantiate(questCardTemplate.GetComponent<QuestCardModel>());
                newCard.Construct("Boar Hunt", 0, 2, new string[]{"Boar"});
                storyDeck.Push(newCard);
				using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\StoryDeckLog.txt", true))
					file.WriteLine (i+": "+newCard.name);
            }
        }
    }

    public void CreateAdventureDeck()
    {
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\AdventureDeckLog.txt", true))
			file.WriteLine ("Adventure Deck Distrobution - Log");
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\AdventureDeckLog.txt", true))
			file.WriteLine ("----------------------------------");
        adventureDeck = new Stack<CardModel>();
        adventureDiscard = new Stack<CardModel>();
        for (int i = 0; i < 70; i++)
        {
			CardModel temp = Instantiate (adventureCardTemplate.GetComponent<CardModel> ());
			if ((i%14) + 1 == 15){
				temp.name = "Dragon";
				temp.faceIndex = 7;
				temp.battlePoints = 50;
				temp.battlePoints = 70;
			}
			else if((i%14) + 1 == 14){
				temp.name = "Thieves";
				temp.faceIndex = 13;
				temp.battlePoints = 5;
			}
			else if((i%14) + 1 == 13){
				temp.name = "Saxons";
				temp.faceIndex = 12;
				temp.battlePoints = 10;
				temp.specialBattlePoints = 20;
			}
			else if((i%14) + 1 == 12){
				temp.name = "Robber Knight";
				temp.faceIndex = 11;
				temp.battlePoints = 15;
			}
			else if((i%14) + 1 == 11){
				temp.name = "Mordred";
				temp.faceIndex = 10;
				temp.battlePoints = 30;
			}
			else if((i%14) + 1 == 10){
				temp.name = "Green Knight";
				temp.faceIndex = 9;
				temp.battlePoints = 25;
				temp.specialBattlePoints = 40;
			}
			else if((i%14) + 1 == 9){
				temp.name = "Giant";
				temp.faceIndex = 8;
				temp.battlePoints = 40;
			}
			else if((i%14) + 1 == 8){
				temp.name = "Excalibur";
				temp.faceIndex = 6;
				temp.battlePoints = 30;
			}
			else if((i%14) + 1 == 7){
				temp.name = "Boar";
				temp.faceIndex = 0;
				temp.battlePoints = 5;
				temp.specialBattlePoints = 15;
			}
			else if((i%14) + 1 == 6){
				temp.name = "Dagger";
				temp.faceIndex = 1;
				temp.battlePoints = 5;
			}
			else if((i%14) + 1 == 5){
				temp.name = "Horse";
				temp.faceIndex = 2;
				temp.battlePoints = 10;
			}
			else if((i%14) + 1 == 4){
				temp.name = "Battle-ax";
				temp.faceIndex = 3;
				temp.battlePoints = 15;
			}
			else if((i%14) + 1 == 3){
				temp.name = "Sword";
				temp.faceIndex = 4;
				temp.battlePoints = 10;
			}
			else if((i%14) + 1 == 2){
				temp.name = "Lance";
				temp.faceIndex = 5;
				temp.battlePoints = 20;
			}
			else if((i%14) + 1 == 1){
				temp.name = "Excalibur";
				temp.faceIndex = 6;
				temp.battlePoints = 30;
			}

            adventureDeck.Push(temp);
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\AdventureDeckLog.txt", true))
				file.WriteLine (i+": "+temp.name);
        }
		if (!fast) {
			Shuffle (adventureDeck);
		}
    }

    public void CreateAllPlayers()
    {
        allPlayers = new PlayerModel[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            PlayerModel temp = Instantiate(playerTemplate.GetComponent<PlayerModel>());
            temp.setCoaIndex(i);
            allPlayers[i] = temp;
        }
    }

    public void DealAdventureCard(int player)
    {
		if (adventureDeck.Count == 0) {
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
				file.WriteLine ("-- MODEL : adventure discard pile shuffled into adventure deck");
			Shuffle (adventureDiscard);
			Stack<CardModel> temp = adventureDiscard;
			adventureDiscard = adventureDeck;
			adventureDeck = temp;
		}
		CardModel tempCard = adventureDeck.Pop ();
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
			file.WriteLine ("-- MODEL : player "+player+" drew "+tempCard.name);
        allPlayers[player].drawCard(tempCard);
    }

    public void GainShields(int player, int numShields)
    {
		int temp = allPlayers [player].GetRankNum ();
        allPlayers[player].gainShields(numShields);
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
			file.WriteLine ("-- MODEL : player "+player+" gained "+numShields+ " shields");
		if (temp != allPlayers [player].GetRankNum ()) {
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
				file.WriteLine ("-- MODEL : player "+player+" evolved to rank "+allPlayers [player].GetRankNum ());
		}
    }

    public int GetNumberOfPlayers()
    {
        return numberOfPlayers;
    }

    public void DrawStoryCard()
    {
		if (storyDeck.Count == 0) {
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
				file.WriteLine ("-- MODEL : story discard pile shuffled into story deck");
			Shuffle (storyDiscard);
			Stack<CardModel> temp = storyDeck;
			storyDeck = storyDiscard;
			storyDiscard = temp;
		}
        if(currentStoryCard == null)
        {
            currentStoryCard = storyDeck.Pop();
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
				file.WriteLine ("-- MODEL : "+currentStoryCard.name+" has been drawn");
        }
        else
        {
			CardModel[] temp = currentStoryCard.CleanUp ();
			if (temp != null) {
				for (int i = 0; i < temp.Length; i++) {
					adventureDiscard.Push (temp [i]);
				}
			}
            storyDiscard.Push(currentStoryCard);
            currentStoryCard = storyDeck.Pop();
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
				file.WriteLine ("-- MODEL : "+currentStoryCard.name+" has been drawn");
        }
    }

    public CardModel GetCurrentStoryCard()
    {
        return currentStoryCard;
    }

    public void NextPlayer()
    {
        if (IsStoryComplete())
        {
            if (currentPlayer == allPlayers.Length - 1)
            {
                currentPlayer = 0;
            }
            else
            {
                currentPlayer++;
            }
        }
        else
        {
            currentStoryCard.NextPlayer();
        }
    }

    public int GetCurrentPlayer()
    {
        if (currentStoryCard && !(currentStoryCard.complete))
        {
            return currentStoryCard.GetCurrentEngagedPlayer();
        }
        return currentPlayer;
    }

    public Sprite[] GetPlayerHand(int player)
    {
        return allPlayers[player].getHand();
    }

    public Sprite[] GetEngagedHand()
    {
        return allPlayers[GetEngagedPlayer()].getHand();
    }

    public Sprite GetPlayerCOA(int player)
    {
        return allPlayers[player].getCoatOfArms();
    }

    public int GetPlayerCardsInHand(int player)
    {
        return allPlayers[player].GetCardsInHand();
    }

    public int GetPlayerShields(int player)
    {
        return allPlayers[player].getShields();
    }

    public int CheckWin()
    {
        for (int i = 0; i < allPlayers.Length; i++)
        {
			if ((fast && allPlayers[i].GetRankNum() > 0) || (!fast && allPlayers[i].getShields() >= 15)){
				using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
					file.WriteLine ("-- MODEL : Player "+i+" has won");
				return i;
			}
        }
        return -1;
    }

    public void PlayerDiscardsCard(int player, int card)
    {
		CardModel temp = allPlayers[player].Discard(card);
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
			file.WriteLine ("-- MODEL : Player "+player+" has discarded "+temp.name);
        adventureDiscard.Push(temp);
    }

	public int GetPlayerBattlePoints(int player)
	{
		return allPlayers [player].GetBattlePoints ();
	}

	public Sprite GetPlayerRank(int player)
	{
		return allPlayers [player].GetRank ();
	}

    public void PerformStage()
    {
        currentStoryCard.DoStage(this);
    }

    public bool IsStoryComplete()
    {
        return currentStoryCard.complete;
    }

    public int GetEngagedPlayer()
    {
        return currentStoryCard.GetCurrentEngagedPlayer();
    }

    public string GetStoryMessage()
    {
        return currentStoryCard.message;
    }

	public bool ConfirmPendingCards(int h)
    {
        CardModel[] cards = new CardModel[numPendingCards];
		for (int x = 0; x < numPendingCards; x++) {
			cards [x] = allPlayers [GetEngagedPlayer ()].GetCard (pendingCards [x]);
		}
		if (currentStoryCard.UpdateStory (cards)) {
			for (int i = 0; i < numPendingCards; i++)
			{
				//this agjusts the indexs of the cards because they will change 
				//as they are discarded 1 by 1
				for (int n = i+1; n < numPendingCards; n++)
				{
					if(pendingCards[n] > pendingCards[i])
					{
						pendingCards[n]--;
					}
				}
				//this potentially changes the index of cards to the right of it 
				allPlayers[h].Discard(pendingCards[i]);
			}
			pendingCards = new int[12];
			numPendingCards = 0;
			return true;
		} else {
			pendingCards = new int[12];
			numPendingCards = 0;
			return false;
		}
    }

    public void ResetPendingCards()
    {
        pendingCards = new int[12];
        numPendingCards = 0;
    }

    public void AddPendingCard(int i)
    {
        pendingCards[numPendingCards] = i;
        numPendingCards++;
    }

    public string GetStoryContext()
    {
        return currentStoryCard.GetContext();
    }

    public void JoinStoryStage()
    {
        currentStoryCard.JoinStory();
    }

    public void DeclineStoryStage()
    {
        currentStoryCard.DeclineStory();
    }

	public bool FightStory()
	{
		//discard what you need to
		return currentStoryCard.FightStory(allPlayers[GetCurrentPlayer()]);
	}

    public bool CheckCurrentPlayer()
    {
        Debug.Log("currentPlayer: " + currentPlayer + " GetCurrent: " + GetCurrentPlayer());
        if (currentPlayer == GetCurrentPlayer())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

	public Sprite[] GetStoryStage()
	{
		return currentStoryCard.GetStage ();
	}

	public void DiscardWeapons()
	{
		CardModel[] temp;
		for (int i = 0; i < numberOfPlayers; i++) {
			temp = allPlayers [i].DiscardWeapons ();
			for (int c = 0; c < 6; c++) {
				if (temp [c] != null) {
					adventureDiscard.Push (temp [c]);
				}
			}
		}
	}

	public string PlayCard(int player, int card)
	{
		return allPlayers [player].AddWeapon (card);
	}

	public void CollectReward (int p){
		currentStoryCard.CollectReward(this, p);
	}

	public int GetPlayerRankNum(int player){
		return allPlayers [player].GetRankNum ();
	}

	public void Shuffle(Stack<CardModel> deck){
		System.Random rnd = new System.Random ();
		CardModel[] temp = deck.ToArray ();
		for (int i = 0; i < temp.Length; i++) {
			Swap (temp, i, rnd.Next (i, temp.Length));
		}
	}

	public void Swap(CardModel[] deck, int x, int y){
		CardModel temp = deck [x];
		deck [x] = deck [y];
		deck [y] = temp;
	}
}


