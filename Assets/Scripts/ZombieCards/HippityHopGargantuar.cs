using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HippityHopGargantuar : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return MakeEgg();
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        if (hurt.Item1.GetComponent<Card>() == this) yield return MakeEgg();
        yield return base.OnCardHurt(hurt);
	}

    private IEnumerator MakeEgg()
    {
        choices.Clear();
        for (int j = 0; j < 5; j++)
        {
            if (Tile.CanPlantInCol(j, Tile.zombieTiles, false, false)) choices.Add(Tile.zombieTiles[0, j].GetComponent<BoxCollider2D>());
        }
        if (choices.Count > 0)
        {
            yield return new WaitForSeconds(1);
            var choice = choices[UnityEngine.Random.Range(0, choices.Count)];
            yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col);
            Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Mystery Egg")]).GetComponent<Card>();
            Tile.zombieTiles[int.Parse(GameManager.Instance.GetShuffledList()[0]), int.Parse(GameManager.Instance.GetShuffledList()[1])].Plant(c);
        }
    }

}
