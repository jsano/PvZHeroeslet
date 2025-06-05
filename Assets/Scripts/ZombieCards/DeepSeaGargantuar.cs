using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeepSeaGargantuar : Card
{

	protected override IEnumerator OnCardMoved(Card moved)
	{
		if (moved.team == Team.Zombie) moved.ChangeStats(1, 1);
		yield return base.OnCardMoved(moved);
	}

}