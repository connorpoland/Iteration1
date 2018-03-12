using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel : MonoBehaviour {

    public Sprite[] faces;
    public Sprite cardBack;
    public int faceIndex;
    public int stages;
    public int currentStage;
    public bool complete;
    public string message;
    public string context;
	public int battlePoints;
	public int specialBattlePoints;

    public Sprite getFace()
    {
        return faces[faceIndex];
    }

    public Sprite getBack()
    {
        return cardBack;
    }

    public void setFace(int f)
    {
        if (f >= 0 && f < faces.Length)
        {
            faceIndex = f;
        }
    }

    virtual public void DoStage(GameModel gameModel)
    {

    }

    virtual public int GetCurrentEngagedPlayer()
    {
        return -1;
    }

    virtual public bool UpdateStory(CardModel[] cards)
    {
		return false;
    }

    public string GetContext()
    {
        return context;
    }

    virtual public void JoinStory()
    {

    }

    virtual public void DeclineStory()
    {

    }

    virtual public void NextPlayer()
    {

    }

	virtual public bool FightStory(PlayerModel p)
	{
		return true;
	}

	virtual public Sprite[] GetStage()
	{
		return null;
	}

	virtual public int CalcBattlePoints(CardModel c){
		return 0;
	}

	virtual public void CollectReward(GameModel g, int i){
	}

	virtual public CardModel[] CleanUp(){
		return null;
	}
}
