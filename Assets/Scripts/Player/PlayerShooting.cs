using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    [Header("Initial Conditions")]
    [SerializeField] int[] ammoLoaded = new int[2] { 0, 0 };
    [SerializeField] int[] ammoSpent = new int[2] { 0, 0 };

    [Header("Prefabs")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject pellet;
    [SerializeField] Transform gun;
    [SerializeField] Transform gunPivot;
    [SerializeField] Transform barrelExit;
    [SerializeField] GameObject ejectedShell;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] GameObject muzzleFlashBig;
    [SerializeField] GameObject blankBlast;

    [Header("Ammo Sprites")]
    [SerializeField] Texture[] shellSprite = new Texture[3];
    [SerializeField] Texture[] shellSpentSprite = new Texture[3];
    [SerializeField] RawImage[] shellImage = new RawImage[2];

    [Header("Recoil")]
    [SerializeField] List<int> recoil = new List<int> { 1, 2, 3 };

    [Header("Pellets")]
    [SerializeField] float pelletSpeed = 40f;
    [SerializeField] int pelletCount = 6;
    [SerializeField] float pelletSpread = 30f;

    [Header("Bullets")]
    [SerializeField] float bulletSpeed = 40f;

    PlayerMovement playerMovement;
    Animator gunAnimator;
    Rigidbody2D myRigidBody;

    public bool inMenu;
    Vector3 facingDirection;
    Vector3 aimingDirection;
    bool isGunTilted;
    float gunAngle = 0f;
    float offset = 0.4f;
    public bool isAlive = true;

    const bool ejectorEnabled = true;
 

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        gunAnimator = gun.GetComponent<Animator>();
        aimingDirection = new Vector3 (1, 0, 0);
    }

    private void Update()
    {
        facingDirection = playerMovement.GetAimDirection();
        aimingDirection = Quaternion.Euler(0, 0, facingDirection.x * gunAngle) * facingDirection;
    }

    private void Start()
    {
        DisplayShell(0);
        DisplayShell(1);
    }

    void OnAttack1(InputValue value)
    {
        ShootBarrel(0);
    }

    void OnAttack2(InputValue value)
    {
        ShootBarrel(1);
    }

    void ShootBarrel(int barrelNumber)
    {

        if (playerMovement.IsDisabled() || inMenu) { return; }
        if (ammoLoaded[barrelNumber] == 0 || ammoSpent[barrelNumber]==1)
        {
            AudioPlayer.Instance.PlayDryFireClip();
            return;
        }
        
        if (ammoLoaded[barrelNumber] == 1)
        {
            AudioPlayer.Instance.PlayShootingClip1();
            ShootSpread();
        }
            
        else if (ammoLoaded[barrelNumber] == 2)
        {
            ShootBullet();
            AudioPlayer.Instance.PlayShootingClip2();
        }
            
        else if (ammoLoaded[barrelNumber] == 3)
        {
            ShootBlank();
            AudioPlayer.Instance.PlayShootingClip3();
        }
        ammoSpent[barrelNumber] = 1;
        DisplayShell(barrelNumber);
        myRigidBody.linearVelocityX += -aimingDirection.x * recoil[ammoLoaded[barrelNumber]-1];
        myRigidBody.linearVelocityY = Mathf.Max(myRigidBody.linearVelocityY , -aimingDirection.y * recoil[ammoLoaded[barrelNumber] - 1]);
    }

    public int ReloadBarrel(int barrelNumber, int shellType)
    {
        if (!playerMovement.isAlive || (playerMovement.tutorialStage!=2 && playerMovement.tutorialStage != 3 && playerMovement.tutorialStage != 0)
            || shellType == ammoLoaded[barrelNumber]) { return shellType; }
        if (playerMovement.tutorialStage == 2)
        {
            if (barrelNumber == 1) { return shellType; }
            playerMovement.tutorialStage = 3;
        }
        else if (playerMovement.tutorialStage == 3)
        {
            if (barrelNumber == 0) { return shellType; }
            playerMovement.tutorialStage = 4;
        }

        int loadedShellType = ammoLoaded[barrelNumber];
        ammoLoaded[barrelNumber] = shellType;
        AudioPlayer.Instance.PlayReloadingClip();
        DisplayShell(barrelNumber);
        return loadedShellType;
    }

    void DisplayShell(int barrelNumber)
    {
        if (ammoLoaded[barrelNumber] == 0)
        {
            shellImage[barrelNumber].enabled = false;
        }
        else
        {
            shellImage[barrelNumber].enabled = true;
            if (ammoSpent[barrelNumber] == 1)
            {
                shellImage[barrelNumber].texture = shellSpentSprite[ammoLoaded[barrelNumber]-1];
            }
            else
            {
                shellImage[barrelNumber].texture = shellSprite[ammoLoaded[barrelNumber]-1];
            }
        }
    }

    void OnOpenGun(InputValue value)
    {
        OpenGun();
    }

    public void OpenGun()
    {
        if (playerMovement.IsDisabled() && playerMovement.tutorialStage !=1) { return; }
        if (playerMovement.tutorialStage == 1) { playerMovement.tutorialStage = 2; }
        if (!inMenu)
        {
            
            if (isGunTilted)
            {
                TiltGun(0);
                isGunTilted = false;
                playerMovement.SetIsGunTilted();
            }
            GameManager.Instance.StartSlowMotion();
            inMenu = true;
            gunAnimator.SetBool("isOpen", true);
            AudioPlayer.Instance.PlayOpeningClip();
            for (int i = 0; i < 2; i++)
            {
                if (ejectorEnabled)
                {
                    EjectShell(i);
                    DisplayShell(i);
                }
            }
        }
    }

    void OnScrollWheel(InputValue value)
    {
        if (!inMenu && value.Get<Vector2>().y == -1f)
        {
            OpenGun();
        }
        if (inMenu && value.Get<Vector2>().y == +1f)
        {
            CloseGun();
        }
    }

    void OnCloseGun(InputValue value)
    {
        CloseGun();
    }

    public void CloseGun()
    {
        if (playerMovement.IsDisabled() && playerMovement.tutorialStage != 4) { return; }
        if (playerMovement.tutorialStage == 4) { playerMovement.tutorialStage = 0; }
        if (inMenu)
        {
            GameManager.Instance.StopSlowMotion();
            inMenu = false;
            gunAnimator.SetBool("isOpen", false);
            AudioPlayer.Instance.PlayOpeningClip();
        }
    }

    public int GetLoadedShell(int barrelNumber)
    {
        return ammoLoaded[barrelNumber];
    }

    public void TiltGun(int angle)
    {
        gunAngle = angle;
        
        gunPivot.transform.SetLocalPositionAndRotation(gunPivot.transform.localPosition , Quaternion.Euler(0, 0, angle));
        if (angle != 0){
            isGunTilted = true;
        }
        else
        {
            isGunTilted = false;
        }
    }

    void EjectShell(int barrelNumber)
    {
        if (ammoLoaded[barrelNumber] == 0) { return; }
        
        GameObject instance = Instantiate(ejectedShell, gun.transform.position - facingDirection * offset, Quaternion.Euler(new Vector3(0,0,90 * facingDirection.x)));
        Rigidbody2D instanceBody = instance.GetComponent<Rigidbody2D>();

        if (ammoSpent[barrelNumber] == 1)
        {
            instance.GetComponent<EjectedShell>().IsSpent();
        }
        
        instance.transform.localScale = new Vector3( facingDirection.x , instance.transform.localScale.y, 1);
        instanceBody.linearVelocityY = 4f + Random.Range(-0.8f, +0.8f);
        instanceBody.linearVelocityX = -facingDirection.x* 2f + Random.Range(-0.5f, +0.5f);
        instanceBody.angularVelocity = Random.Range(-120f, +120f);

        instance.GetComponent<EjectedShell>().SetShellType(ammoLoaded[barrelNumber]);

        ammoSpent[barrelNumber] = 0;
        ammoLoaded[barrelNumber] = 0;
    }


    void ShootSpread()
    {
        Instantiate(muzzleFlash, barrelExit.position, Quaternion.Euler(0, 0, gunAngle * facingDirection.x + 90*(facingDirection.x -1)  ));
        for ( int i =0;  i < pelletCount; i++)
        {
            Rigidbody2D instance = Instantiate(pellet, barrelExit.position, gunPivot.rotation).GetComponent<Rigidbody2D>();
            instance.linearVelocity = pelletSpeed * (Quaternion.Euler(0, 0, Random.Range(-pelletSpread/2, pelletSpread/2)) * aimingDirection);
        }
    }

    void ShootBullet()
    {
        Instantiate(muzzleFlash, barrelExit.position, Quaternion.Euler(0, 0, gunAngle * facingDirection.x + 90 * (facingDirection.x - 1) ));
        Rigidbody2D instance = Instantiate(bullet, barrelExit.position, gunPivot.rotation).GetComponent<Rigidbody2D>();
        instance.linearVelocity =  bulletSpeed * aimingDirection;
    }

    void ShootBlank()
    {
        Instantiate(muzzleFlashBig, barrelExit.position, Quaternion.Euler(0, 0, gunAngle * facingDirection.x + 90 * (facingDirection.x - 1)) );
        Instantiate(blankBlast, barrelExit.position, Quaternion.Euler(0, 0, gunAngle * facingDirection.x + 90 * (facingDirection.x - 1)));
    }

}
