using System.Collections;
using UnityEngine;

public class BlankBlast : MonoBehaviour
{
    [SerializeField] float boostSpead = 10f;
    
    void Start()
    {

        StartCoroutine(SelfDelete());
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy" || collider.tag ==  "Hitbox" || collider.tag ==  "Stunned Enemy")
        {
            float angle = transform.rotation.eulerAngles.z * Mathf.PI / 180;
            collider.GetComponent<Rigidbody2D>().linearVelocityX += boostSpead*(Mathf.Cos(angle)) *Time.deltaTime;
            collider.GetComponent<Rigidbody2D>().linearVelocityY += boostSpead * (Mathf.Sin(angle)) *Time.deltaTime;
            
            if (collider.TryGetComponent<EnemyMovement>(out EnemyMovement enemyMovement))
            {
                enemyMovement.Stun();
            }

            if (collider.TryGetComponent<BanditMovement>(out BanditMovement banditMovement))
            {
                banditMovement.Parry();
            }
            
        }
    }

    IEnumerator SelfDelete()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
