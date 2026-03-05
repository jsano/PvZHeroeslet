using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MadRespect : Buff
{

    protected override int CardHurtModifiers(Tuple<Damagable, Card, int> hurt)
    {
        if (hurt.Item1.GetComponent<Hero>() != null && hurt.Item1.GetComponent<Hero>().team != team)
        {
            int count = 0;
            for (int i = 0; i < Tile.ROWS; i++)
            {
                for (int j = 0; j < Tile.COLUMNS; j++)
                {
                    if (Tile.GetTeamTiles(Card.GetOpponent(team))[i, j].HasRevealedPlanted()) count++;
                }
            }
            if (count > 0) StartCoroutine(Glow());
            return count * 2;
        }
        return base.CardHurtModifiers(hurt);
    }

}
