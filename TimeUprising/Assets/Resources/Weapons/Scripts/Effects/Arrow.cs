using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    private float speed = 100f;
    private Vector3 destination;
    
    // Use this for initialization
    void Start ()
    {
        
    }
    
    void OnTriggerEnter2D (Collider2D other)
    {
        if (collider2D.enabled == false)
            return;
        
        // TODO replace with EnemyUnit base type
        if (other.gameObject.name == "Border") {
            Destroy (this.gameObject);
            Destroy (this);
        }
    }
    
    void FixedUpdate ()
    {
        if (Vector3.Distance (transform.position, destination) < 1.0f) {
            Destroy (this.gameObject);
            Destroy (this);
        }
        
        transform.position += speed * Time.deltaTime * transform.up;
    }
    
    public void SetDestination (Vector3 destination)
    {
        this.destination = destination;
        
        Vector3 toTarget = destination - transform.position;
        transform.up = toTarget.normalized;
    }
}
