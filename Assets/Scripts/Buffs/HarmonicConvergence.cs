using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HarmonicConvergence : Buff
{

    private Dictionary<Card.Class, int> done = new();
    private bool activated = false;

    protected override void Start()
    {
        for (int i = 0; i < Tile.ROWS; i++)
            for (int j = 0; j < Tile.COLUMNS; j++)
                if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted()) AddToDict(Tile.GetTeamTiles(team)[i, j].planted._class);
        Transform buffs = team == GameManager.Instance.team ? GameManager.Instance.playerBuffs : GameManager.Instance.opponentBuffs;
        foreach (Transform t in buffs) AddToDict(t.GetComponent<Buff>().buffClass);
        base.Start();
    }

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.team == team && played.type == Card.Type.Unit) AddToDict(played._class);
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1.team == team) done[died.Item1._class] -= 1;
        return base.OnCardDeath(died);
    }

    protected override IEnumerator OnCardBounce(Card bounced)
    {
        if (bounced.team == team) done[bounced._class] -= 1;
        return base.OnCardBounce(bounced);
    }

    protected override void OnBuffGainedImmediate(Buff gained)
    {
        if (gained.team == team) AddToDict(gained.buffClass);
        base.OnBuffGainedImmediate(gained);
    }

    private void AddToDict(Card.Class c)
    {
        if (done.ContainsKey(c)) done[c]++;
        else done[c] = 1;
        bool all = done.Count >= 6;
        foreach (Card.Class c1 in done.Keys) if (done[c1] <= 0) all = false;
        if (all) Activated();
    }

    private void Activated()
    {
        if (activated) return;
        activated = true;
        for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted()) Tile.GetTeamTiles(team)[i, j].planted.ChangeStats(2, 2);
        if (team == GameManager.Instance.team)
        {
            GameManager.Instance.playerPermanentAttackBonus += 2;
            GameManager.Instance.playerPermanentHPBonus += 2;
            foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Card.Type.Unit)
                {
                    hc.ChangeAttack(0);
                    hc.ChangeHP(0);
                }
        }
        else
        {
            GameManager.Instance.opponentPermanentAttackBonus += 2;
            GameManager.Instance.opponentPermanentHPBonus += 2;
        }
    }

}
