using UnityEngine;

public class AttackFX : MonoBehaviour
{

    [HideInInspector] public Transform destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.up = destination.position - transform.position;
        LeanTween.move(gameObject, destination, 0.25f).setOnComplete(() => Destroy(gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
