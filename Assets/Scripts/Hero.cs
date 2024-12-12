using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hero : Damagable
{

    public int HP = 20;
	private TextMeshProUGUI hpUI;
	private SpriteRenderer SR;

	// Start is called before the first frame update
	void Start()
    {
		SR = GetComponent<SpriteRenderer>();
		hpUI = transform.Find("HP").GetComponent<TextMeshProUGUI>();
		hpUI.text = HP + "";
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public override int ReceiveDamage(int dmg)
	{
		HP -= dmg;
		hpUI.text = Mathf.Max(0, HP) + "";
		StartCoroutine(HitVisual());
		return dmg;
	}

	private IEnumerator HitVisual()
	{
		SR.material.color = new Color(1, 0.8f, 0.8f, 0.8f);
		yield return new WaitForSeconds(0.1f);
		SR.material.color = Color.white;
	}

}
