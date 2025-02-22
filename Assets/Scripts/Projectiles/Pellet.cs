using UnityEngine;


public class Pellet : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] GameObject bulletImpact;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            collision.collider.GetComponent<Health>().DecreaseHealthPoints(damage);
        }
        float direction = Mathf.Sign(GetComponent<Rigidbody2D>().linearVelocityX);
        Instantiate(bulletImpact, transform.position, Quaternion.Euler(0,0, 90* (direction-1) ));
        Destroy(gameObject);
    }
}
