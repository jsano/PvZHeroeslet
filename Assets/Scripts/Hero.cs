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
	public Sprite sixImage;
	[HideInInspector] public int blockActivationLimit;
    [HideInInspector] public int segmentsToActivation = 8;

    public Transform thinking;

	// Start is called before the first frame update
	void Start()
    {
        target = transform.Find("Target").gameObject;
        SR = GetComponent<SpriteRenderer>();
		hpUI.text = HP + "";
		maxHP = HP;

		for (int i = 0; i < 3; i++) StartCoroutine(DotAnimation(i));
		blockActivationLimit = 3;
	}

    void Update()
    {
		if (timesBlocked >= blockActivationLimit)
		{
			eight.SetActive(false);
			ResetBlock();
		}
		else
		{
			blockMeter.fillAmount = block / (1f * segmentsToActivation);
			if (segmentsToActivation == 6) eight.GetComponent<Image>().sprite = sixImage;
		}
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
		if (invulnerable == 1) yield break;
		if (invulnerable == 0.5f)
		{
			ToggleInvulnerability(false);
			yield break;
		}
        int change = 0;
        foreach (int a in Buff.CallAllImmediate("CardHurtModifiers", new Tuple<Damagable, Card, int>(this, source, dmg))) change += a;
		dmg += change;
        foreach (int a in Buff.CallAllImmediate("OnCardHurtImmediate", new Tuple<Damagable, Card, int>(this, source, dmg))) dmg += a;
        if (team != Card.Team.B && Tile.IsOnField("Binary Stars")) dmg *= 2;
        if (team == Card.Team.A)
		{
			Card s = Tile.IsOnField("Soul Patch");
			if (s != null)
			{
				yield return s.Glow();
				yield return s.ReceiveDamage(dmg, source);
				yield break;
			}
		}
					
        if (team == Card.Team.B)
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

		if (dmg >= 5 && Buff.PlayerHasBuff("Panic Reflex", team)) block += 10;

        if (!bullseye && timesBlocked < blockActivationLimit)
		{
			if (dmg <= 0) yield break;
            else if (dmg <= 1) block += 1;
			else if (dmg <= 3) block += 2;
			else block += 3;
		}

		int max = Buff.PlayerHasBuff("Suffocating Limits", Card.GetOpponent(team)) ? 8 : 10;
        if (block >= segmentsToActivation && !bullseye && 
            (GameManager.Instance.team == team && GameManager.Instance.GetHandCards().Count < max || GameManager.Instance.team != team && GameManager.Instance.opponentHandCards.childCount < max))
		{ if (blockMeter.color != Color.yellow)
			{
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
				GameManager.Instance.GameEnded(Card.GetOpponent(team));
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
    }

	public override IEnumerator Heal(int amount)
	{
		if (team == Card.Team.A && Tile.IsOnField("Sneezing")) yield break;
		foreach (int a in Buff.CallAllImmediate("OnHeroHealImmediate", new Tuple<Hero, int>(this, amount)))
		{
			amount += a;
		}
        int HPBefore = HP;
		HP += amount;
		HP = Mathf.Min(maxHP, HP);
		hpUI.text = HP + "";
		if (amount > 0 && HPBefore < HP) GameManager.Instance.TriggerEvent("OnHeroHeal", new Tuple<Hero, int>(this, HP - HPBefore));
		yield return GameManager.Instance.ProcessEvents(false, true);
	}

	/// <summary>
	/// Unlike in <c>Card</c>, <c>hpAmount</c> merely raises the HP cap without affecting HP, unless <c>temporary</c> is true
	/// </summary>
	/// <param name="atkAmount"></param>
	/// <param name="hpAmount"></param>
	/// <param name="temporary"></param>
	/// <param name="silent"></param>
    public override void ChangeStats(int atkAmount, int hpAmount, bool temporary = false, bool silent = false)
    {
        maxHP += hpAmount;
		if (temporary)
		{
            HP += hpAmount;
            HP = Mathf.Min(maxHP, HP);
            hpUI.text = HP + "";
        }
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

    public override void ToggleInvulnerability(bool active, bool oneTime = false)
    {
        if (oneTime) invulnerable = active ? 0.5f : 0;
        else invulnerable = active ? 1 : 0;
        if (active) SR.material.color = Color.yellow;
        else SR.material.color = Color.white;
    }

    public void ToggleTarget(bool on)
    {
        target.SetActive(on);
    }

}
