using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimaPleurodon : Card
{

	protected override IEnumerator OnCardDraw(Team team)
	{
		if (team == this.team)
		{
            int id = AllCards.NameToID("Magic Beanstalk");
            GameManager.Instance.ShuffleIntoDeck(team, new() { id });
        }
		yield return base.OnCardDraw(team);
	}

}
