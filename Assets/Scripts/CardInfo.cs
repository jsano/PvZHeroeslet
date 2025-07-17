using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

		Card baseCard = AllCards.InstanceToPrefab(source);
		image.sprite = baseCard.GetComponent<SpriteRenderer>().sprite;
		atk.text = baseCard.atk + "";
		HP.text = baseCard.HP + "";
        if (baseCard.type != Card.Type.Unit)
		{
			atk.transform.parent.gameObject.SetActive(false);
			HP.transform.parent.gameObject.SetActive(false);
		}
		else
		{
            atk.transform.parent.gameObject.SetActive(true);
            HP.transform.parent.gameObject.SetActive(true);
			atk.GetComponentInParent<Image>().sprite = baseCard.GetAttackIcon();
			HP.GetComponentInParent<Image>().sprite = baseCard.GetHPIcon();
        }
		cost.text = baseCard.cost + "";
        if (baseCard.team == Card.Team.Zombie) cost.GetComponentInParent<Image>().sprite = AllCards.Instance.brainUI;
        cardClass.text = Enum.GetName(typeof(Card.Class), baseCard._class);
		cardName.text = baseCard.name;

		tribes.text = "";
		foreach (Card.Tribe t in baseCard.tribes)
		{
			tribes.text += Enum.GetName(typeof(Card.Tribe), t) + " ";
		}
		if (baseCard.type == Card.Type.Trick) tribes.text += "Trick";
        else if (baseCard.type == Card.Type.Terrain) tribes.text += "Terrain";
        else tribes.text += baseCard.team == Card.Team.Plant ? "Plant" : "Zombie";

		description.text = "";
		if (baseCard.amphibious) description.text += "Amphibious\n";
		if (baseCard.antihero > 0) description.text += "Anti-hero " + baseCard.antihero + "\n";
		if (baseCard.armor > 0) description.text += "Armor " + baseCard.armor + "\n";
		if (baseCard.bullseye > 0) description.text += "Bullseye\n";
		if (baseCard.deadly > 0) description.text += "Deadly\n";
		if (baseCard.doubleStrike > 0) description.text += "Double Strike\n";
		if (baseCard.frenzy > 0) description.text += "Frenzy\n";
		if (baseCard.gravestone) description.text += "Gravestone\n";
		if (baseCard.hunt > 0) description.text += "Hunt\n";
		if (baseCard.overshoot > 0) description.text += "Overshoot " + baseCard.overshoot + "\n";
		if (baseCard.splash > 0) description.text += "Splash Damage " + baseCard.splash + "\n";
		if (baseCard.strikethrough > 0) description.text += "Strikethrough\n";
		if (baseCard.teamUp) description.text += "Team Up\n";
		if (baseCard.untrickable > 0) description.text += "Untrickable\n";
		description.text += baseCard.description;
		FormatDescriptionForTooltip();

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

    private Dictionary<string, string> descriptions = new()
	{
		{ "Amphibious", "Can be placed in water (lane 5)" },
		{ "Anti-hero", "Increases attack when targeting the hero" },
		{ "Armor", "Reduces damage taken" },
		//{ "Bonus Attack", "Does an extra attack right then" },
		{ "Bounce", "Return the card to the user's hand" },
		{ "Bullseye", "Doesn't charge the opponent's block meter" },
        { "Conjure", "Gain a card from the game into your hand" },
        { "Deadly", "Destroys any card it deals damage to,\nregardless of its remaining HP" },
		{ "Dino-Roar", "Activates when the player gains a card" },
		{ "Double Strike", "Does a bonus attack after its combat" },
		{ "Evolution", "Play this over a card to use this ability" },
		{ "Freeze", "Cannot attack during its combat,\nand wears off afterwards" },
		{ "Frenzy", "When this attacks, kills its target,\nand survives, it does a bonus attack" },
        { "Fusion", "Play a card over this to use this ability" },
        { "Gravestone", "Hides its identity to the opponent\nuntil it's time for Zombie Tricks" },
		{ "Hunt", "When an opponent card is played,\nthis moves to that lane (if possible)" },
        { "Overshoot", "Before its combat, do damage\nto the opponent hero" },
        { "Splash Damage", "Attacks any opponent cards next door" },
        { "Strikethrough", "Attacks all targets in lane and the hero" },
		{ "Team Up", "Can be played on a lane that\nalready contains a card" },
        { "Untrickable", "Unaffected by the opponent's tricks" },
    };

    public GameObject tooltipContainer;

    private int _currentlyActiveLinkedElement;

    public delegate void HoverOnLinkEvent(string keyword, Vector3 mousePos);
    public static event HoverOnLinkEvent OnHoverOnLinkEvent;

    public delegate void CloseTooltipEvent();
    public static event CloseTooltipEvent OnCloseTooltipEvent;

    void Update()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		bool isIntersectingRectTransform = TMP_TextUtilities.IsIntersectingRectTransform(description.GetComponent<RectTransform>(), mousePosition, null);
        if (!isIntersectingRectTransform) return;
        int intersectingLink = TMP_TextUtilities.FindIntersectingLink(description, mousePosition, null);
        if (_currentlyActiveLinkedElement != intersectingLink) OnCloseTooltipEvent?.Invoke();
        if (intersectingLink == -1) return;

        TMP_LinkInfo linkInfo = description.textInfo.linkInfo[intersectingLink];

        OnHoverOnLinkEvent?.Invoke(linkInfo.GetLinkID(), mousePosition);
        _currentlyActiveLinkedElement = intersectingLink;
    }

	private void OnEnable()
    {
        OnHoverOnLinkEvent += GetTooltipInfo;
        OnCloseTooltipEvent += CloseTooltip;
    }

    private void OnDisable()
    {
        OnHoverOnLinkEvent -= GetTooltipInfo;
        OnCloseTooltipEvent -= CloseTooltip;
    }

    private void GetTooltipInfo(string keyword, Vector3 mousePos)
    {
        
        if (!tooltipContainer.activeInHierarchy)
        {
            tooltipContainer.transform.position = mousePos + new Vector3(0, 60, 0);
			var pos = Math.Clamp(tooltipContainer.transform.localPosition.x, -50, 50);
			tooltipContainer.transform.localPosition = new Vector2(pos, Math.Max(-50, tooltipContainer.transform.localPosition.y));
            tooltipContainer.SetActive(true);
        }

        tooltipContainer.GetComponentInChildren<TextMeshProUGUI>().text = descriptions[keyword];
    }

    public void CloseTooltip()
    {
        if (tooltipContainer.activeInHierarchy) tooltipContainer.SetActive(false);
    }

    public void FormatDescriptionForTooltip()
    {
        var escapedWords = descriptions.Keys;
        string pattern = @"(?:" + string.Join("|", escapedWords) + @")";

        // Get all matches first
        MatchCollection matches = Regex.Matches(description.text, pattern);

		// Process matches from right to left to avoid index shifting issues
		string currentText = description.text;
        for (int i = matches.Count - 1; i >= 0; i--)
        {
            Match match = matches[i];
            string wordFound = match.Value;
            int position = match.Index;
			description.text = description.text.Substring(0, position) + "<link=\"" + wordFound + "\"><color=#00aaff>" + 
							description.text.Substring(position, wordFound.Length) + 
							"</color></link>" + description.text.Substring(position + wordFound.Length);
        }
    }

}
