using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffInfo : CardInfo
{

    public void Show(Buff source)
    {
        if (isActiveAndEnabled) return;
        transform.parent.gameObject.SetActive(true);

        image.sprite = source.image.sprite;
        
        cardClass.text = Enum.GetName(typeof(Card.Class), source.buffClass);
        cardName.text = source.name;
        tribes.text = Enum.GetName(typeof(Buff.Rarity), source.rarity);
        description.text = source.description;
        FormatDescriptionForTooltip();

        lore.text = source.lore;
    }

}
