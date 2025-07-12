using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomGrotto : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
        if (played.col == col && played.type == Type.Unit && played.team == Team.Plant)
        {
            yield return new WaitForSeconds(1);
            choices.Clear();
            if (team == GameManager.Instance.team)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Tile.CanPlantInCol(j, Tile.plantTiles, true, false)) choices.Add(Tile.plantTiles[1, j].GetComponent<BoxCollider2D>());
                }
                for (int n = choices.Count - 1; n > 0; n--)
                {
                    int k = UnityEngine.Random.Range(0, n + 1);
                    var temp = choices[n];
                    choices[n] = choices[k];
                    choices[k] = temp;
                }
                GameManager.Instance.StoreRpc(choices[0].GetComponent<Tile>().row + " - " + choices[0].GetComponent<Tile>().col);
            }
            yield return new WaitUntil(() => GameManager.Instance.shuffledList != null);

            Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Puff-shroom")]).GetComponent<Card>();
            Tile.plantTiles[int.Parse(GameManager.Instance.shuffledList[0]), int.Parse(GameManager.Instance.shuffledList[1])].Plant(c);
        }
        
        yield return base.OnCardPlay(played);
	}

}
