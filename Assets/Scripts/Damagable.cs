using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Damagable : MonoBehaviour
{
	public abstract int ReceiveDamage(int dmg, bool bullseye = false, bool deadly = false);

	public abstract void Heal(int amount, bool raiseCap);

	public abstract bool isDamaged();

}
