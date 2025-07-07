using Unity.Services.Samples.Friends;
using UnityEngine;

public class RelationshipsOpener : MonoBehaviour
{

    private RelationshipsManager RM;

    void Start()
    {
        RM = FindAnyObjectByType<RelationshipsManager>();
    }

    public void Open()
    {
        
        if (RM != null) RM.transform.GetChild(0).gameObject.SetActive(true);
    }

}
