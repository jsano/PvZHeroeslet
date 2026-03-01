using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffSelection : MonoBehaviour
{

    [HideInInspector] public int ID;
    public TextMeshProUGUI buffName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI rarity;
    public GradientImage BG;
    public Transform icon;

    public static int current = -1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Buff source = AllCards.Instance.buffs[ID];
        var b = Instantiate(source, icon).GetComponent<Buff>();
        b.SetVisualMode();
        buffName.text = source.name;
        description.text = source.description;
        rarity.text = source.rarity.ToString();

        BG.Color1 = Buff.classColors[source.buffClass];
        if (source.rarity == Buff.Rarity.Duo) BG.Color2 = Buff.classColors[source.duoSecondClass];
        else BG.Color2 = BG.Color1;

        BG.Color1 = Color.Lerp(BG.Color1, Color.white, 0.3f);
        BG.Color2 = Color.Lerp(BG.Color2, Color.white, 0.3f);
    }

    public void Select()
    {
        foreach (Transform t in GameManager.Instance.buffList) t.GetComponent<Image>().color = Color.black;
        GameManager.Instance.lockInButton.interactable = true;
        GetComponent<Image>().color = Color.red;
        current = ID;
    }

}
