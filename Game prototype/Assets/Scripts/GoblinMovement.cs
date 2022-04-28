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
    private bool playerInArea = false;
    private bool playerInContact = false;

    private HitboxDetector leftHitbox;
    private HitboxDetector rightHitbox;
    private HitboxDetector upHitbox;
    private HitboxDetector downHitbox;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentDirection = defaultDirection;
        state = State.idle;

        leftHitbox = gameObject.transform.GetChild(1).gameObject.GetComponent<HitboxDetector>();
        rightHitbox = gameObject.transform.GetChild(2).gameObject.GetComponent<HitboxDetector>();
        upHitbox = gameObject.transform.GetChild(3).gameObject.GetComponent<HitboxDetector>();
        downHitbox = gameObject.transform.GetChild(4).gameObject.GetComponent<HitboxDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        bool canAttack = inAttackRange();
        switch (state)
        {
            case State.chasing:
            case State.rechasing:
                if (canAttack)
                {
                    attack();
                    break;
                }

                // !canAttack is also a condition
                if (playerInContact && isFacingPlayer())
                {
                    reChase();
                    break;
                }

                if (currentDirection == Direction.right && transform.position.x >= target.position.x
                    || currentDirection == Direction.left && transform.position.x <= target.position.x)
                {
                    chase();
                }
                else if (currentDirection == Direction.up && transform.position.y >= target.position.y
                  || currentDirection == Direction.down && transform.position.y <= target.position.y)
                {
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
                if (playerInArea && !canAttack)
                {
                    AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
                    if (state.normalizedTime > 1 && !animator.IsInTransition(0))
                    {
                        chase();
                    }
                        
                }
                break;
        }
    }

    public void playerTriggerEnter()
    {
        playerInArea = true;
        chase();
    }

    public void playerTriggerExit()
    {
        playerInArea = false;
        returnToOrigin();
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
                    return;
                }
            }
            else
            {
                if (currentDirection != Direction.left)
                {
                    walk(WalkAction.walkLeft);
                    return;
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
                    return;
                }
            }
            else
            {
                if (currentDirection != Direction.down)
                {
                    walk(WalkAction.walkDown);
                    return;
                }
            }
        }

        reChase();
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
        }
        else
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
                break;
            case WalkAction.walkRight:
                rb.velocity = new Vector2(speed, 0);
                break;
            case WalkAction.walkUp:
                rb.velocity = new Vector2(0, speed);
                break;
            case WalkAction.walkDown:
                rb.velocity = new Vector2(0, -speed);
                break;
        }
    }

    // assuming playerInContact is true
    bool isFacingPlayer()
    {
        float distanceX = target.position.x - transform.position.x;
        float distanceY = target.position.y - transform.position.y;
        switch (currentDirection)
        {
            case Direction.left:
            case Direction.right:
                return distanceY < 0.494999f && distanceY > -0.574999f;
            case Direction.up:
            case Direction.down:
                return distanceX < 0.795f && distanceX > -0.795f;
        }

        // should never reach here
        return false;
    }

    bool inAttackRange()
    {
        switch (currentDirection)
        {
            case Direction.left:
                return leftHitbox.playerInRange;
            case Direction.right:
                return rightHitbox.playerInRange;
            case Direction.up:
                return upHitbox.playerInRange;
            case Direction.down:
                return downHitbox.playerInRange;
        }
        // should never reach here
        return false;
    }
}
