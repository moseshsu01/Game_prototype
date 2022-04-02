using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
            updateSpeed();
        }
    }

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;
    private enum Direction
    {
        left, right, up, down
    }
    private enum Action
    {
        walkLeft, walkRight, walkUp, walkDown,
        slash
    }
    private List<Action> actionInputs = new();
    private Direction currentDirection = Direction.down;
    private Action? currentAction;

    private bool movementFrozen = false;
    public void freezeMovement()
    {
        movementFrozen = true;
        actionInputs.Clear();
    }
    public void unfreezeMovement() => movementFrozen = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!movementFrozen)
        {
            if (Input.GetKeyDown("left"))
            {
                actionInputs.Add(Action.walkLeft);
            }
            if (Input.GetKeyDown("right"))
            {
                actionInputs.Add(Action.walkRight);
            }
            if (Input.GetKeyDown("up"))
            {
                actionInputs.Add(Action.walkUp);
            }
            if (Input.GetKeyDown("down"))
            {
                actionInputs.Add(Action.walkDown);
            }
            if (Input.GetButtonDown("AButton"))
            {
                actionInputs.Add(Action.slash);
            }

            if (Input.GetKeyUp("left"))
            {
                actionInputs.Remove(Action.walkLeft);
            }
            if (Input.GetKeyUp("right"))
            {
                actionInputs.Remove(Action.walkRight);
            }
            if (Input.GetKeyUp("up"))
            {
                actionInputs.Remove(Action.walkUp);
            }
            if (Input.GetKeyUp("down"))
            {
                actionInputs.Remove(Action.walkDown);
            }
        }
        

        if (actionInputs.Count > 0)
        {
            string movementName = actionInputs[^1].ToString();
            if (movementName.StartsWith("walk"))
            {
                walk(actionInputs[^1]);
            } else if (actionInputs[^1] == Action.slash)
            {
                slash();
            }
            
        } else
        {
            switch (currentDirection)
            {
                case Direction.left:
                case Direction.right:
                    animator.Play("Idle left");
                    break;
                case Direction.up:
                    animator.Play("Idle up");
                    break;
                case Direction.down:
                    animator.Play("Idle down");
                    break;
            }

            rb.velocity = Vector2.zero;
        }

        sprite.flipX = currentDirection == Direction.right;
    }

    private Direction getWalkDirection(Action walkInput)
    {
        if (walkInput == Action.walkLeft)
        {
            return Direction.left;
        } else if (walkInput == Action.walkRight)
        {
            return Direction.right;
        } else if (walkInput == Action.walkUp)
        {
            return Direction.up;
        } else
        {
            return Direction.down;
        }
    }

    private void walk(Action walkInput)
    {
        if (getWalkDirection(walkInput) != currentDirection || rb.velocity == Vector2.zero)
        {
            Vector2 movement = Vector2.zero;
            currentDirection = getWalkDirection(walkInput);
            if (walkInput == Action.walkRight)
            {
                animator.Play("walkLeft");
            } else
            {
                animator.Play(walkInput.ToString());
            }

            switch (currentDirection)
            {
                case Direction.left:
                    movement = new Vector2(-speed, 0);
                    currentAction = Action.walkLeft;
                    break;
                case Direction.right:
                    movement = new Vector2(speed, 0);
                    currentAction = Action.walkRight;
                    break;
                case Direction.up:
                    movement = new Vector2(0, speed);
                    currentAction = Action.walkUp;
                    break;
                case Direction.down:
                    movement = new Vector2(0, -speed);
                    currentAction = Action.walkDown;
                    break;
            }

            if (rb.velocity != movement)
            {
                rb.velocity = movement;
            }
        }
    }

    private void slash()
    {
        if (currentAction == Action.slash)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.normalizedTime > 1 && !animator.IsInTransition(0))
            {
                actionInputs.Remove(Action.slash);
                currentAction = null;
            }

            return;
        }

        rb.velocity = Vector2.zero;
        switch (currentDirection)
        {
            case Direction.left:
            case Direction.right:
                animator.Play("slashLeft");
                break;
            case Direction.up:
                animator.Play("slashUp");
                break;
            case Direction.down:
                animator.Play("slashDown");
                break;
        }

        currentAction = Action.slash;
    }

    private void updateSpeed()
    {
        if (rb.velocity == Vector2.zero)
        {
            return;
        }

        switch (currentDirection)
        {
            case Direction.left:
                rb.velocity = new Vector2(-speed, 0);
                break;
            case Direction.right:
                rb.velocity = new Vector2(speed, 0);
                break;
            case Direction.up:
                rb.velocity = new Vector2(0, speed);
                break;
            case Direction.down:
                rb.velocity = new Vector2(0, -speed);
                break;
        }
    }
}
