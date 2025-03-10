using UnityEngine;

public class LobbyManager : MonoBehaviour
{

    public GameObject loading;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.rotateAroundLocal(loading, Vector3.forward, -360f, 2f).setRepeat(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
