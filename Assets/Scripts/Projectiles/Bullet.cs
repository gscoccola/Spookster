using UnityEngine;


public class Bullet : MonoBehaviour
{

    [SerializeField] int damage = 50;
    [SerializeField] GameObject bulletImpact;
    bool hasSpeed;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<Health>().DecreaseHealthPoints(damage);
        } 
        else if (collision.collider.tag == "EnemyWeakpoint")
        {
            collision.collider.GetComponent<BanditHead>().DecreaseHealthPoints(damage);
        }
        float direction = Mathf.Sign(GetComponent<Rigidbody2D>().linearVelocityX);
        Instantiate(bulletImpact, transform.position, Quaternion.Euler(0, 0, 90 * (direction - 1)));
        Destroy(gameObject);
    }
}
