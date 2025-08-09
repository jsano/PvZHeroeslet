using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStarfruit : Card
{

	public override IEnumerator Attack(int savedHP = -1)
	{
        if (frozen)
        {
            yield return new WaitForSeconds(0.5f);
            frozen = false;
            transform.Find("Frozen").gameObject.SetActive(false);
            SR.material.color = Color.white;
            yield break;
        }

        int finalAtk = atk;
        if (strengthHeart > 0)
        {
            if (savedHP != -1) finalAtk = savedHP;
            else finalAtk = HP;
        }
        if (finalAtk <= 0 || gravestone) yield break;

        yield return new WaitForSeconds(0.25f);

        List<Damagable>[] targets = new List<Damagable>[] { null, null, null, null, null };
        for (int i = 0; i < 5; i++)
        {
            targets[i] = GetTargets(i);
        }
        List<Damagable> temp = new();
        for (int i = 0; i < 5; i++) if (targets[i] != null) temp.Add(targets[i][0]);
        yield return AttackFXs(temp);

        for (int i = 0; i < 5; i++) if (targets[i] != null) foreach (Damagable c in targets[i]) StartCoroutine(c.ReceiveDamage(finalAtk, this, bullseye > 0, deadly > 0, freeze));

        yield return new WaitForSeconds(0.1f); // this only exists so all the receive damages get sent before the other units attack. TODO: fix??
    }

}
