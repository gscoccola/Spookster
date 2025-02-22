using System.Collections;
using Animancer;
using UnityEngine;
using UnityEngine.Windows;

public class BanditMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 4.0f;
    [SerializeField] float boostSpeed = 8.0f;
    [SerializeField] float detectDistance = 5f;
    [SerializeField] float attackDistance = 1f;
    [SerializeField] Transform playerTransform;
    [SerializeField] float aggroTime = 1.5f;
    [SerializeField] [Range(0.01f, 0.25f)] float parryTimeWindow = 0.2f;
    [SerializeField] float reviveDelay = 3f;
    [SerializeField] CircleCollider2D headCollider;


    bool corRunning;
    Coroutine deAggro;
    float playerDistance;
    bool isAttacking;
    public bool hasHeadExploded;

    Health health;
    SpriteRendererTextureSwap textureSwap;
    AudioPlayer audioPlayer;
    Animator m_animator;
    Rigidbody2D m_body2d;
    bool isDead;
    bool detectedPlayer ;
    bool aggro;
    public bool isStunned;
    bool parryWindow;
    float xScale;

    void Start()
    {
        audioPlayer = FindAnyObjectByType<AudioPlayer>();
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        textureSwap = GetComponent<SpriteRendererTextureSwap>();
        health = GetComponent<Health>();
        xScale = transform.localScale.x;
        //m_collider.enabled = false;
    }

    void Update()
    {
        if (isDead || isStunned) { return; }
        DetectPlayer();
        ManageAggro();
        ManageDistance();
    }

    void DetectPlayer()
    {
        Vector3 facingDirection = new Vector3(- Mathf.Sign(transform.localScale.x), 0f, 0f);
        int layerMask = 1 << 8;
        RaycastHit2D hit1 = (Physics2D.Raycast(transform.position + 0.4f * Vector3.up, facingDirection, detectDistance, layerMask));
        RaycastHit2D hit2 = (Physics2D.Raycast(transform.position + 0.8f * Vector3.up, facingDirection, detectDistance, layerMask));

        if (hit1)
        {
            Debug.DrawRay(transform.position + 0.4f * Vector3.up , facingDirection * hit1.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(transform.position + 0.4f * Vector3.up, facingDirection * detectDistance, Color.red);
        }

        if (hit2)
        {
            Debug.DrawRay(transform.position + 0.8f * Vector3.up, facingDirection * hit2.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(transform.position + 0.8f * Vector3.up, facingDirection * detectDistance, Color.red);
        }

        if (hit1 | hit2)
        {
            detectedPlayer = true;
        }
        else
        {
            detectedPlayer = false;
        }
    }

    void ManageAggro()
    {
        IEnumerator DeAggro()
        {
            yield return new WaitForSeconds(aggroTime);
            aggro = false;
            corRunning = false;
        }

        if (detectedPlayer && !aggro)
        {
            aggro = true;  
        }

        else if (detectedPlayer && corRunning)
        {
            StopCoroutine(deAggro);
            corRunning = false;
        }

        else if (!detectedPlayer && aggro && !corRunning)
        {
            corRunning = true;
            deAggro = StartCoroutine(DeAggro());
        }

    }

    void ManageDistance()
    {
        playerDistance =  playerTransform.position.x - transform.position.x;
        if (Mathf.Sign(playerDistance) > 0)
            transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
        else if (Mathf.Sign(playerDistance) < 0)
            transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);

        if (!aggro) { m_animator.SetInteger("AnimState", 0); return; }

        if (  Mathf.Abs(playerDistance) > attackDistance)
        {
            Run();
        }
        else
        {
            Attack();
        }     
    }

    private void Run()
    {
        if (isAttacking) { return; }
        m_body2d.linearVelocity = new Vector2(moveSpeed * Mathf.Sign(playerDistance), m_body2d.linearVelocity.y);
        m_animator.SetInteger("AnimState", 2);
    }

    void Attack()
    {
        
        m_animator.SetInteger("AnimState", 1);

        IEnumerator AttackAnimation()
        {
            
            isAttacking = true;
            yield return new WaitForSeconds(Random.Range(0.2f, 0.3f));
            
            m_animator.SetTrigger("Attack");
            yield return new WaitForSeconds( parryTimeWindow/2);
            
  
            parryWindow = true;

            yield return new WaitForSeconds(0.4f - parryTimeWindow / 2);
            //hitbox frames
            parryWindow = false;
            if (!isStunned){audioPlayer.PlaySlashClip();}
            
            this.gameObject.layer = LayerMask.NameToLayer("Hitbox");
            m_body2d.linearVelocity = new Vector2(boostSpeed * Mathf.Sign(playerDistance), m_body2d.linearVelocity.y);

            yield return new WaitForSeconds(0.2f);

            //recovery frames
            
            this.gameObject.layer = LayerMask.NameToLayer("Enemy");
            m_body2d.linearVelocity = new Vector2(boostSpeed * Mathf.Sign(playerDistance), m_body2d.linearVelocity.y);
            

            yield return new WaitForSeconds(0.2f);
            isAttacking = false;
        }

        if (!isAttacking)
        {
            StartCoroutine(AttackAnimation());
        } 
    }

    public void Die()
    {
        IEnumerator Revive()
        {
            yield return new WaitForSeconds(reviveDelay);
            m_animator.SetTrigger("Recover");
            yield return new WaitForSeconds(0.7f);
            isDead = false;
            aggro = false;
            corRunning = false;
            isAttacking = false;
            isStunned = false;
            detectDistance = 100f;
            health.SetHealth(10);
            this.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

        hasHeadExploded = true;
        textureSwap.enabled = true;
        this.gameObject.layer = LayerMask.NameToLayer("Dead Enemy");
        m_animator.SetTrigger("Death");
        isDead = true;
        StopAllCoroutines();
        StartCoroutine(Revive());
    }

    public void Parry()
    {
        if (!parryWindow) { return; }
        audioPlayer.PlayParryClip();
        m_animator.SetTrigger("Hurt");
        this.gameObject.layer = LayerMask.NameToLayer("Stunned Enemy");
        parryWindow = false;
        isStunned = true;
        aggro = false;
        corRunning = false;
        isAttacking = false;
        headCollider.enabled = true;
        StopAllCoroutines();
        StartCoroutine(DeStun());

        IEnumerator DeStun()
        {
            yield return new WaitForSeconds(1.4f);
            isStunned = false;
            aggro = false;
            corRunning = false;
            isAttacking = false;
            headCollider.enabled = false;
            this.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

    }
}
