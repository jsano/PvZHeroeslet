using UnityEngine;

public class AttackFX : MonoBehaviour
{

    [HideInInspector] public Transform destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.up = destination.position - transform.position;
        LeanTween.move(gameObject, destination, 0.25f).setOnComplete(OnComplete);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnComplete()
    {
        Destroy(gameObject);
    }

}
