using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBlueberry : Card
{

	public override IEnumerator AfterCombat()
	{
		yield return new WaitForSeconds(1);
        if (team == GameManager.Instance.team)
        {
            for (int j = 0; j < 5; j++)
            {
                if (Tile.zombieTiles[0, j].planted != null) choices.Add(Tile.zombieTiles[0, j].GetComponent<BoxCollider2D>());
            }
            choices.Add(GameManager.Instance.zombieHero.GetComponent<BoxCollider2D>());
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
            yield return AttackFX(Tile.zombieHeroTiles[col]);
            yield return Tile.zombieHeroTiles[col].ReceiveDamage(6, this);
        }
        else
        {
            Tile t = Tile.zombieTiles[int.Parse(GameManager.Instance.shuffledList[0]), int.Parse(GameManager.Instance.shuffledList[1])];
            yield return AttackFX(t.planted);
            yield return t.planted.ReceiveDamage(6, this);
        }

        yield return base.AfterCombat();
	}

}
