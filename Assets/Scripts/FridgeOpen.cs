using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeOpen : MonoBehaviour
{
    private float fridgeCounter;
    private float fridgeTime = 2f;

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private BoxCollider2D fridgeDoor;

    // Start is called before the first frame update
    void Start()
    {
        fridgeCounter = fridgeTime;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateJellyAnimation();

        if (fridgeCounter != 0)
        {
            fridgeCounter -= Time.deltaTime;
        }

    }

    private void UpdateJellyAnimation()
    {
        if (fridgeCounter <= 0)
        {
            if (anim.GetBool("open") == false)
            {
                anim.SetBool("open", true);
                fridgeDoor.enabled = true;
            }
            else
            {
                anim.SetBool("open", false);
                fridgeDoor.enabled = false;
            }

            fridgeCounter = fridgeTime;
        }
    }
}
