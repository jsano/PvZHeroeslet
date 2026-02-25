using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliquePeas : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return Glow();
		int id = AllCards.NameToID("Clique Peas");
		GameManager.Instance.ShuffleIntoDeck(team, new() { id, id });
		if (GameManager.Instance.team == team) GameManager.Instance.playerCliquePeas += 1;
		else GameManager.Instance.opponentCliquePeas += 1;
		for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted() && AllCards.InstanceToPrefab(Tile.GetTeamTiles(team)[i, j].planted).name == "Clique Peas")
				{
					Tile.GetTeamTiles(team)[i, j].planted.ChangeStats(1, 1);
				}
		yield return base.OnThisPlay();
	}

}
