using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffSelection : MonoBehaviour
{

    [HideInInspector] public int ID;
    public TextMeshProUGUI buffName;
    public TextMeshProUGUI description;
    public Image image;
    public Image BG;

    public static int current = -1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Buff source = AllCards.Instance.buffs[ID];
        image.sprite = source.GetImage();
        buffName.text = source.name;
        description.text = source.description;

        switch (source.buffClass)
        {
            case Card.Class.Elation:
                BG.color = Color.yellow;
                break;
            case Card.Class.Misery:
                BG.color = Color.blue;
                break;
            case Card.Class.Wrath:
                BG.color = Color.red;
                break;
            case Card.Class.Fright:
                BG.color = Color.magenta + Color.blue * 0.5f;
                break;
            case Card.Class.Awe:
                BG.color = Color.red + Color.cyan * 0.5f;
                break;
            case Card.Class.Contempt:
                BG.color = Color.green;
                break;
            default:
                BG.color = Color.white;
                break;
        }
        BG.color += Color.white * 0.35f;
    }

    public void Select()
    {
        foreach (Transform t in GameManager.Instance.buffList) t.GetComponent<Image>().color = Color.black;
        GameManager.Instance.lockInButton.interactable = true;
        GetComponent<Image>().color = Color.red;
        current = ID;
    }

}
