using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goatify : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Destroy(Tile.GetTeamTiles(GetOpponent(team))[row, col].planted.gameObject);
        Tile.GetTeamTiles(GetOpponent(team))[row, col].Unplant();
        Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Goat")]).GetComponent<Card>();
        Tile.GetTeamTiles(GetOpponent(team))[row, col].Plant(card);
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        List<BoxCollider2D> targets = new();
        int highest = -1;
        for (int r = 0; r < Tile.ROWS; r++) for (int i = 0; i < 5; i++) if (Tile.GetTeamTiles(GetOpponent(team))[r, i].HasRevealedPlanted() && Tile.GetTeamTiles(GetOpponent(team))[r, i].planted.atk > highest)
            {
                highest = Tile.GetTeamTiles(GetOpponent(team))[r, i].planted.atk;
            }
        for (int r = 0; r < Tile.ROWS; r++) for (int i = 0; i < 5; i++) if (Tile.GetTeamTiles(GetOpponent(team))[r, i].HasRevealedPlanted() && Tile.GetTeamTiles(GetOpponent(team))[r, i].planted.atk == highest)
            {
                targets.Add(Tile.GetTeamTiles(GetOpponent(team))[r, i].GetComponent<BoxCollider2D>());
            }
        if (targets.Contains(bc)) return true;
        return false;
    }

}