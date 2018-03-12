using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCardModel : CardModel {

    public void Construct(string n, int face, int s)
    {
        name = n;
        setFace(face);
        stages = s;
        currentStage = 1;
        complete = true;
    }

    override
    public void DoStage(GameModel gameModel)
    {
        complete = true;

        if (name == "Prosperity Throughout the Realm")
        {
            for (int i = 0; i < gameModel.GetNumberOfPlayers(); i++)
            {
                gameModel.DealAdventureCard(i);
                gameModel.DealAdventureCard(i);
            }
        }
        else if (name == "Chivalrous Deed")
        {
            int minShields = gameModel.GetPlayerShields(0);
			int minRank = gameModel.GetPlayerRankNum (0);
            int tempShields;
			int tempRank;

            for (int i = 1; i < gameModel.GetNumberOfPlayers(); i++)
            {
                tempShields = gameModel.GetPlayerShields(i);
				tempRank = gameModel.GetPlayerRankNum(i);
				if (tempRank <= minRank && tempShields < minShields)
                {
					minRank = tempRank;
					minShields = tempShields;
                }
            }
            for (int i = 0; i < gameModel.GetNumberOfPlayers(); i++)
            {
				if (gameModel.GetPlayerShields(i) == minShields && gameModel.GetPlayerRankNum(i) == minRank)
                {
                    gameModel.GainShields(i, 3);
                }
            }
        }
        else
        {
            Debug.Log("Event unknown");
        }
    }
}
