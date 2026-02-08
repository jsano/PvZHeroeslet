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
        Buff baseBuff = AllCards.InstanceToPrefab(source);

        image.sprite = baseBuff.image.sprite;
        
        cardClass.text = Enum.GetName(typeof(Card.Class), baseBuff.buffClass);
        cardName.text = baseBuff.name;
        tribes.text = Enum.GetName(typeof(Buff.Rarity), baseBuff.rarity);
        description.text = baseBuff.description;
        FormatDescriptionForTooltip();

        lore.text = baseBuff.lore;
    }

}
