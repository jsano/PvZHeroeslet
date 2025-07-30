using Unity.Services.Samples.Friends;
using UnityEngine;

public class RelationshipsOpener : MonoBehaviour
{

    void Start()
    {
        
    }

    public void Open()
    {
        RelationshipsManager.Instance.transform.GetChild(0).gameObject.SetActive(true);
    }

}
