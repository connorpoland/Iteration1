using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCardModel : CardModel {

    private string[] specialFoes;
    private int[] engagedPlayers;
    private int numEngagedPlayers;
    private int[] disengagedPlayers;
    private int numDisengagedPlayers;
    private int currentEngagedPlayer;
    private CardModel[][] questStages;
	private int sponsor;

    public void Construct(string n, int face, int s, string[] foes)
    {
        name = n;
        setFace(face);
        stages = s;
        currentStage = 0;
        specialFoes = foes;
        complete = true;
        questStages = new CardModel[s][];
		for (int i = 0; i < s; i++) {
			questStages [i] = new CardModel[7];
		}
        sponsor = -1;
    }

    override
    public void DoStage(GameModel gameModel)
    {
        //if this is the first time DoStage is being called for this story card
        if(engagedPlayers == null)
        {
            //make a list of possible sponsors. starting with the current player
            disengagedPlayers = new int[gameModel.GetNumberOfPlayers()];
            numDisengagedPlayers = 0;
            engagedPlayers = new int[gameModel.GetNumberOfPlayers()];
            numEngagedPlayers = gameModel.GetNumberOfPlayers();
            engagedPlayers[0] = gameModel.GetCurrentPlayer();
            Debug.Log(gameModel.GetCurrentPlayer());
            for (int i = 1; i < gameModel.GetNumberOfPlayers(); i++)
            {
                engagedPlayers[i] = (gameModel.GetCurrentPlayer() + i) % gameModel.GetNumberOfPlayers();
            }
            currentEngagedPlayer = 0;
            complete = false;
        }
        //if we dont have a sponsor yet
		if (currentStage > stages + 1) {
			complete = true;
		} else if (sponsor == -1) {
			message = "Sponsor: " + name + "?";
			context = "AddToStory";
		}
        //other stages will go in else ifs here
		else if (currentStage == 0) {
			message = "Join: " + name + "?";
			context = "None";
		} else if (currentStage <= stages) {
			gameModel.DiscardWeapons ();
			message = "Stage: " + currentStage + " - a foe is encountered!";
			context = "Play";
		} else {
			gameModel.DiscardWeapons ();
			//win stage 
			message = name+" has ended!";
			context = "End";
		}
    }

    override
    public int GetCurrentEngagedPlayer()
    {
        return engagedPlayers[currentEngagedPlayer];
    }

    override
    public bool UpdateStory(CardModel[] cards)
    {
		//validate quest
		int[] runningTotals = new int[stages];
		int tempStage = -1;
		int[] weapons = new int[6];
		for (int c = 0; c < cards.Length; c++) {
			if ((cards [c].name == "Boar" ||
				cards [c].name == "Dragon" ||
				cards [c].name == "Robber Knight" ||
				cards [c].name == "Giant" ||
				cards [c].name == "Green Knight" ||
				cards [c].name == "Saxons" ||
				cards [c].name == "Thieves" ||
				cards [c].name == "Mordred"
			)) {
				if (tempStage + 1 >= stages) {
					using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
						file.WriteLine ("- Player "+GetCurrentEngagedPlayer()+"s sponsorship attempt included too many foes.");
					return false;
				}
				tempStage++;
				runningTotals [tempStage] = CalcBattlePoints (cards[c]);
				Debug.Log ("Foe Ok");
				weapons = new int[6];
			} else {
				if (tempStage == -1) {
					using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
						file.WriteLine ("- Player "+GetCurrentEngagedPlayer()+"s sponsorship attempt included a weapon with no foe to equip it.");
					return false;
				}
				if (cards [c].name == "Dagger" && weapons [0] != 1) {
					weapons [0] = 1;
				} else if (cards [c].name == "Sword" && weapons [1] != 1) {
					weapons [1] = 1;
				} else if (cards [c].name == "Lance" && weapons [2] != 1) {
					weapons [2] = 1;
				} else if (cards [c].name == "Battle-ax" && weapons [3] != 1) {
					weapons [3] = 1;
				} else if (cards [c].name == "Horse" && weapons [4] != 1) {
					weapons [4] = 1;
				} else if (cards [c].name == "Excalibur" && weapons [5] != 1) {
					weapons [5] = 1;
				} else {
					using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
						file.WriteLine ("- Player "+GetCurrentEngagedPlayer()+"s sponsorship attempt broke the repeated weapon rule.");
					return false;
				}
				runningTotals [tempStage] = runningTotals [tempStage] + cards [c].battlePoints;
			}
		}
		//ascending bp rule
		for (int i = 0; i < stages - 1; i++) {
			if (runningTotals [i] >= runningTotals [i + 1]) {
				using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
					file.WriteLine ("- Player "+GetCurrentEngagedPlayer()+"s sponsorship attempt broke the increasing BP rule. Stage "+(i+1)+": "+runningTotals [i]+"BP - Stage "+(i+2)+": "+runningTotals [i+1]+"BP");
				return false;
			}
		}
		//add the cards
		string str = "Player "+GetCurrentEngagedPlayer()+" sponsors "+this.name+" with ";
		int x = -1;
		int y = 0;
		for (int c = 0; c < cards.Length; c++) {
			if (cards [c].name == "Boar" ||
				cards [c].name == "Dragon" ||
				cards [c].name == "Giant" ||
				cards [c].name == "Robber Knight" ||
				cards [c].name == "Green Knight" ||
				cards [c].name == "Saxons" ||
				cards [c].name == "Thieves" ||
				cards [c].name == "Mordred"
			) {
				y = 0;
				x++;
				questStages [x] [y] = cards [c];
				y++;
			} else {
				Debug.Log (x + " " + y);
				questStages [x] [y] = cards [c];
				y++;
			}
		}

		sponsor = GetCurrentEngagedPlayer();
		//make a list of possible players.
		int tempL = engagedPlayers.Length;
		disengagedPlayers = new int[tempL-1];
		numDisengagedPlayers = 0;
		engagedPlayers = new int[tempL-1];
		numEngagedPlayers = tempL-1;
		for (int i = 0; i < tempL-1; i++)
		{
			engagedPlayers [i] = (sponsor + i + 1) % tempL;
		}
		currentEngagedPlayer = 0;
		complete = false;
		int count = 0;
		for (int i = 0; i < stages; i++) {
			for (int j = 0; j < 7; j++){
				if(questStages[i][j] != null)
				{
					str += questStages [i] [j].name + " ";
					count += CalcBattlePoints (questStages [i] [j]);
				}
			}
			str += count + " BP, ";
			count = 0;
		}
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
			file.WriteLine (str);
		return true;
    }

    override
    public void DeclineStory()
    {
        //remove currentEngagedPlayer from engaged players
        //add currentEngagedPlayer to disengagedPlayers
        disengagedPlayers[numDisengagedPlayers] = GetCurrentEngagedPlayer();
        numDisengagedPlayers++;
		if (sponsor == -1) {
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
				file.WriteLine ("-- MODEL : Player "+GetCurrentEngagedPlayer()+" declined sponsoring "+this.name);
		}
        if (numEngagedPlayers == numDisengagedPlayers)
        {
			engagedPlayers = new int[1];
			numEngagedPlayers = 1;
			engagedPlayers [0] = sponsor;
			currentStage = stages + 1;
			currentEngagedPlayer = -1;
        }
		else if (currentEngagedPlayer == numEngagedPlayers-1) 
		{
			currentStage++;
			//make a list of possible players.
			int[] temp;
			if (currentStage > stages) {
				temp = new int[numEngagedPlayers - numDisengagedPlayers +  1];
			} else {
				temp = new int[numEngagedPlayers - numDisengagedPlayers];
			}

			int index = 0;
			for (int i = 0; i < 4; i++) {
				if (PlayerContinues ((sponsor + i + 1) % 4)) {
					temp [index] = (sponsor + i + 1) % (4);
					index++;
					Debug.Log (temp [index-1]);
				}
			}
			if (currentStage > stages) {
				temp[index] = sponsor;
			}
			engagedPlayers = temp;
			numEngagedPlayers = numEngagedPlayers - numDisengagedPlayers;
			disengagedPlayers = new int[temp.Length];
			numDisengagedPlayers = 0;
			currentEngagedPlayer = 0;
		}
    }

	override 
	public void JoinStory()
	{
		if (currentEngagedPlayer == numEngagedPlayers - 1) {
			currentStage++;
			//make a list of possible players.
			int[] temp;
			if (currentStage > stages) {
				temp = new int[numEngagedPlayers - numDisengagedPlayers +  1];
			} else {
				temp = new int[numEngagedPlayers - numDisengagedPlayers];
			}

			int index = 0;
			for (int i = 0; i < 4; i++) {
				if (PlayerContinues ((sponsor + i + 1) % 4)) {
					temp [index] = (sponsor + i + 1) % (4);
					index++;
					Debug.Log (temp [index-1]);
				}
			}
			if (currentStage > stages) {
				temp[index] = sponsor;
			}
			engagedPlayers = temp;
			numEngagedPlayers = numEngagedPlayers - numDisengagedPlayers;
			disengagedPlayers = new int[temp.Length];
			numDisengagedPlayers = 0;
			currentEngagedPlayer = 0;
		} else {
			if (numDisengagedPlayers == 0) {
				currentEngagedPlayer++;
			}
		}
	}

    override
    public void NextPlayer()
    {
        //Everyone has declined
        //else
        //{
		if (numDisengagedPlayers != 0)
            {
                currentEngagedPlayer++;
				Debug.Log ("pass");
            }
        //}
    }

	public bool PlayerContinues(int p)
	{
		bool found = false;
		for (int x = 0; x < numEngagedPlayers; x++) {
			if (p == engagedPlayers [x]) {
				found = true;
			}
		}
		for (int i = 0; i < numDisengagedPlayers; i++) {
			if(p == disengagedPlayers[i])
			{
					return false;
			}
		}
		if (p == sponsor) {
			return false;
		}
		Debug.Log ("Player " + p + " Continues");
		return found;
	}

	override
	public bool FightStory(PlayerModel player)
	{
		int total = 0;
		for (int i = 0; i < 7; i++) {
			if (questStages[currentStage-1][i] != null){
				total = total + CalcBattlePoints (questStages [currentStage - 1] [i]);
			}
		}
		if (player.GetBattlePoints () >= total) {
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
				file.WriteLine ("-- MODEL : Player "+GetCurrentEngagedPlayer()+" defeated stage "+(currentStage)+" in the "+this.name+" : "+player.GetBattlePoints ()+"BP vs "+total+"BP");
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
				file.WriteLine ("-- MODEL : Player "+GetCurrentEngagedPlayer()+" will continue to stage "+currentStage+1);
			JoinStory ();
			return true;
		} else {
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
				file.WriteLine ("-- MODEL : Player "+GetCurrentEngagedPlayer()+" was defeated by stage  "+(currentStage)+" in the "+this.name+" : "+player.GetBattlePoints ()+"BP vs "+total+"BP.");
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
				file.WriteLine ("-- MODEL : Player "+GetCurrentEngagedPlayer()+" is eliminated from "+this.name);
			DeclineStory ();
			return false;
		}
	}

	override
	public Sprite[] GetStage()
	{
		int counter = 0;
		for (int x = 0; x < 7; x++) {
			if (questStages [currentStage - 1] [x] != null) {
				counter++;
			}
		}
		Sprite[] temp = new Sprite[counter];
		int index = 0;
		for (int i = 0; i < 7; i++){
			if (questStages [currentStage - 1] [i] != null) {
				temp [index] = questStages [currentStage - 1] [i].getFace ();
				index++;
			}
		}
		return temp;
	}

	override
	public int CalcBattlePoints(CardModel c){
		for (int i = 0; i < specialFoes.Length; i++) {
			if (c.name == specialFoes [i]) {
				return c.specialBattlePoints;
			}
		}
		return c.battlePoints;
	}

	override
	public void CollectReward(GameModel gameModel, int p){
		if (p == sponsor) {
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
				file.WriteLine ("-- MODEL : Player "+p+" collects their reward for sponsoring the "+this.name);
			int count = 0;
			for (int i = 0; i < stages; i++) {
				for (int c = 0; c < 7; c++) {
					if (questStages [i] [c] != null) {
						count++;
					}
				}
			}
			for (int s = 0; s < stages+count; s++) {
				gameModel.DealAdventureCard (p);
				complete = true;
			}
		} else {
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
				file.WriteLine ("-- MODEL : Player "+p+" collects their reward for completeing the "+this.name);
			gameModel.GainShields (p, stages);
			currentEngagedPlayer++;
			Debug.Log (currentEngagedPlayer);
		}
	}

	override
	public CardModel[] CleanUp()
	{
		int count = 0;
		for (int i = 0; i < stages; i++) {
			for (int c = 0; c < 7; c++) {
				if (questStages [i] [c] != null) {
					count++;
				}
			}
		}
		CardModel[] temp = new CardModel[count];
		int index = 0;
		for (int i = 0; i < stages; i++) {
			for (int c = 0; c < 7; c++) {
				if (questStages [i] [c] != null) {
					temp [index] = questStages [i] [c];
					index++;
				}
			}
		}
		Debug.Log (count + " cleaned");
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@".\MyLogs.txt", true))
			file.WriteLine ("-- MODEL : the "+this.name+" and it's "+count+" adventure cards are discarded");
		return temp;
	}
}
