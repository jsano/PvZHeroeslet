using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{

	public Image image;
	public TextMeshProUGUI atk;
	public TextMeshProUGUI HP;
	public TextMeshProUGUI cost;
	public TextMeshProUGUI cardClass;
	public TextMeshProUGUI cardName;
	public TextMeshProUGUI tribes;
	public TextMeshProUGUI description;

	public Button exit;

	public IEnumerator Show(Card source)
	{
		if (isActiveAndEnabled) yield break;
		yield return null;
		transform.parent.gameObject.SetActive(true);
		Card baseCard = source;
		foreach (Card c in AllCards.Instance.cards)
		{
			if (source.name == c.name || source.name.Substring(0, source.name.IndexOf("(")) == c.name)
			{
				baseCard = c;
				break;
			}
		}

		image.sprite = baseCard.GetComponent<SpriteRenderer>().sprite;
		atk.text = baseCard.atk + "";
		HP.text = baseCard.HP + "";
		if (baseCard.type == Card.Type.Trick)
		{
			atk.text = "";
			HP.text = "";
		}
		cost.text = baseCard.cost + "";
		cardClass.text = Enum.GetName(typeof(Card.Class), baseCard._class);
		cardName.text = baseCard.name;

		tribes.text = "";
		foreach (Card.Tribe t in baseCard.tribes)
		{
			tribes.text += Enum.GetName(typeof(Card.Tribe), t) + " ";
		}
		if (baseCard.type == Card.Type.Trick) tribes.text += "Trick";
		else tribes.text += baseCard.team == Card.Team.Plant ? "Plant" : "Zombie";

		description.text = "";
		if (baseCard.amphibious) description.text += "Amphibious\n";
		if (baseCard.antihero > 0) description.text += "Anti-hero " + baseCard.antihero + "\n";
		if (baseCard.armor > 0) description.text += "Armor " + baseCard.armor + "\n";
		if (baseCard.bullseye) description.text += "Bullseye\n";
		if (baseCard.deadly) description.text += "Deadly\n";
		if (baseCard.doubleStrike) description.text += "Double Strike\n";
		if (baseCard.frenzy) description.text += "Frenzy\n";
		if (baseCard.gravestone) description.text += "Gravestone\n";
		if (baseCard.hunt) description.text += "Hunt\n";
		if (baseCard.overshoot > 0) description.text += "Overshoot " + baseCard.overshoot + "\n";
		if (baseCard.splash > 0) description.text += "Splash Damage " + baseCard.splash + "\n";
		if (baseCard.strikethrough) description.text += "Strikethrough\n";
		if (baseCard.teamUp) description.text += "Team Up\n";
		if (baseCard.untrickable) description.text += "Untrickable\n";
		description.text += baseCard.description;
	}

	/*public void Hide()
	{
		exit.interactable = false;
		transform.parent.gameObject.SetActive(false);
	}*/

}
