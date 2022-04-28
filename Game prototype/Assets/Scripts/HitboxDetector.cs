using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDetector : MonoBehaviour
{
    [System.NonSerialized] public bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player hurtbox"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player hurtbox"))
        {
            playerInRange = false;
        }
    }
}
