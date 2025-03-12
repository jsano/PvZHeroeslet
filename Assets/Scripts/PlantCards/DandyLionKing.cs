using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandyLionKing : Card
{

	protected override IEnumerator OnThisPlay()
	{
		GameManager.Instance.DisableHandCards();
		yield return new WaitForSeconds(1);
		GameManager.Instance.zombieHero.ReceiveDamage((int)Mathf.Floor(GameManager.Instance.zombieHero.HP / 2));

		yield return base.OnThisPlay();
	}

}
