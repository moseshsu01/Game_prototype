using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private State state = State.idle;

    private enum State
    {
        attacking, idle
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (state == State.attacking)
        {
            AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(0);
            if (animatorState.normalizedTime > 1 && !animator.IsInTransition(0))
            {
                state = State.idle;
                //animator.Play("Blank");
            }
        }
    }

    public void slash(Vector2 position)
    {
        if (state == State.attacking)
        {
            return;
        }

        transform.position = position;
        state = State.attacking;
        animator.Play("Slash hit", -1, 0);
    }
}
