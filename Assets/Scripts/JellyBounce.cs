using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyBounce : MonoBehaviour
{
    public PlayerScript player;
    private bool colliding = false;
    private float collisionCounter;
    private float collisionTime = 0.1f;

    [SerializeField] private float bounceStrength = 10;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private AudioSource bounceSound;

    // Start is called before the first frame update
    void Start()
    {
        collisionCounter = 0;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!colliding)
        {
            collisionCounter -= Time.deltaTime;
        }
        UpdateJellyAnimation();
    }

    private void UpdateJellyAnimation()
    {
        if (collisionCounter > 0f)
        {
            anim.SetBool("bouncing", true);
        }
        else
        {
            anim.SetBool("bouncing", false);
        }
    }

    // Player collides with oven
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            // Bounce player
            player.PlayerBounce(bounceStrength);
            bounceSound.Play();
            colliding = true;
            collisionCounter = collisionTime;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            colliding = false;
        }
    }
}
