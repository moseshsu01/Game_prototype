using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private State state = State.idle;
    [SerializeField] private float knockForce;
    [SerializeField] private float knockTime;
    GameObject player;

    private enum State
    {
        attacking, idle
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (state == State.attacking)
        {
            AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(0);
            if (animatorState.normalizedTime > 1 && !animator.IsInTransition(0))
            {
                state = State.idle;
            }
        }
    }

    public void slash(GameObject enemy)
    {
        if (state == State.attacking)
        {
            return;
        }

        transform.position = enemy.transform.position;
        state = State.attacking;
        // fix the animaiton cuz it sucks right now
        //animator.Play("Slash hit", -1, 0);

        if (enemy != null)
        {
            PlayerMovement.Direction direction = player.GetComponent<PlayerMovement>().currentDirection;

            Vector2 force = Vector2.zero;
            switch (direction)
            {
                case PlayerMovement.Direction.left:
                    force = new Vector2(-knockForce, 0);
                    break;
                case PlayerMovement.Direction.right:
                    force = new Vector2(knockForce, 0);
                    break;
                case PlayerMovement.Direction.up:
                    force = new Vector2(0, knockForce);
                    break;
                case PlayerMovement.Direction.down:
                    force = new Vector2(0, -knockForce);
                    break;
            }

            enemy.GetComponent<GoblinMovement>().getHit(force, knockTime, 1);
        }
    }
}
