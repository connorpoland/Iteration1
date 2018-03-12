using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerModel : PlayerModel {

	public Strategy strat;

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

	public void SetStrategy(int s){
		if (s == 1) {
			strat = new Strategy1 ();
		} else {
			strat = new Strategy2 ();
		}
	}
}
