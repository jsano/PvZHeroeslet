using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Hero : NetworkBehaviour
{

    public int HP = 20;
	private TextMeshProUGUI hpUI;

	// Start is called before the first frame update
	void Start()
    {
		hpUI = transform.Find("HP").GetComponent<TextMeshProUGUI>();
		hpUI.text = HP + "";
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ReceiveDamage(int dmg)
	{
		HP -= dmg;
		hpUI.text = Mathf.Max(0, HP) + "";
	}

}
