using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomGrotto : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
        choices.Clear();
        if (played.col == col && played.type == Type.Unit && played.team == team)
        {
            for (int j = 0; j < Tile.COLUMNS; j++)
            {
                if (Tile.CanPlantInCol(j, Tile.GetTeamTiles(team), true, false)) choices.Add(Tile.GetTeamTiles(team)[1, j].GetComponent<BoxCollider2D>());
            }
            if (choices.Count > 0)
            {
                yield return Glow();
                var choice = choices[UnityEngine.Random.Range(0, choices.Count)];
                yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col);
                Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Puff-shroom")]).GetComponent<Card>();
                Tile.GetTeamTiles(team)[int.Parse(GameManager.Instance.GetShuffledList()[0]), int.Parse(GameManager.Instance.GetShuffledList()[1])].Plant(c);
            }
        }
        
        yield return base.OnCardPlay(played);
	}

}
