using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;
    private enum Direction
    {
        left, right, up, down
    }
    private List<Direction> directionInputs = new();
    private Direction currentDirection = Direction.down;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown("left"))
        {
            directionInputs.Add(Direction.left);
        }
        if (Input.GetKeyDown("right"))
        {
            directionInputs.Add(Direction.right);
        }
        if (Input.GetKeyDown("up"))
        {
            directionInputs.Add(Direction.up);
        }
        if (Input.GetKeyDown("down"))
        {
            directionInputs.Add(Direction.down);
        }

        if (Input.GetKeyUp("left"))
        {
            directionInputs.Remove(Direction.left);
        }
        if (Input.GetKeyUp("right"))
        {
            directionInputs.Remove(Direction.right);
        }
        if (Input.GetKeyUp("up"))
        {
            directionInputs.Remove(Direction.up);
        }
        if (Input.GetKeyUp("down"))
        {
            directionInputs.Remove(Direction.down);
        }

        if (directionInputs.Count > 0)
        {
            if (directionInputs[^1] != currentDirection || rb.velocity == Vector2.zero)
            {
                Vector2 movement = Vector2.zero;
                currentDirection = directionInputs[^1];

                switch (currentDirection)
                {
                    case Direction.left:
                        animator.Play("Walk left");
                        movement = new Vector2(-speed, 0);
                        break;
                    case Direction.right:
                        animator.Play("Walk left");
                        movement = new Vector2(speed, 0);
                        break;
                    case Direction.up:
                        animator.Play("Walk up");
                        movement = new Vector2(0, speed);
                        break;
                    case Direction.down:
                        animator.Play("Walk down");
                        movement = new Vector2(0, -speed);
                        break;
                }

                if (rb.velocity != movement)
                {
                    rb.velocity = movement;
                }
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
}
