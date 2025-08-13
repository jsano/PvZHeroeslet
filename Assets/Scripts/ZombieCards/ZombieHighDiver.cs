using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHighDiver : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (col == 0)
		{
            yield return Glow();
            ChangeStats(1, 1);
			Move(row, 4);
		}
		yield return base.OnThisPlay();
	}

}
