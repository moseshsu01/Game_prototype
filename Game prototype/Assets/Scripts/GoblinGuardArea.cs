using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinGuardArea : MonoBehaviour
{
    [SerializeField] private GoblinMovement goblin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            goblin.playerTriggerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            goblin.playerTriggerExit();
        }
    }
}
