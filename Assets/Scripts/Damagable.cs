using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Damagable : MonoBehaviour
{

	protected bool invulnerable = false;

	public abstract IEnumerator ReceiveDamage(int dmg, Card source, bool bullseye = false, bool deadly = false, bool freeze = false, int heroCol = -1);

	public abstract void Heal(int amount, bool raiseCap);

	public abstract bool isDamaged();

	public abstract void ToggleInvulnerability(bool active);

}
