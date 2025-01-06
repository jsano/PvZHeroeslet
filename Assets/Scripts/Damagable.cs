using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Damagable : NetworkBehaviour
{
	public abstract int ReceiveDamage(int dmg, bool bullseye = false);

	public abstract void Heal(int amount, bool raiseCap);

	public abstract bool isDamaged();

}
