using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gentleman : Card
{

	public override IEnumerator OnZombieTricks()
	{
        yield return Glow();
        yield return GameManager.Instance.UpdateRemaining(2, team);
		yield return base.OnZombieTricks();
	}

}
