using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TapSoundingButton : MonoBehaviour, IPointerUpHandler
{

    private Selectable button;

    void Start()
    {
        button = GetComponent<Selectable>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable) AudioManager.Instance.PlaySFX("Tap");
    }

}
