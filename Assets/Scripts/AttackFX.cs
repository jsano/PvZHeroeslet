using UnityEngine;

public class AttackFX : MonoBehaviour
{

    [HideInInspector] public Transform destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.up = destination.position - transform.position;
        Vector3 destination1 = destination.position;
        if (destination.GetComponent<Hero>() != null)
        {
            destination1 = new Vector3(transform.position.x, destination.position.y, destination.position.z);
        }
        LeanTween.move(gameObject, destination1, 0.25f).setOnComplete(() => Destroy(gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
