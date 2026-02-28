using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffSelection : MonoBehaviour
{

    [HideInInspector] public int ID;
    public TextMeshProUGUI buffName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI rarity;
    public Image image;
    public GradientImage BG;

    public static int current = -1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Buff source = AllCards.Instance.buffs[ID]; Debug.Log(ID + "    " + source);
        image.sprite = source.GetImage();
        image.color = Color.Lerp(Buff.classColors[source.buffClass], Color.white, 0.5f);
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
