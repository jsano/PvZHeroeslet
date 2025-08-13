using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Damagable
{

	public Card.Team team;
    public Card.Class[] classes;
	public Card[] superpowers;
    public int HP = 20;
	private int maxHP;
	public TextMeshProUGUI hpUI;
	private SpriteRenderer SR;
	private GameObject target;
	public Image blockMeter;
	private int block;
	private int timesBlocked = 0;
	public GameObject eight;

	public Transform thinking;

	// Start is called before the first frame update
	void Start()
    {
        target = transform.Find("Target").gameObject;
        SR = GetComponent<SpriteRenderer>();
		hpUI.text = HP + "";
		maxHP = HP;

		for (int i = 0; i < 3; i++) StartCoroutine(DotAnimation(i));
	}

    private IEnumerator DotAnimation(int index)
    {
		var dot = thinking.GetChild(index).gameObject;
		var start = dot.transform.position.y;
        yield return new WaitForSeconds(index * 0.5f);
        while (true)
        {
            yield return new WaitForSeconds(3f);
			LeanTween.moveY(dot, dot.transform.position.y + 0.1f, 0.25f).setEaseOutQuad().setOnComplete(() => 
			LeanTween.moveY(dot, dot.transform.position.y - 0.1f, 0.75f).setEaseOutElastic().setOnComplete(() => dot.transform.position = new Vector3(dot.transform.position.x, start)));
        }
    }
    public void ToggleThinking(bool on)
    {
		//thinking.localPosition = new Vector3(thinking.localPosition.x, thinking.localPosition.y, on ? 0 : -10);
		thinking.GetComponent<SpriteRenderer>().enabled = on;
        for (int i = 0; i < 3; i++) thinking.GetChild(i).GetComponent<SpriteRenderer>().enabled = on;
    }

	public override IEnumerator ReceiveDamage(int dmg, Card source, bool bullseye = false, bool deadly = false, bool freeze = false, int heroCol = -1)
	{
		if (invulnerable) yield break;

        if (team != Card.Team.Zombie && Tile.IsOnField("Binary Stars")) dmg *= 2;
        if (team == Card.Team.Plant)
		{
			Card s = Tile.IsOnField("Soul Patch");
			if (s != null)
			{
				yield return s.Glow();
				yield return s.ReceiveDamage(dmg, source);
				yield break;
			}
		}
					
        if (team == Card.Team.Zombie)
		{
            Card s = Tile.IsOnField("Undying Pharaoh");
			if (s != null)
			{
				StartCoroutine(s.Glow());
				dmg = Math.Min(dmg, HP - 1);
			}

            s = Tile.IsOnField("Planetary Gladiator");
            if (s != null)
            {
				yield return s.Glow();
                yield return s.ReceiveDamage(dmg, source);
				yield break;
            }
        }

        if (!bullseye && timesBlocked < 3)
		{
			if (dmg <= 1) block += 1;
			else if (dmg <= 3) block += 2;
			else block += 3;
			blockMeter.fillAmount = block/8f;
		}
		if (block >= 8 && !bullseye && 
            (GameManager.Instance.team == team && GameManager.Instance.GetHandCards().Count < 10 || GameManager.Instance.team != team && GameManager.Instance.opponentHandCards.childCount < 10))
		{ if (blockMeter.color != Color.yellow)
			{
                Debug.Log(GameManager.Instance.opponentHandCards.childCount);
                AudioManager.Instance.PlaySFX("Block");
				GameManager.Instance.TriggerEvent("OnBlock", this);
				blockMeter.color = Color.yellow;
				timesBlocked++;
			}
		}
		else
		{
			HP -= dmg;
			hpUI.text = Mathf.Max(0, HP) + "";
            AudioManager.Instance.PlaySFX("Hit");
            if (HP <= 0)
			{
				GameManager.Instance.GameEnded(team == Card.Team.Plant ? Card.Team.Zombie : Card.Team.Plant);
			}
			else StartCoroutine(HitVisual());

            GameManager.Instance.TriggerEvent("OnCardHurt", new Tuple<Damagable, Card, int, int>(this, source, dmg, heroCol));
        }
	}

	public void ResetBlock()
	{
        block = 0;
        blockMeter.fillAmount = 0;
        blockMeter.color = Color.white;
        if (timesBlocked == 3) eight.SetActive(false);
    }

	public override IEnumerator Heal(int amount)
	{
		if (team == Card.Team.Plant && Tile.IsOnField("Sneezing")) yield break;
		int HPBefore = HP;
		HP += amount;
		HP = Mathf.Min(maxHP, HP);
		hpUI.text = HP + "";
		if (amount > 0 && HPBefore < maxHP) GameManager.Instance.TriggerEvent("OnHeroHeal", new Tuple<Hero, int>(this, maxHP - HPBefore));
		yield return GameManager.Instance.ProcessEvents(false, true);
	}

    public override void ChangeStats(int atkAmount, int hpAmount, bool temporary = false)
    {
        maxHP += hpAmount;
    }

	public int StealBlock(int amount)
	{
		amount = Math.Min(block, amount);
        block -= amount;
		block = Math.Clamp(block, 0, 8);
        blockMeter.fillAmount = block / 8f;
		return amount;
    }

	private IEnumerator HitVisual()
	{
		SR.material.color = new Color(1, 0.8f, 0.8f, 0.8f);
		yield return new WaitForSeconds(0.1f);
		SR.material.color = Color.white;
	}

	public override bool isDamaged()
	{
		return HP < maxHP;
	}

    public override void ToggleInvulnerability(bool active)
    {
        invulnerable = active;
        if (active) SR.material.color = Color.yellow;
        else SR.material.color = Color.white;
    }

    public void ToggleTarget(bool on)
    {
        target.SetActive(on);
    }

}
