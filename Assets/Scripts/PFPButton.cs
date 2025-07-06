using UnityEngine;
using UnityEngine.UI;

public class PFPButton : MonoBehaviour
{

    public int ID;
    public Image image;

    void Start()
    {
        image.sprite = ProfileThumbnail.ProfilePictureIDToSprite(ID);
    }

    public void Toggle(bool on)
    {
        if (on)
        {
            foreach (Transform t in transform.parent) t.GetComponent<PFPButton>().Toggle(false);
            GetComponent<Image>().color = new Color(1f, 1f, 0.8f);
            ProfileInfo.chosenPFP = ID;
        }
        else GetComponent<Image>().color = Color.white;
    }

}
