using System;
using System.Collections;
using System.Collections.Generic;
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

	// Start is called before the first frame update
	void Start()
    {
        target = transform.Find("Target").gameObject;
        SR = GetComponent<SpriteRenderer>();
		hpUI.text = HP + "";
		maxHP = HP;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public override IEnumerator ReceiveDamage(int dmg, Card source, bool bullseye = false, bool deadly = false, bool freeze = false, int heroCol = -1)
	{
		if (invulnerable) yield break;

		if (team == Card.Team.Plant)
			for (int col = 0; col < 5; col++)
			{
				for (int row = 0; row < 2; row++)
				{
					if (Tile.plantTiles[row, col].planted != null && Tile.plantTiles[row, col].planted.name.Contains("Soul Patch"))
					{
                        yield return new WaitForSeconds(0.5f);
                        yield return Tile.plantTiles[row, col].planted.ReceiveDamage(dmg, source);
					}
				}
			}
        if (team == Card.Team.Zombie)
            for (int col = 0; col < 5; col++)
            {
                if (Tile.zombieTiles[0, col].planted != null && Tile.zombieTiles[0, col].planted.name.Contains("Undying Pharaoh"))
                {
					dmg = Math.Min(dmg, HP - 1);
                }
            }

        if (!bullseye)
		{
			if (dmg <= 1) block += 1;
			else if (dmg <= 3) block += 2;
			else block += 3;
			blockMeter.fillAmount = block/8f;
		}
		if (block >= 8 && !bullseye)
		{
            GameManager.Instance.TriggerEvent("OnBlock", this);
            block = 0;
			blockMeter.fillAmount = 0;
		}
		else
		{
			HP -= dmg;
			hpUI.text = Mathf.Max(0, HP) + "";
			if (HP <= 0)
			{
				Debug.Log("DEAD");
			}
			else StartCoroutine(HitVisual());

            GameManager.Instance.TriggerEvent("OnCardHurt", new Tuple<Damagable, Card, int, int>(this, source, dmg, heroCol));
        }
	}

	public override void Heal(int amount, bool raiseCap=false)
	{
		HP += amount;
		if (raiseCap) maxHP += amount;
		else HP = Mathf.Min(maxHP, HP);
		hpUI.text = HP + "";
		GameManager.Instance.TriggerEvent("OnHeal", this);
	}

	public int StealBlock(int amount)
	{
		amount = Math.Min(block, amount);
        block -= amount;
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
