using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private PlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = gameObject.transform.parent.transform.Find("Attack hit")
            .gameObject.GetComponent<PlayerAttack>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy hurtbox"))
        {
            playerAttack.slash(collision.gameObject.transform.position);
        }
    }
}
