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
        chasing, rechasing, returning, attacking, idle
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
    private bool playerInContact = false;

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

                // !inAttackRange() is also a requirement
                if (playerInContact && state != State.rechasing)
                {
                    reChase();
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInContact = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInContact = false;
        }
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
                animator.Play("Idle left");
                break;
            case Direction.right:
                animator.Play("Idle right");
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
                animator.Play("Stab left");
                break;
            case Direction.right:
                animator.Play("Stab right");
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
                if (currentDirection != Direction.right)
                {
                    walk(WalkAction.walkRight);
                }
            }
            else
            {
                if (currentDirection != Direction.left)
                {
                    walk(WalkAction.walkLeft);
                }
            }
        }
        else
        {
            if (distanceY > 0)
            {
                if (currentDirection != Direction.up)
                {
                    walk(WalkAction.walkUp);
                }
            }
            else
            {
                if (currentDirection != Direction.down)
                {
                    walk(WalkAction.walkDown);
                }
            }
        }
    }

    void reChase()
    {
        state = State.rechasing;
        if (currentDirection == Direction.left || currentDirection == Direction.right)
        {
            float distanceY = target.position.y - transform.position.y;
            if (distanceY > 0)
            {
                walk(WalkAction.walkUp);
            }
            else
            {
                walk(WalkAction.walkDown);
            }
        } else
        {
            float distanceX = target.position.x - transform.position.x;
            if (distanceX > 0)
            {
                walk(WalkAction.walkRight);
            }
            else
            {
                walk(WalkAction.walkLeft);
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
        animator.Play(walkInput.ToString());

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
        Vector2 position;
        // awkward initialization, fix
        RaycastHit2D[] stabHits = Physics2D.RaycastAll(transform.position, Vector2.left, 0);
        switch (currentDirection)
        {
            case Direction.left:
                position = new Vector2(transform.position.x, transform.position.y + 0.18f);
                stabHits = Physics2D.RaycastAll(position, Vector2.left, 0.85f);
                break;
            case Direction.right:
                position = new Vector2(transform.position.x, transform.position.y + 0.18f);
                stabHits = Physics2D.RaycastAll(position, Vector2.right, 0.85f);
                break;
            case Direction.up:
                position = new Vector2(transform.position.x + 0.1f, transform.position.y + 0.4f);
                stabHits = Physics2D.RaycastAll(position, Vector2.up, 0.5f);
                break;
            case Direction.down:
                position = new Vector2(transform.position.x - 0.25f, transform.position.y);
                stabHits = Physics2D.RaycastAll(position, Vector2.down, 0.5f);
                break;
        }

        foreach (RaycastHit2D hit in stabHits)
        {
            if (hit.collider.CompareTag("Player hurtbox"))
            {
                return true;
            }
        }

        return false;
    }
}
