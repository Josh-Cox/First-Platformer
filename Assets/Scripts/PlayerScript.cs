using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    private float horizontal = 0f;
    private float speed = 3f;
    [SerializeField] private float jumpingPower = 12f;
    private float hangCounter;
    [SerializeField] private float hangTime = 0.1f;
    private float jumpBufferCounter;
    [SerializeField] private float jumpBufferLength = 0.1f;
    private float registerJumpCounter;
    [SerializeField] private bool onGround;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private ParticleSystem footsteps;
    [SerializeField] private SceneChanger logic;
    [SerializeField] private AudioSource burnSound;

    private bool onFire = false;

    private enum playerState
    {
        idle,
        running,
        jumping,
        falling,
        fire,
        grater
    }

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<SceneChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.6f, 0.03f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        if (onFire)
        {
            horizontal = 0;
        }
        else
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }

        // Register Jump
        if (!Input.GetButtonDown("Jump"))
        {
            registerJumpCounter -= Time.deltaTime;
        }

        // Hang Time
        if (onGround)
        {
            hangCounter = hangTime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }

        // Jump Buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferLength;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Jump in the air
        if (jumpBufferCounter >= 0 && hangCounter > 0f && registerJumpCounter <= 0 && !onFire)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpBufferCounter = 0f;
            registerJumpCounter = 0.25f;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // Player falls off screen
        if (transform.position.y < -30)
        {
            PlayerDies();
        }


        // Show footstep effect
        if (rb.velocity.x != 0 && (onGround))
        {
            var footEmission = footsteps.emission;
            footEmission.rateOverTime = 35.0f;
        }
        else
        {
            var footEmission = footsteps.emission;
            footEmission.rateOverTime = 0.1f;
        }

        // Animation control
        Flip();
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        playerState state;

        // Check player state
        

        if (onFire)
        {
            state = playerState.fire;
        }
        else
        {
            if (horizontal > 0f)
            {
                state = playerState.running;
            }
            else if (horizontal < 0f)
            {
                state = playerState.running;
            }
            else
            {
                state = playerState.idle;
            }

            if (rb.velocity.y > 0.1f)
            {
                state = playerState.jumping;
            }
            else if (rb.velocity.y < -0.1f)
            {
                state = playerState.falling;
            }
        }

        // Set animation
        anim.SetInteger("state", (int)state);
    }

    private void FixedUpdate()
    {
        if (horizontal > 0.1f || horizontal < -0.1f)
        {
            rb.AddForce(new Vector2(horizontal * speed, 0f), ForceMode2D.Impulse);
        }
    }

    public void PlayerDies()
    {
        transform.position = logic.getRespawn();
        onFire = false;
    }

    public void PlayerBounce(float height)
    {
        rb.velocity = new Vector2(rb.velocity.x, height);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fire")
        {
            onFire = true;
            burnSound.Play();
        }

        if (collision.gameObject.tag == "Mouse")
        {
            logic.MoveToScene(0);
        }
    }
}
