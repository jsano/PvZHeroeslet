using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomGrotto : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
        choices.Clear();
        if (played.col == col && played.type == Type.Unit && played.team == Team.Plant)
        {
            for (int j = 0; j < 5; j++)
            {
                if (Tile.CanPlantInCol(j, Tile.plantTiles, true, false)) choices.Add(Tile.plantTiles[1, j].GetComponent<BoxCollider2D>());
            }
            if (choices.Count > 0)
            {
                yield return new WaitForSeconds(1);
                var choice = choices[UnityEngine.Random.Range(0, choices.Count)];
                yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col);
                Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Puff-shroom")]).GetComponent<Card>();
                Tile.plantTiles[int.Parse(GameManager.Instance.GetShuffledList()[0]), int.Parse(GameManager.Instance.GetShuffledList()[1])].Plant(c);
            }
        }
        
        yield return base.OnCardPlay(played);
	}

}
