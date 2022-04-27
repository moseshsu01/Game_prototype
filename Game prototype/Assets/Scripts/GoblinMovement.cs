using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMovement : MonoBehaviour
{
    [SerializeField] private Transform origin;
    [SerializeField] private float speed;
    [SerializeField] private Direction defaultDirection;
    private Transform target;
    private enum State
    {
        chasing, returning, attacking, idle
    }
    private State state;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;

    private enum WalkAction
    {
        walkLeft, walkRight, walkUp, walkDown
    }
    private enum Direction
    {
        left, right, up, down
    }
    private Direction currentDirection;
    private bool playerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentDirection = defaultDirection;
        state = State.idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.chasing:
                if (inAttackRange())
                {
                    attack();
                    break;
                }

                if (currentDirection == Direction.right && transform.position.x >= target.position.x
                    || currentDirection == Direction.left && transform.position.x <= target.position.x)
                {
                    transform.position = new Vector3(target.position.x, transform.position.y);
                    chase();
                }
                else if (currentDirection == Direction.up && transform.position.y >= target.position.y
                  || currentDirection == Direction.down && transform.position.y <= target.position.y)
                {
                    transform.position = new Vector3(transform.position.x, target.position.y);
                    chase();
                }
                break;
            case State.returning:
                if (transform.position == origin.position)
                {
                    idle();
                    break;
                }

                if (currentDirection == Direction.right && transform.position.x >= origin.position.x
                    || currentDirection == Direction.left && transform.position.x <= origin.position.x)
                {
                    transform.position = new Vector3(origin.position.x, transform.position.y);
                    returnToOrigin();
                } else if (currentDirection == Direction.up && transform.position.y >= origin.position.y
                    || currentDirection == Direction.down && transform.position.y <= origin.position.y)
                {
                    transform.position = new Vector3(transform.position.x, origin.position.y);
                    returnToOrigin();
                }
                break;
            case State.attacking:
                if (playerInRange && !inAttackRange())
                {
                    chase();
                }
                break;
        }

        sprite.flipX = currentDirection == Direction.right;
    }

    public void playerTriggerEnter()
    {
        playerInRange = true;
        chase();
    }

    public void playerTriggerExit()
    {
        playerInRange = false;
        returnToOrigin();
    }

    void idle()
    {
        rb.velocity = Vector2.zero;
        state = State.idle;

        switch (defaultDirection)
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

        currentDirection = defaultDirection;
    }

    void attack()
    {
        state = State.attacking;
        rb.velocity = Vector2.zero;

        switch (currentDirection)
        {
            case Direction.left:
            case Direction.right:
                animator.Play("Stab left");
                break;
            case Direction.up:
                animator.Play("Stab up");
                break;
            case Direction.down:
                animator.Play("Stab down");
                break;
        }
    }

    void chase()
    {
        // display an exclamation mark or something

        state = State.chasing;
        float distanceX = target.position.x - transform.position.x;
        float distanceY = target.position.y - transform.position.y;
        if (Mathf.Abs(distanceX) > Mathf.Abs(distanceY))
        {
            if (distanceX > 0)
            {
                walk(WalkAction.walkRight);
            }
            else
            {
                walk(WalkAction.walkLeft);
            }
        }
        else
        {
            if (distanceY > 0)
            {
                walk(WalkAction.walkUp);
            }
            else
            {
                walk(WalkAction.walkDown);
            }
        }
    }

    void returnToOrigin()
    {
        state = State.returning;
        float distanceX = origin.position.x - transform.position.x;
        float distanceY = origin.position.y - transform.position.y;
        if (Mathf.Abs(distanceX) > Mathf.Abs(distanceY))
        {
            if (distanceX > 0)
            {
                walk(WalkAction.walkRight);
            }
            else
            {
                walk(WalkAction.walkLeft);
            }
        }
        else
        {
            if (distanceY > 0)
            {
                walk(WalkAction.walkUp);
            }
            else
            {
                walk(WalkAction.walkDown);
            }
        }
    }

    private Direction getWalkDirection(WalkAction walkInput)
    {
        if (walkInput == WalkAction.walkLeft)
        {
            return Direction.left;
        }
        else if (walkInput == WalkAction.walkRight)
        {
            return Direction.right;
        }
        else if (walkInput == WalkAction.walkUp)
        {
            return Direction.up;
        }
        else
        {
            return Direction.down;
        }
    }

    private void walk(WalkAction walkInput)
    {
        currentDirection = getWalkDirection(walkInput);
        if (walkInput == WalkAction.walkRight)
        {
            animator.Play("walkLeft");
        }
        else
        {
            animator.Play(walkInput.ToString());
        }

        switch (walkInput)
        {
            case WalkAction.walkLeft:
                rb.velocity = new Vector2(-speed, 0);
                currentDirection = Direction.left;
                break;
            case WalkAction.walkRight:
                rb.velocity = new Vector2(speed, 0);
                currentDirection = Direction.right;
                break;
            case WalkAction.walkUp:
                rb.velocity = new Vector2(0, speed);
                currentDirection = Direction.up;
                break;
            case WalkAction.walkDown:
                rb.velocity = new Vector2(0, -speed);
                currentDirection = Direction.down;
                break;
        }
    }

    bool inAttackRange()
    {
        if (Mathf.Abs(transform.position.x - target.position.x) <= 0.2f)
        {
            float distance = target.position.y - transform.position.y;
            if (currentDirection == Direction.up)
            {
                return distance <= 0.75f && distance >= 0.65f;
            } else if (currentDirection == Direction.down)
            {
                return distance >= -0.75f && distance <= -0.65f;
            }
        } else if (Mathf.Abs(transform.position.y - target.position.y) <= 0.2f)
        {
            float distance = target.position.x - transform.position.x;
            if (currentDirection == Direction.left)
            {
                return distance >= -1 && distance <= -0.9f;
            }
            else if (currentDirection == Direction.right)
            {
                return distance <= 1 && distance >= 0.9f;
            }
        }
        return false;
    }
}
