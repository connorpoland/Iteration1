using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour {

    private GameModel gameModel;
    private GameView gameView;
    //When a player presses a card, the current context will inform what happens to that card
    //on the fence about making this a part of the model
    private enum context {Discard, Play, AddToStory, None};
    private context playCardContext = context.Discard;
	private context recoverContext = context.None;
	private int hotSeat;

    public GameModel gameTemplate;
    public GameView viewTemplate;

    private void Awake()
    {
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
		file.WriteLine ("Quest of the Round Table - Log");
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
			file.WriteLine ("-----------------------------");
        gameView = viewTemplate.GetComponent<GameView>();
        //This call will draw the buttons we need
        gameView.SetUpView();
        //TODO Listeners for UI Buttons
        gameView.startGameEvent4 += (sender, args) => { Construct(4, false); };
        gameView.startTurnEvent += (sender, args) => { StartTurn(args); };
        gameView.doStoryCardEvent += (sender, args) => { RevealStoryCard(); };
        gameView.endTurnEvent += (sender, args) => { SeatChange(args); };
        //for playing sets of cards
        gameView.confirmEvent += (sender, args) => { ConfirmPendingCards(); };
        //for playering sets of cards
        gameView.resetEvent += (sender, args) => { ResetPendingCards();  };
        //for leaving the current stage
        gameView.declineEvent += (sender, args) => { DeclineStoryStage(); };
        //for joining the current stage
        gameView.joinEvent += (sender, args) => { JoinStoryStage(); };
        //for continuing sequence after card is drawn
        gameView.continueEvent += (sender, args) => { DoStory(); };
        //for playing cards from hand
        gameView.playCardEvent += (sender, args) => { PlayCard(args); };
		gameView.fightEvent += (sender, x) => {FightStory();};
		gameView.collectRewardEvent += (sender, x) => {CollectReward();};
		gameView.riggedGameEvent += (sender, x) => {Construct(4, true);};
    }

	public void Construct(int numPlayers, bool rigged)
    {
        gameTemplate.numberOfPlayers = numPlayers;
        gameModel = gameTemplate.GetComponent<GameModel>();
		if (rigged) {
			gameModel.fast = true;
		}
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
			file.WriteLine ("CONTROL : A new game has begun with "+numPlayers+" players");
        //maybe these 2 functions would take an array that determines how many of each
        //card are in each deck
        gameModel.CreateAdventureDeck();
        gameModel.CreateStoryDeck();

        gameModel.CreateAllPlayers();
        for (int player = 0; player < numPlayers; player++)
        {
            DealAdventureCards(player, 12);
        }

        gameView.UpdateMessage("Player " + gameModel.GetCurrentPlayer().ToString() + ", it is your turn.");
		hotSeat = gameModel.GetCurrentPlayer ();
        //draw start turn button
        gameView.ShowStartTurnButton();
        gameView.SetUpHUD(numPlayers);
    }

    public void StartTurn(bool newTurn)
    {
        Debug.Log("StartTurn");
		hotSeat = gameModel.GetCurrentPlayer ();
		EnforceHandSize (context.None);
        //called by the start turn button
        if (newTurn)
        {
            //show current player, and UI elements;
            UpdatePlayer();
            gameView.TogglePlayerView(true);
            UpdateHUD(gameModel.GetNumberOfPlayers());

            gameView.UpdateMessage("");
            //draw a new story card
            gameModel.DrawStoryCard();
            //update UI representation of the story card
            gameView.UpdateStoryCard(gameModel.GetCurrentStoryCard().getFace(), gameModel.GetCurrentStoryCard().getBack(), true, false);
        }

        //called by the take action button
        else
        {
            //show engaged player, and UI elements;
            UpdatePlayer();
            gameView.TogglePlayerView(true);
            UpdateHUD(gameModel.GetNumberOfPlayers());
            //update buttons and context based on what the sequence is
            gameView.UpdateMessage(gameModel.GetStoryMessage());
            //update play card context
			if (gameModel.GetStoryContext () == "AddToStory") {
				playCardContext = context.AddToStory;
				recoverContext = context.AddToStory;
				EnforceHandSize (context.AddToStory);
				ResetPendingCards ();
				gameView.ShowDeclineButton ();
			} else if (gameModel.GetStoryContext () == "Play") {
				playCardContext = context.Play;
				recoverContext = context.Play;
				EnforceHandSize (context.Play);
				gameView.ShowFightButton ();
			} else if (gameModel.GetStoryContext () == "None") {
				playCardContext = context.None;
				EnforceHandSize (context.None);
				gameView.ShowDeclineButton ();
				gameView.ShowJoinButton ();
			} else {
				playCardContext = context.None;
				EnforceHandSize (context.None);
				gameView.ShowCollectRewardButton ();
			}
        }
    }

    public void RevealStoryCard()
    {
        Debug.Log("RevealStoryCard");
        //story card has just been flipped
        // begin/do story button appears 
        gameView.ShowContinueButton();
    }

    public void DoStory()
    {
        Debug.Log("DoStory");
        gameModel.PerformStage();
        UpdatePlayer();
        UpdateHUD(gameModel.GetNumberOfPlayers());
		EnforceHandSize (context.None);
        if (gameModel.IsStoryComplete())
        {
            gameView.ShowEndTurnButton();
        }
        else
        {
            gameView.UpdateMessage("A player must take an action");
            gameView.ShowPassButton();
        }
    }

    public void SeatChange(bool newTurn)
    {
        Debug.Log("SeatChange");
		EnforceHandSize (context.None);
        gameView.TogglePlayerView(false);
		gameView.UpdateStageMessage ("");
		gameView.ToggleStage (false);

        //this was called by an end turn button
        if (newTurn)
        {
            gameView.UpdateStoryCard(null, null, false, true);
			int temp = gameModel.CheckWin ();
            if (temp != -1)
            {
                //update view to show the game is over and who the winner(s) are
                gameView.GameOver(temp);
            }
            else
			{
                //show start turn button
                gameView.ShowStartTurnButton();
                //change the active player in the model
                gameModel.NextPlayer();
                //show which player has the next turn
                gameView.UpdateMessage("Player " + gameModel.GetCurrentPlayer().ToString() + ", it is your turn.");
            }
        }
        //this was called by a "let a player take action" button or decline button
        else
        {
            gameModel.NextPlayer();
            //show the "take action" button
            gameView.ShowTakeActionButton();
            //show which player has the next action
            gameView.UpdateMessage("Player " + gameModel.GetCurrentPlayer().ToString() + ", must take an action.");
        }
    }

    public void PlayCard(int i)
    {
        //play the ith card in the current players hand
        if (playCardContext == context.Play)
        {
			string weap = gameModel.PlayCard (hotSeat, i);
			if (weap != "Invalid") {
				gameView.UpdateStageMessage ("You equipped a "+weap);
				using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
					file.WriteLine ("- Player "+hotSeat+" eqipped a "+weap+". New BP: "+gameModel.GetPlayerBattlePoints(hotSeat));
				UpdatePlayer();
			} else {
				gameView.UpdateStageMessage ("You can't equip that!");
				using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
					file.WriteLine ("- Player "+hotSeat+" tried to equip an invalid weapon.");
			}
        }
        else if (playCardContext == context.Discard)
        {
            gameModel.PlayerDiscardsCard(hotSeat, i);
            UpdatePlayer();
            Debug.Log("player discarded "+ i);
			EnforceHandSize (context.None);
        }
        else if (playCardContext == context.AddToStory)
        {
            gameModel.AddPendingCard(i);
            gameView.HighlightCard(i);
            gameView.ShowResetButton();
            gameView.ShowConfirmButton();
        }
        else
        {
            Debug.Log("Card played in unknown context");
        }
		UpdateHUD(gameModel.GetNumberOfPlayers());
    }

	public void UpdateHUD(int numPlayers)
	{
		Sprite[] playerIcons = new Sprite[numPlayers];
		int[] numCards = new int[numPlayers];
		int[] numShields = new int[numPlayers];
		Sprite[] rankCards = new Sprite[numPlayers];
		for (int i = 0; i < numPlayers; i++)
		{
			playerIcons[i] = gameModel.GetPlayerCOA(i);
			numCards[i] = gameModel.GetPlayerCardsInHand(i);
			numShields[i] = gameModel.GetPlayerShields(i);
			rankCards [i] = gameModel.GetPlayerRank (i);
		}
		gameView.UpdateHUD(playerIcons, numCards, numShields, rankCards);
	}

    public void DealAdventureCards(int player, int numCards)
    {
        for (int i = 0; i < numCards; i++)
        {
            gameModel.DealAdventureCard(player);
        }
		EnforceHandSize (context.None);
    }

    //short hand for a long function call to gameView.UpdatePlayer(...)
    public void UpdatePlayer()
    {
        gameView.UpdatePlayer(gameModel.GetPlayerHand(hotSeat), gameModel.GetPlayerCOA(hotSeat), gameModel.GetPlayerBattlePoints(hotSeat));
    }

    public void ConfirmPendingCards()
    {
        //some way to confirm these cards CAN be added
		if (gameModel.ConfirmPendingCards (hotSeat)) {
			UpdatePlayer ();
			gameView.UpdateStageMessage ("You are now the sponsor");
			playCardContext = context.Discard;
			DoStory ();
		} else {
			UpdatePlayer ();
			gameView.UpdateStageMessage ("That quest is invalid");
			gameView.ShowDeclineButton ();
		}

    }

    public void ResetPendingCards()
    {
        UpdatePlayer();
        gameModel.ResetPendingCards();
        gameView.ShowDeclineButton();
    }

    public void JoinStoryStage()
    {
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
			file.WriteLine ("- Player "+hotSeat+" joins the current quest");
        gameModel.JoinStoryStage();
		gameModel.DealAdventureCard (hotSeat);
		DoStory();
		EnforceHandSize (context.None);
    }

    public void DeclineStoryStage()
    {
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
			file.WriteLine ("- Player "+hotSeat+" declines the current quest");
        gameModel.DeclineStoryStage();
        playCardContext = context.Discard;
        DoStory();
    }

	public void FightStory()
	{
		gameView.UpdateStageView (gameModel.GetStoryStage ());
		if (gameModel.FightStory ()) {
			gameView.UpdateStageMessage ("You defeated the enemy!");
			gameModel.DealAdventureCard (hotSeat);
		} else {
			gameView.UpdateStageMessage ("You failed to defeat the enemy");
		}
		DoStory ();
	}

	public void CollectReward(){
		gameModel.CollectReward (hotSeat);
		DoStory ();
		EnforceHandSize (context.None);
	}

	private void EnforceHandSize(context c){
		if (gameModel.GetPlayerCardsInHand (hotSeat) > 12) {
			playCardContext = context.Discard;
			gameView.EnableView (false);
			gameView.UpdateErrorText ("Discard a card to continue...");
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
				file.WriteLine ("- Player "+hotSeat+" needs to discard before continuing");

		} else {
			gameView.EnableView (true);
			playCardContext = recoverContext;
			recoverContext = context.None;
			gameView.UpdateErrorText ("");
		}
	}
}
