using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private enum Direction
    {
        left, right, up, down
    }
    private List<Direction> directionInputs = new();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        Vector2 movement = Vector2.zero;
        if (directionInputs.Count > 0)
        {
            switch (directionInputs[^1])
            {
                case Direction.left:
                    movement = new Vector2(-speed, 0);
                    break;
                case Direction.right:
                    movement = new Vector2(speed, 0);
                    break;
                case Direction.up:
                    movement = new Vector2(0, speed);
                    break;
                case Direction.down:
                    movement = new Vector2(0, -speed);
                    break;
            }
        }

        if (rb.velocity != movement)
        {
            rb.velocity = movement;
        }
    }
}
