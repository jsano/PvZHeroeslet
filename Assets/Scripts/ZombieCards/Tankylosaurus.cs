using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tankylosaurus : Card
{

	protected override IEnumerator OnCardDraw(Team team)
	{
		if (team == this.team)
		{
            choices.Clear();
            if (this.team == GameManager.Instance.team)
            {
                for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
                    {
                        if (Tile.plantTiles[i, j].planted != null) choices.Add(Tile.plantTiles[i, j].GetComponent<BoxCollider2D>());
                    }
                choices.Add(GameManager.Instance.plantHero.GetComponent<BoxCollider2D>());
                for (int n = choices.Count - 1; n > 0; n--)
                {
                    int k = UnityEngine.Random.Range(0, n + 1);
                    var temp = choices[n];
                    choices[n] = choices[k];
                    choices[k] = temp;
                }
                if (choices[0].GetComponent<Hero>() != null) GameManager.Instance.StoreRpc(-1 + " - " + -1);
                else GameManager.Instance.StoreRpc(choices[0].GetComponent<Tile>().row + " - " + choices[0].GetComponent<Tile>().col);
            }
            yield return new WaitUntil(() => GameManager.Instance.shuffledList != null);

            if (int.Parse(GameManager.Instance.shuffledList[0]) == -1)
            {
                yield return AttackFX(Tile.plantHeroTiles[col]);
                yield return Tile.plantHeroTiles[col].ReceiveDamage(2, this);
            }
            else
            {
                Tile t = Tile.plantTiles[int.Parse(GameManager.Instance.shuffledList[0]), int.Parse(GameManager.Instance.shuffledList[1])];
                yield return AttackFX(t.planted);
                yield return t.planted.ReceiveDamage(2, this);
            }
        }
		yield return base.OnCardDraw(team);
	}

}
