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
    public TextMeshProUGUI gained;
	public TextMeshProUGUI lore;

    public Button exit;

	public void Show(Card source, FinalStats fs = null)
	{
        if (isActiveAndEnabled) return;
		transform.parent.gameObject.SetActive(true);

		Card baseCard = source;
        if (source.name.IndexOf("(") >= 0)
            foreach (Card c in AllCards.Instance.cards)
            {
                if (source.name.Substring(0, source.name.IndexOf("(")) == c.name)
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

		gained.text = "";
		if (source.sourceFS != null || fs != null)
		{
			FinalStats fs1 = fs == null ? source.sourceFS : fs;
			if (fs1.atk != baseCard.atk) gained.text += "Gained " + (fs1.atk - baseCard.atk) + " attack\n";
			if (fs1.hp != baseCard.HP) gained.text += "Gained " + (fs1.hp - baseCard.HP) + " HP\n";
			string[] abilities = fs1.abilities.Split(" - ", StringSplitOptions.RemoveEmptyEntries);
			foreach (string s in abilities)
			{
				if (s.Contains("amphibious")) gained.text += "Gained Amphibious\n";
				if (s.Contains("antihero")) gained.text += "Gained Anti-hero " + ExtractValue(s) + "\n";
				if (s.Contains("armor")) gained.text += "Gained Armor " + ExtractValue(s) + "\n";
				if (s.Contains("bullseye")) gained.text += "Gained Bullseye\n";
				if (s.Contains("deadly")) gained.text += "Gained Deadly\n";
				if (s.Contains("doubleStrike")) gained.text += "Gained Double Strike\n";
				if (s.Contains("frenzy")) gained.text += "Gained Frenzy\n";
				if (s.Contains("gravestone")) gained.text += "Gained Gravestone\n";
				if (s.Contains("hunt")) gained.text += "Gained Hunt\n";
				if (s.Contains("overshoot")) gained.text += "Gained Overshoot " + ExtractValue(s) + "\n";
				if (s.Contains("splash")) gained.text += "Gained Splash Damage " + ExtractValue(s) + "\n";
				if (s.Contains("strikethrough")) gained.text += "Gained Strikethrough\n";
				if (s.Contains("teamUp")) gained.text += "Gained Team Up\n";
				if (s.Contains("untrickable")) gained.text += "Gained Untrickable\n";
			}
		}

		lore.text = "";
		lore.text += baseCard.lore;
    }

    private int ExtractValue(string s)
	{
        string value = "";
        for (int i = 0; i < s.Length; i++) if (char.IsDigit(s[i])) value += s[i];
		return int.Parse(value);
    }

	/*public void Hide()
	{
		exit.interactable = false;
		transform.parent.gameObject.SetActive(false);
	}*/

}
