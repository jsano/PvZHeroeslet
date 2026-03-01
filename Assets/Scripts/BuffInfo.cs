using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffInfo : InfoUI
{

    public Transform icon;
    public TextMeshProUGUI cardClass;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI tribes;
    public TextMeshProUGUI lore;

    private static BuffInfo instance;
    public static BuffInfo Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    public void Show(Buff source)
    {
        if (isActiveAndEnabled) return;
        transform.parent.gameObject.SetActive(true);
        Buff baseBuff = AllCards.InstanceToPrefab(source);

        var b = Instantiate(source, icon).GetComponent<Buff>();
        b.SetVisualMode();

        cardClass.text = Enum.GetName(typeof(Card.Class), baseBuff.buffClass);
        if (baseBuff.rarity == Buff.Rarity.Duo) cardClass.text += "\n" + Enum.GetName(typeof(Card.Class), baseBuff.duoSecondClass);
        cardName.text = baseBuff.name;
        tribes.text = Enum.GetName(typeof(Buff.Rarity), baseBuff.rarity);
        description.text = baseBuff.description;
        FormatDescriptionForTooltip();

        lore.text = baseBuff.lore;
    }

}
