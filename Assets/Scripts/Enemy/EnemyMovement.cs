using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] BoxCollider2D wallCollider;
    bool isCapsized;
    bool isStunned;
    CircleCollider2D myCollider;
    BoxCollider2D myFeetCollider;
    bool feetTouchingFloor;
    bool bodyTouchingFloor;
    Animator animator;
    [SerializeField] float torqueAmount = 6f;
    float initialDirection;

    private void Awake()
    {
        myFeetCollider = GetComponent<BoxCollider2D>();
        myCollider = GetComponent<CircleCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initialDirection = transform.localScale.x;
    }


    private void Update()
    {
        IsFlipped();
        IsTouchingFloor();

        if (isStunned) {
            animator.SetBool("IsRunning", false);
            return; }
        if (feetTouchingFloor && !isCapsized)
        {
            animator.SetBool("IsRunning", true);
            myRigidBody.linearVelocity = new Vector3( Vector3.MoveTowards(myRigidBody.linearVelocity, new Vector3(moveSpeed, 0, 0), 10).x * initialDirection, myRigidBody.linearVelocityY, 0);

        }
        else if (bodyTouchingFloor || feetTouchingFloor)
        {
            myRigidBody.AddTorque(-torqueAmount * Mathf.Sign(transform.rotation.z));
            animator.SetBool("IsRunning", false);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        transform.localScale = new Vector3(-transform.localScale.x , transform.localScale.y, transform.localScale.z);
    }

    void IsFlipped()
    {
        isCapsized = (Mathf.Abs(transform.rotation.z) > 0.05);
    }

    void IsTouchingFloor()
    {
        feetTouchingFloor = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bodyTouchingFloor = myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    public void Stun()
    {
        isStunned = true;
        StartCoroutine(DeStun());
    }

    IEnumerator DeStun()
    {
        yield return new WaitForSeconds(1.2f);
        isStunned = false;
    }

    public void SetLayerOnDeath()
    {
        isStunned = true;
        animator.enabled = false;
        this.gameObject.layer = LayerMask.NameToLayer("Dead Enemy");
    }
}
