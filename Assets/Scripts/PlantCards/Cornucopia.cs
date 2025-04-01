using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cornucopia : Card
{

	protected override IEnumerator OnThisPlay()
	{
		GameManager.Instance.DisableHandCards();
		
        yield return new WaitForSeconds(1);
        if (GameManager.Instance.team == team)
        {
            for (int col = 0; col < 5; col++)
		    {
                if (!Tile.CanPlantInCol(col, Tile.plantTiles, false, true)) continue;

                List<int> possible = new();
                for (int i = 0; i < AllCards.Instance.cards.Length; i++)
                {
                    if (AllCards.Instance.cards[i].team == team && AllCards.Instance.cards[i].type == Type.Unit && (col != 4 || AllCards.Instance.cards[i].amphibious))
                    {
                        possible.Add(i);
                    }
                }
                int chosen = possible[Random.Range(0, possible.Count)];
                GameManager.Instance.PlayCardRpc(HandCard.MakeDefaultFS(chosen), 0, col, true);
            }
        }

        yield return base.OnThisPlay();
	}

}
