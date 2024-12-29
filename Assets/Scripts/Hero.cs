using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Damagable
{

    public int HP = 20;
	private int maxHP;
	public TextMeshProUGUI hpUI;
	private SpriteRenderer SR;
	public Image blockMeter;
	private int block;

	// Start is called before the first frame update
	void Start()
    {
		SR = GetComponent<SpriteRenderer>();
		hpUI.text = HP + "";
		maxHP = HP;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public override int ReceiveDamage(int dmg)
	{
		if (dmg == 1) block += 1;
		else if (dmg <= 3) block += 2;
		else block += 3;
		blockMeter.fillAmount = block/8f;
		if (block >= 8)
		{
			Debug.Log("BLOCK");
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
		}
		return dmg;
	}

	public override void Heal(int amount, bool raiseCap)
	{
		HP += amount;
		if (raiseCap) maxHP += amount;
		else HP = Mathf.Min(maxHP, HP);
		hpUI.text = HP + "";
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

}
