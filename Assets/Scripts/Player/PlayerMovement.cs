
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    
    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    BoxCollider2D myCollider;
    CapsuleCollider2D myFeetCollider;
    SpriteRenderer mySpriteRenderer;
    PlayerShooting playerShooting;

    CameraTarget cameraTarget;
    float lookDirection = 0;

    float defaultGravity;
    public int tutorialStage = 0;
    public bool isAlive = true;
    bool isPaused = false;
    bool isDisabled = false;
    bool isOnFloor;
    bool isOnLadder;
    bool isRunning;
    bool isPlayingFootsteps;
    int gunTilt;
    bool playerIsClimbing;
    bool hasFoundManager;
    Coroutine runningCoroutine;
    Vector3 aimDirection = new Vector3(1, 0 ,0);

    [SerializeField] float footstepsDelay = 1.5f;

    [Header("Speeds")]
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float jumpSpeed = 6f;
    [SerializeField] float runSpeed = 10f;

    [Header("Accelerations")]
    [SerializeField] float groundAcceleration = 2f;
    [SerializeField] float airAcceleration = 2f;
    float deathSpeed = 5f;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myFeetCollider = GetComponent<CapsuleCollider2D>();
        defaultGravity = myRigidBody.gravityScale;
        myCollider = GetComponent<BoxCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        playerShooting = GetComponent<PlayerShooting>();
        cameraTarget = FindAnyObjectByType<CameraTarget>();
    }

    void Update()
    {

        Run();
        IsOnFloorOrLadder();
        DetermineIfDisabled();
        if (isDisabled) {return;}

        
        
        FlipSprite();
        ClimbLadder();
        Die();
        HandleGunTilt();
        HandleCamera();
    }

    void DetermineIfDisabled()
    {
            isDisabled = (!isAlive || isPaused || tutorialStage!=0);
    }

    void IsOnFloorOrLadder()
    {
        isOnFloor = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        isOnLadder = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    }

    private void Run()
    {
        if (isDisabled)
        {
            moveInput = new Vector2(0f, 0f);
        }
        Vector2 playerVelocity = new Vector2(moveInput.x, 0f);
        if (!isOnFloor && !isOnLadder) 
        {
            myRigidBody.linearVelocityX = Vector2.MoveTowards(myRigidBody.linearVelocity, playerVelocity * runSpeed, airAcceleration * Time.deltaTime).x;
        }
        else
        {
            myRigidBody.linearVelocityX = Vector2.MoveTowards(myRigidBody.linearVelocity, playerVelocity * runSpeed, groundAcceleration * Time.deltaTime).x;
            bool playerHadHorizontalSpeed = Mathf.Abs(moveInput.x) > Mathf.Epsilon;
            myAnimator.SetBool("isRunning", playerHadHorizontalSpeed);
            if (playerHadHorizontalSpeed)
            {
                isRunning = true;
            }
            else { isRunning = false; }
        }
        HandleFootsteps();
    }

    void HandleFootsteps()
    {
        if (isRunning && !isPlayingFootsteps)
        {
            runningCoroutine = StartCoroutine(PlayFootsteps());
        }
    }

    IEnumerator PlayFootsteps()
    {
        isPlayingFootsteps = true;
        AudioPlayer.Instance.PlayFootstepsClip();
        yield return new WaitForSeconds(footstepsDelay);
        isPlayingFootsteps = false;
    }


    private void ClimbLadder()
    {
        if (!isOnLadder)
        {
            myRigidBody.gravityScale = defaultGravity;
            myAnimator.SetBool("isClimbing", false);
            playerIsClimbing = false;
            return;
        }

        myRigidBody.gravityScale = 0;
        myRigidBody.linearVelocityX = Vector2.MoveTowards(myRigidBody.linearVelocity, new Vector2(moveInput.x, 0f) * runSpeed, groundAcceleration * 2 * Time.deltaTime).x;
        myRigidBody.linearVelocityY = moveInput.y*climbSpeed;
        playerIsClimbing =  Mathf.Abs(myRigidBody.linearVelocityY) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerIsClimbing);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isOnFloor || isDisabled) {return;}
        if (value.isPressed)
        {
            myRigidBody.linearVelocityY = jumpSpeed;
            AudioPlayer.Instance.PlayJumpClip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.relativeVelocity.magnitude > 5f)
        {
            AudioPlayer.Instance.PlayLandingClip();
        }
    }

    private void FlipSprite()
    {
        bool isPressingInputDirection = Mathf.Abs(moveInput.x) > Mathf.Epsilon;
        if (isPressingInputDirection)
        {
            transform.localScale = new Vector2(Mathf.Sign(moveInput.x), 1f);
            aimDirection = new Vector2(Mathf.Sign(moveInput.x), 0f);
        }
        
    }

    public Vector3 GetAimDirection()
    {
        return aimDirection;
    }

    void HandleCamera()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (lookDirection != -1)
            {
                cameraTarget.LookDown();
                lookDirection = -1;
            }
        }

        else if (Input.GetKey(KeyCode.W))
        {
            if (lookDirection != 1)
            {
                cameraTarget.LookUp();
                lookDirection = 1;
            }
        }
        else
        {
            if (lookDirection != 0)
            {
                cameraTarget.ResetCamera();
                lookDirection = 0;
            }   
        }

    }

    void HandleGunTilt()
    {
        if (playerShooting == null) { return; }
        if (!playerShooting.inMenu && !isOnFloor && Input.GetKey(KeyCode.S) && !playerIsClimbing)
        {
            playerShooting.TiltGun(-90);
            gunTilt = -1;
        }
        else if (!playerShooting.inMenu && Input.GetKey(KeyCode.W) && !playerIsClimbing)
        {
            playerShooting.TiltGun(45);
            gunTilt = 1;
        }
        else if (gunTilt!=0)
        {
            playerShooting.TiltGun(0);
            gunTilt = 0;
        }
    }

    public void SetIsGunTilted()
    {
        gunTilt = 0;
    }

    private void Die()
    {
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Hazards", "Hitbox")))
        {
            AudioPlayer.Instance.PlayDeathClip();
            isAlive = false;
            myAnimator.SetTrigger("dying");
            mySpriteRenderer.color = Color.red;
            myRigidBody.linearVelocityY = deathSpeed;
            if (playerShooting != null)
            {
                playerShooting.isAlive = false;
            }
            StartCoroutine(GameManager.Instance.ReloadSceneCoroutine());
        }
    }


    void OnReloadLevel(InputValue value)
    {
        GameManager.Instance.ReloadLevel();
        isPaused = false;
    }

    void OnPause(InputValue value)
    {
        GameManager.Instance.TogglePause();
        isPaused = !isPaused;
    }

    public bool IsDisabled() => isDisabled;

    public void SetTutorialStage(int stage)
    {
        tutorialStage = stage;
    }

   

}
