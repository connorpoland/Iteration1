using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Strategy  {

	virtual public bool DoIPartispateInTournament(GameModel gameState){
		return false;
	}

	virtual public bool DoISponsorAQuest(GameModel gameState){
		return false;
	}

	virtual public bool DoIParticipateInQuest(){
		return false;
	}

	virtual public int NextBid(GameModel gameState){
		return 0;
	}

	virtual public CardModel WhatIDiscard(){
		return null;
	}

	virtual public CardModel[] WhatISetup(){
		return null;
	}

	virtual public void WhatIPlay(GameModel gameState, PlayerModel ourAIPlayer){
	}
}
