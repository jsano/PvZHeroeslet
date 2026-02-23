using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhantomPain : Buff
{

    private Dictionary<HandCard, int> cap = new();

    protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        if (hurt.Item1 == GameManager.Instance.GetTeamHero(team))
        {
            if (team == GameManager.Instance.team)
            {
                foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Card.Type.Unit)
                    {
                        if (cap.ContainsKey(hc))
                        {
                            if (cap[hc] < 5)
                            {
                                cap[hc]++;
                                hc.ChangeAttack(1);
                            }
                        }
                        else
                        {
                            cap[hc] = 1;
                            hc.ChangeAttack(1);
                        }
                }
            }
        }
        yield return base.OnCardHurt(hurt);
    }  
    
}
