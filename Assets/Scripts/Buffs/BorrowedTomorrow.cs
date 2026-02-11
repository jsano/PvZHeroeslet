using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BorrowedTomorrow : Buff
{

    private int state = 1; // 1 = free, 0 = cost more, -1 = done

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        if (team == GameManager.Instance.team)
        {
            GameManager.Instance.playerUnitPermanentDiscount += 100;
            GameManager.Instance.playerTrickPermanentDiscount += 100;
        }
        if (team == GameManager.Instance.team) foreach (HandCard hc in GameManager.Instance.GetHandCards()) hc.ChangeCost(0);
        base.Start();
    }

    protected override void OnHandCardPlayImmediate(Tuple<Card.Team, int> played)
    {
        if (played.Item1 == team)
        {
            state -= 1;
            if (state == 0)
            {
                GameManager.Instance.playerUnitPermanentDiscount -= 103;
                GameManager.Instance.playerTrickPermanentDiscount -= 103;
                foreach (HandCard hc in GameManager.Instance.GetHandCards()) hc.ChangeCost(0);
            }
            else
            {
                GameManager.Instance.playerUnitPermanentDiscount += 3;
                GameManager.Instance.playerTrickPermanentDiscount += 3;
                foreach (HandCard hc in GameManager.Instance.GetHandCards()) hc.ChangeCost(0);
            }
        }
        base.OnHandCardPlayImmediate(played);
    }

}
