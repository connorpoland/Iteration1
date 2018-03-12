using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strategy2 : Strategy {
	
	override
	public bool DoIPartispateInTournament(GameModel gameState){
		return true;
	}

	override
	public bool DoISponsorAQuest(GameModel gameState){
		return false;
	}

	override
	public bool DoIParticipateInQuest(){
		return false;
	}

	override
	public int NextBid(GameModel gameState){
		return 0;
	}

	override
	public CardModel WhatIDiscard(){
		return null;
	}

	override
	public CardModel[] WhatISetup(){
		return null;
	}

	override
	public void WhatIPlay(GameModel gameState, PlayerModel ourAIPlayer){
	}
}
