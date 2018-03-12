using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameView : MonoBehaviour {

	private bool viewEnabled;

    private Button startGameButton4;
    private Button startTurnButton;
    private Button takeActionButton;
    private Button confirmButton;
    private Button resetButton;
    private Button declineButton;
    private Button joinButton;
	private Button fightButton;
    private Button passButton;
    private Button continueButton;
    private Button endTurnButton;
	private Button collectRewardButton;
	private Button riggedButton;

    private Text messageText;
	private Text stageText;
	private Text errorText;
    private StoryCardView storyCard;
    private PlayerView playerView;
    private HUDView hudView;
	public AdventureCardView[] stageView;
	public int stageSize;
	public Vector2 stageStart;
	public float stageOffset;

    public delegate void EventHandler(object sender, bool x);
    public event EventHandler startGameEvent4 = delegate { };
    public event EventHandler startTurnEvent = delegate { };
    public event EventHandler doStoryCardEvent = delegate { };
    public event EventHandler endTurnEvent = delegate { };
    public event EventHandler confirmEvent = delegate { };
    public event EventHandler resetEvent = delegate { };
    public event EventHandler declineEvent = delegate { };
    public event EventHandler joinEvent = delegate { };
	public event EventHandler fightEvent = delegate {};
    public event EventHandler continueEvent = delegate { };
    public event EventHandler cardInHandEvent = delegate { };
	public event EventHandler collectRewardEvent = delegate { };
	public event EventHandler riggedGameEvent = delegate { };
    public delegate void PlayCardHandler(object sender, int i);
    public event PlayCardHandler playCardEvent = delegate { };

    public Button startGameButton4Template;
    public Button startTurnButtonTemplate;
    public Button endTurnButtonTemplate;
    public Button takeActionButtonTemplate;
    public Button confirmButtonTemplate;
    public Button resetButtonTemplate;
    public Button declineButtonTemplate;
    public Button joinButtonTemplate;
	public Button fightButtonTemplate;
    public Button passButtonTemplate;
    public Button continueButtonTemplate;
	public Button collectRewardButtonTemplate;
	public Button riggedButtonTemplate;
    public Text messageTextTemplate;
	public Text stageTextTemplate;
	public Text errorTextTemplate;
    public StoryCardView storyCardViewTemplate;
	public AdventureCardView advCardViewTemplate;
    public PlayerView playerViewTemplate;
    public HUDView hudViewTemplate;

    public void SetUpView()
    {
		viewEnabled = true; 

        messageText = messageTextTemplate.GetComponent<Text>();
		stageText = stageTextTemplate.GetComponent<Text>();
		errorText = errorTextTemplate.GetComponent<Text>();

        storyCard = storyCardViewTemplate.GetComponent<StoryCardView>();
        storyCard.ToggleShow(true);
        storyCard.doStoryCardEvent += (sender, args) => { DoTurn(); };

        playerView = playerViewTemplate.GetComponent<PlayerView>();

        startGameButton4 = startGameButton4Template.GetComponent<Button>();
        startGameButton4.onClick.AddListener(StartGame4);
        showButton(startGameButton4, true);

        startTurnButton = startTurnButtonTemplate.GetComponent<Button>();
        startTurnButton.onClick.AddListener(StartTurn);
        showButton(startTurnButton, false);

        endTurnButton = endTurnButtonTemplate.GetComponent<Button>();
        endTurnButton.onClick.AddListener(EndTurn);
        showButton(endTurnButton, false);

        takeActionButton = takeActionButtonTemplate.GetComponent<Button>();
        takeActionButton.onClick.AddListener(takeAction);
        showButton(takeActionButton, false);

        confirmButton = confirmButtonTemplate.GetComponent<Button>();
        confirmButton.onClick.AddListener(confirmChoice);
        showButton(confirmButton, false);

        resetButton = resetButtonTemplate.GetComponent<Button>();
        resetButton.onClick.AddListener(resetChoice);
        showButton(resetButton, false);

        declineButton = declineButtonTemplate.GetComponent<Button>();
        declineButton.onClick.AddListener(declineChoice);
        showButton(declineButton, false);

        joinButton = joinButtonTemplate.GetComponent<Button>();
        joinButton.onClick.AddListener(joinChoice);
        showButton(joinButton, false);

        passButton = passButtonTemplate.GetComponent<Button>();
        passButton.onClick.AddListener(yieldAction);
        showButton(passButton, false);

        continueButton = continueButtonTemplate.GetComponent<Button>();
        continueButton.onClick.AddListener(continueChoice);
        showButton(continueButton, false);

		fightButton = fightButtonTemplate.GetComponent<Button>();
		fightButton.onClick.AddListener (FightReady);
		showButton (fightButton, false);

		collectRewardButton = collectRewardButtonTemplate.GetComponent<Button> ();
		collectRewardButton.onClick.AddListener (CollectReward);
		showButton (collectRewardButton, false);

		riggedButton = riggedButtonTemplate.GetComponent<Button> ();
		riggedButton.onClick.AddListener (StartRiggedGame);
		showButton (riggedButton, true);

        playerView.SetUpPlayerView();
        for (int i = 0; i < 17; i++)
        {
            playerView.advCardHand[i].playCardEvent += (sender, args) => { PlayCard(args); };
        }

        hudView = hudViewTemplate.GetComponent<HUDView>();

		stageView = new AdventureCardView[7];
		for (int i = 0; i < 7; i++)
		{
			stageView [i] = Instantiate (advCardViewTemplate.GetComponent<AdventureCardView> ());
			stageView [i].ToggleShow (false);
		}
    }

    public void SetUpHUD(int numPlayers)
    {
        hudView.SetUpHUDView(numPlayers);
    }

	public void UpdateHUD(Sprite[] coa, int[] numCards, int[] numShields, Sprite[] ranks)
    {
        hudView.UpdateHUDView(coa, numCards, numShields, ranks);
    }

    public void UpdateMessage(string message)
    {
        messageText.text = message;
    }

    public void UpdateStoryCard(Sprite cardFace, Sprite cardBack, bool showCard, bool showFace)
    {
		storyCard.SetCard (cardFace, cardBack);
		if (viewEnabled || showFace == false) {
			storyCard.ToggleFace (showFace);
		}
		storyCard.ToggleShow (showCard);
    }

    public void StartGame4()
    {
			showButton (startGameButton4, false);
			showButton (riggedButton, false);
			startGameEvent4 (this, true);
    }

	public void StartRiggedGame(){
		showButton (riggedButton, false);
		showButton (startGameButton4, false);
		riggedGameEvent (this, true);
	}

    public void StartTurn()
    {
		if (viewEnabled) {
			showButton (startTurnButton, false);
			startTurnEvent (this, true);
		}
    }

    public void DoTurn()
    {
		if (viewEnabled) {
			storyCard.cardBoundry.enabled = false;
			doStoryCardEvent (this, true);
		}
    }

    public void PlayCard(int i)
    {
        playCardEvent(this, i);
    }

    public void EndTurn()
    {
		if (viewEnabled) {
			showButton (endTurnButton, false);
			endTurnEvent (this, true);
		}
    }

    public void takeAction()
    {
		if (viewEnabled) {
			showButton (takeActionButton, false);
			startTurnEvent (this, false);
		}
    }

    public void confirmChoice()
    {
		if (viewEnabled) {
			showButton (confirmButton, false);
			showButton (resetButton, false);
			showButton (declineButton, false);
			confirmEvent (this, true);
		}
    }

    public void resetChoice()
    {
		if (viewEnabled) {
			showButton (confirmButton, false);
			showButton (resetButton, false);
			showButton (declineButton, false);
			resetEvent (this, true);
		}
    }

    public void declineChoice()
    {
		if (viewEnabled) {
			showButton (joinButton, false);
			showButton (confirmButton, false);
			showButton (declineButton, false);
			declineEvent (this, true);
		}
    }

    public void joinChoice()
    {
		if (viewEnabled) {
			showButton (joinButton, false);
			showButton (declineButton, false);
			joinEvent (this, true);
		}
    }

    public void yieldAction()
    {
		if (viewEnabled) {
			showButton (passButton, false);
			endTurnEvent (this, false);
		}
    }

    public void continueChoice()
    {

		if (viewEnabled) {
			showButton (continueButton, false);
			continueEvent (this, true);
		}
    }

    public void GameOver(int player)
    {
        showButton(endTurnButton, false);
        showButton(startTurnButton, false);
        storyCard.ToggleShow(false);
        messageText.text = "Player " + player.ToString() + " wins!";
    }

	public void FightReady()
	{
		if (viewEnabled) {
			showButton (fightButton, false);
			fightEvent (this, true);
		}
	}

    public void showButton(Button btn, bool showBtn)
    {
        if (showBtn)
        {
            btn.enabled = true;
            btn.GetComponentInChildren<CanvasRenderer>().SetAlpha(1);
            btn.GetComponentInChildren<Text>().color = Color.black;
        }
        else
        {
            btn.enabled = false;
            btn.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
            btn.GetComponentInChildren<Text>().color = Color.clear;
        }
    }

    public void UpdatePlayer(Sprite[] hand, Sprite coatOfArms, int numShields)
    {
        playerView.UpdatePlayerView(hand, coatOfArms, numShields);
        playerView.ToggleShow(true);
    }

    public void TogglePlayerView(bool showPlayer)
    {
        playerView.ToggleShow(showPlayer);
    }

    public void ShowStartTurnButton()
    {
        showButton(startTurnButton, true);
    }

    public void ShowContinueButton()
    {
        showButton(continueButton, true);
    }

    public void ShowEndTurnButton()
    {
        showButton(endTurnButton, true);
    }

    public void ShowPassButton()
    {
        showButton(passButton, true);
    }

    public void ShowTakeActionButton()
    {
        showButton(takeActionButton, true);
    }

    public void HighlightCard(int i)
    {
        playerView.HighlightCard(i);
    }

    public void ShowResetButton()
    {
        showButton(resetButton, true);
    }

    public void ShowConfirmButton()
    {
        showButton(confirmButton, true);
    }

    public void ShowJoinButton()
    {
        showButton(joinButton, true);
    }

    public void ShowDeclineButton()
    {
        showButton(declineButton, true);
    }

	public void ShowFightButton()
	{
		showButton (fightButton, true);
	}

	public void UpdateStageView(Sprite[] stage)
	{
		stageSize = stage.Length;
		for (int c = 0; c < stageSize; c++)
		{
			float co = stageOffset * c;
			Vector2 temp = stageStart + new Vector2(co, 0f);
			stageView[c].transform.position = temp;
			//CHANGE THIS SO IT ACTUALLY HAS THE RIGHT BACK
			stageView[c].SetCard(stage[c], stage[c]);
			stageView[c].ToggleShow(true);
			stageView[c].ToggleFace(true);
			stageView[c].SetIndex(c);
		}
		for (int d = stageSize; d < 7; d++)
		{
			stageView[d].ToggleShow(false);
		}
	}

	public void ToggleStage(bool b)
	{
		if (!b) {
			for (int i = 0; i < 7; i++) {
				stageView[i].ToggleShow (false);
			}
		}
	}

	public void UpdateStageMessage(string message){
		stageText.text = message;
	}

	public void CollectReward(){
		if (viewEnabled) {
			collectRewardEvent (this, true);
			showButton (collectRewardButton, false);
		}
	}

	public void ShowCollectRewardButton()
	{
		showButton (collectRewardButton, true);
	}

	public void EnableView(bool x){
		viewEnabled = x;
		storyCard.viewEnabled = x;
	}

	public void UpdateErrorText(string s){
		errorText.text = s;
	}
}
