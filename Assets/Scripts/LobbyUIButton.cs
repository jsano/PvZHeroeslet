using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyUIButton : MonoBehaviour, IPointerEnterHandler
{

    public bool selected { get; private set; }
    public bool disabled { get; private set; }
    public int ID;

    private LobbyManager LM;

    void Start()
    {
        LM = FindFirstObjectByType<LobbyManager>();
        transform.Find("Image").GetComponent<Image>().sprite = AllCards.Instance.heroes[ID].GetComponent<SpriteRenderer>().sprite;
    }

    public void Toggle()
    {
        selected = !selected;
        if (selected) GetComponent<Image>().color = new Color(1f, 1f, 0.8f);
        else GetComponent<Image>().color = Color.white;
    }

    public void Disable()
    {
        disabled = true;
        GetComponent<Button>().interactable = false;
        GetComponent<Image>().color = new Color(1f, 0.8f, 0.8f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LM.heroName.text = AllCards.Instance.heroes[ID].name;
    }
}
