using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectHandler : MonoBehaviour
{
    public bool isBridge;
    public bool isStairs;
    public bool isVertical;
    public static bool areaEffectActivated = false;
    public static bool exitedFirstTrigger = false;
    private static Vector3 initialPos;
    private static Vector2 exitDistance = new Vector2(1, 0.55f);

    delegate void areaEffect(GameObject player);
    areaEffect activate;
    areaEffect deActivate;

    private void Start()
    {
        if (isBridge)
        {
            activate = elevate;
            deActivate = deElevate;
        } else if (isStairs)
        {
            activate = decreaseSpeed;
            deActivate = restoreSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        initialPos = collision.gameObject.transform.position;

        if (!areaEffectActivated)
        {
            areaEffectActivated = true;
            activate(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        if (!areaEffectActivated)
        {
            return;
        }

        Vector3 position = collision.gameObject.transform.position;
        bool exitTriggerEnter = (!isVertical && Mathf.Abs(position.x - initialPos.x) < exitDistance.x) ||
            (isVertical && Mathf.Abs(position.y - initialPos.y) < exitDistance.y);

        if ((!exitedFirstTrigger && exitTriggerEnter) || exitedFirstTrigger && !exitTriggerEnter)
        {
            exitedFirstTrigger = false;
            areaEffectActivated = false;
            deActivate(collision.gameObject);
        } else if (!exitedFirstTrigger && !exitTriggerEnter)
        {
            exitedFirstTrigger = true;
        }
    }

    private void elevate(GameObject player)
    {
        player.layer = 6;
        player.GetComponent<SpriteRenderer>().sortingLayerName = "Elevated";
        GameObject playerShadow = player.transform.GetChild(0).gameObject;
        playerShadow.layer = 6;
        playerShadow.GetComponent<SpriteRenderer>().sortingLayerName = "Elevated";
    }

    private void deElevate(GameObject player)
    {
        player.layer = 0;
        player.GetComponent<SpriteRenderer>().sortingLayerName = "Object";
        GameObject playerShadow = player.transform.GetChild(0).gameObject;
        playerShadow.layer = 0;
        playerShadow.GetComponent<SpriteRenderer>().sortingLayerName = "Object";
    }

    private void decreaseSpeed(GameObject player)
    {
        float originalSpeed = player.GetComponent<PlayerMovement>().Speed;
        player.GetComponent<PlayerMovement>().Speed = originalSpeed * 0.8f;
    }

    private void restoreSpeed(GameObject player)
    {
        float originalSpeed = player.GetComponent<PlayerMovement>().Speed;
        player.GetComponent<PlayerMovement>().Speed = originalSpeed * 1.25f;
    }
}
