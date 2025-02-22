using Unity.VisualScripting;
using UnityEngine;

public class EjectedShell : MonoBehaviour
{
    bool disable;
    bool isSpent;
    bool hasHitGround;
    [SerializeField] int shellType = 1;

    [SerializeField] AudioClip hittingGroundClip;
    [SerializeField][Range(0f, 1f)] float hittingGroundVolume = 1f;
    [SerializeField] Sprite[] shellSprites = new Sprite[3];
    AmmoPannel ammoPannel;
    [SerializeField] float minimumSpeedForSound = 2f;

    private void Awake()
    {
        if (shellType < shellSprites.Length && shellType > 0)
        {
            GetComponent<SpriteRenderer>().sprite = shellSprites[shellType - 1];
        }
    }

    public void IsSpent()
    {
        isSpent = true;
    }

    public void SetShellType(int type)
    {
        shellType = type;
        GetComponent<SpriteRenderer>().sprite = shellSprites[shellType - 1];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hittingGroundClip != null )
        {
            hasHitGround = true;
            if (collision.relativeVelocity.magnitude > minimumSpeedForSound)
            {
                AudioSource.PlayClipAtPoint(hittingGroundClip, Vector3.zero, hittingGroundVolume);
            }
            
            if (!isSpent)
            {
                ammoPannel = FindAnyObjectByType<AmmoPannel>();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!disable && hasHitGround && !isSpent && collider.tag == "Player")
        {
            bool wasPickedUp = ammoPannel.OnShellPickup(shellType);
            if (wasPickedUp)
            {
                disable = true;
                Destroy(gameObject);
            }     
        }
    }
}
