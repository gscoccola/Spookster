using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    [SerializeField] int healthPoints = 50;
    BanditMovement banditMovement;
    EnemyMovement enemyMovement;
    bool isBandit;

    [SerializeField] GameObject blood;
    [SerializeField] GameObject gooberChunks;
    [SerializeField] GameObject banditChunks;
    [SerializeField] List<Sprite> remains = new List<Sprite>();
    [SerializeField] float bleedTimeDelay = 0.1f;
    SpriteRenderer spriteRenderer;
    bool canBleed = true;
    bool hasExploded;
    bool hasDied;


    private void Start()
    {
        isBandit = TryGetComponent<BanditMovement>(out banditMovement);
        TryGetComponent<EnemyMovement>(out enemyMovement);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DecreaseHealthPoints(int amount)
    {
        healthPoints -= amount;
        if (canBleed)
        {
            StartCoroutine(SpawnBlood());
        }
        if (isBandit && banditMovement.isStunned)
        {
            healthPoints -= 2* amount;
        }
        if (healthPoints <= 0 && !hasDied)
        {
            hasDied = true;
            Die();
        }

        void Die()
        {
            if (!isBandit) 
            {
                
                if (!hasExploded)
                {
                    CircleCollider2D mCollider = GetComponent<CircleCollider2D>();
                    mCollider.radius = 0.12f;
                    mCollider.offset = new Vector2(-0.07f, -0.3f);

                    hasExploded = true;
                    enemyMovement.SetLayerOnDeath();
                    int randomInt = Random.Range(0, remains.Count+1);
                    if (randomInt == remains.Count)
                    {
                        AudioPlayer.Instance.PlayLoudSplatterClip();
                        spriteRenderer.enabled = false;
                    }
                    else
                    {
                        AudioPlayer.Instance.PlaySplatterClip();
                        spriteRenderer.sprite = remains[randomInt];
                    }
                    for (int i = 0; i < randomInt+1; i++)
                    {
                        Instantiate(gooberChunks, transform.position, Quaternion.identity);
                    }
                    
                    Instantiate(blood, transform.position, Quaternion.identity);
                    Instantiate(blood, transform.position, Quaternion.identity);
                }
            }
            else
            {
                if (!banditMovement.hasHeadExploded)
                {
                    AudioPlayer.Instance.PlayLoudSplatterClip();
                    Instantiate(blood, transform.position, Quaternion.identity);
                    Instantiate(blood, transform.position, Quaternion.identity);
                    Instantiate(banditChunks, transform.position + new Vector3(0f, 1.4f, 0f), Quaternion.identity);
                }
                banditMovement.Die(); 
            }
        }

        IEnumerator SpawnBlood()
        {
            canBleed = false;
            Instantiate(blood, transform.position, Quaternion.identity);
            AudioPlayer.Instance.PlayDamageClip();
            AudioPlayer.Instance.PlayDamageClip();
            yield return new WaitForSeconds(bleedTimeDelay);
            canBleed = true;
        }
    }

    public void SetHealth(int health)
    {
        healthPoints = health;
        hasDied = false;
    }
}
