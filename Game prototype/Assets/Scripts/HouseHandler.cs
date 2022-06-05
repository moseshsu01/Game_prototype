using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseHandler : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement player;
    private float originalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.Play("Door open");
            if (!player) player = collision.gameObject.GetComponent<PlayerMovement>();
            originalSpeed = player.Speed;
            player.Speed = originalSpeed * 0.7f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.Play("Door close");
            player.Speed = originalSpeed;
        }
    }
}
