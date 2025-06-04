using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gentleman : Card
{

	public override IEnumerator OnZombieTricks()
	{
		yield return new WaitForSeconds(1);
		GameManager.Instance.UpdateRemaining(2, team);
		yield return base.OnZombieTricks();
	}

}
