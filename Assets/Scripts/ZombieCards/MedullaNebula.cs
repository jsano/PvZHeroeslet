using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedullaNebula : Card
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Unit && played.team == Team.Zombie && played.col == col)
        {
            yield return new WaitForSeconds(1);
            yield return GameManager.Instance.UpdateRemaining(2, team);
        }
        yield return base.OnCardPlay(played);
    }

}
