using UnityEngine;

public class Bomb : MonoBehaviour {
    public float ThresholdForce = 2;
    public GameObject ExplosionPrefab;

    public void Destruct() 
    {
        Destroy(this.gameObject);
    }
    public void Boom()
    {
        GetComponent<PointEffector2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = false;
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity, transform.parent);
        Invoke("Destruct", .1f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var body = collision.gameObject.GetComponent<Rigidbody2D>();
        if (body != null)
        {
            if (body.velocity.magnitude >= ThresholdForce)
            {
                Boom();
            }
        }
    }
}
