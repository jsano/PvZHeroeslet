using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regifting : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        int id = AllCards.RandomFromCost(Team.Zombie, (0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
		FinalStats fs = new(id);
		fs.atk += 1;
		//fs.hp += 1;
        int id1 = AllCards.RandomFromCost(Team.Plant, (0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
        FinalStats fs1 = new(id1);
        fs1.atk -= 1;
        //fs1.hp -= 1;
        StartCoroutine(GameManager.Instance.GainHandCard(Team.Plant, id1, fs1));
        yield return GameManager.Instance.GainHandCard(Team.Zombie, id, fs);
		yield return base.OnThisPlay();
    }

}
