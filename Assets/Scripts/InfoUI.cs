using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class InfoUI : MonoBehaviour
{

    public TextMeshProUGUI description;
    public Button exit;

    protected int ExtractValue(string s)
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

    void Start()
    {
        exit.onClick.Invoke();
    }

}
