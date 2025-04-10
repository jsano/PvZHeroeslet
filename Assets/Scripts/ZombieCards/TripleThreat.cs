using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleThreat : Card
{

	protected override IEnumerator OnThisPlay()
	{	    
        yield return new WaitForSeconds(1);
        if (GameManager.Instance.team == team)
        {
            List<int> columns = new();
            for (int i = 0; i < 5; i++)
            {
                if (Tile.zombieTiles[0, i].planted == null && i != col) columns.Add(i);
            }
            for (int n = columns.Count - 1; n > 0; n--)
            {
                int k = UnityEngine.Random.Range(0, n + 1);
                var temp = columns[n];
                columns[n] = columns[k];
                columns[k] = temp;
            }
            GameManager.Instance.PlayCardRpc(FinalStats.MakeDefaultFS(AllCards.NameToID("Impfinity Clone")), 0, col, true);
            if (columns.Count > 0) GameManager.Instance.PlayCardRpc(FinalStats.MakeDefaultFS(AllCards.NameToID("Impfinity Clone")), 0, columns[0], true);
        }
        yield return base.OnThisPlay();
	}

    public override bool IsValidTarget(BoxCollider2D bc)
    {
        Tile t = bc.GetComponent<Tile>();
        if (t == null) return false;
        if (Tile.CanPlantInCol(t.col, Tile.zombieTiles, false, true)) return true;
        return false;
    }

}
