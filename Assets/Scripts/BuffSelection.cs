using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffSelection : MonoBehaviour
{

    [HideInInspector] public int ID;
    public TextMeshProUGUI buffName;
    public TextMeshProUGUI description;
    public Image image;
    public GradientImage BG;

    public static int current = -1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Buff source = AllCards.Instance.buffs[ID];
        image.sprite = source.GetImage();
        buffName.text = source.name;
        description.text = source.description;

        BG.Color1 = source.buffClass switch
        {
            Card.Class.Elation => Color.yellow,
            Card.Class.Misery => Color.blue,
            Card.Class.Wrath => Color.red,
            Card.Class.Fright => Color.magenta + Color.blue * 0.5f,
            Card.Class.Awe => Color.red + Color.cyan * 0.5f,
            Card.Class.Contempt => Color.green,
            _ => Color.white,
        };
        if (source.rarity == Buff.Rarity.Duo)
        {
            BG.Color2 = source.duoSecondClass switch
            {
                Card.Class.Elation => Color.yellow,
                Card.Class.Misery => Color.blue,
                Card.Class.Wrath => Color.red,
                Card.Class.Fright => Color.magenta + Color.blue * 0.5f,
                Card.Class.Awe => Color.red + Color.cyan * 0.5f,
                Card.Class.Contempt => Color.green,
                _ => Color.white,
            };
        }
        else BG.Color2 = BG.Color1;

        BG.Color1 += Color.white * 0.35f;
        BG.Color2 += Color.white * 0.35f;
    }

    public void Select()
    {
        foreach (Transform t in GameManager.Instance.buffList) t.GetComponent<Image>().color = Color.black;
        GameManager.Instance.lockInButton.interactable = true;
        GetComponent<Image>().color = Color.red;
        current = ID;
    }

}
