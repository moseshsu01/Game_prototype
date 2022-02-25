using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeHandler : MonoBehaviour
{
    public bool isExitTrigger;
    public bool isVertical;
    public static bool elevated = false;
    public static bool exitedFirstTrigger = false;
    private static Vector3 initialPos;
    private static int exitDistance = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        if (isExitTrigger && elevated)
        {
            deElevate(collision.gameObject);
            return;
        }

        initialPos = collision.gameObject.transform.position;

        if (!elevated)
        {
            elevated = true;
            GameObject player = collision.gameObject;
            player.layer = 6;
            player.GetComponent<SpriteRenderer>().sortingLayerName = "Elevated";
            GameObject playerShadow = player.transform.GetChild(0).gameObject;
            playerShadow.layer = 6;
            playerShadow.GetComponent<SpriteRenderer>().sortingLayerName = "Elevated";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        if (!elevated)
        {
            return;
        }

        Vector3 position = collision.gameObject.transform.position;
        bool exitTriggerEnter = (!isVertical && Mathf.Abs(position.x - initialPos.x) < exitDistance) ||
            (isVertical && Mathf.Abs(position.y - initialPos.y) < exitDistance);

        if ((!exitedFirstTrigger && exitTriggerEnter) || exitedFirstTrigger && !exitTriggerEnter)
        {
            deElevate(collision.gameObject);
        } else if (!exitedFirstTrigger && !exitTriggerEnter)
        {
            exitedFirstTrigger = true;
        }
    }

    private void deElevate(GameObject player)
    {
        exitedFirstTrigger = false;
        elevated = false;
        player.layer = 0;
        player.GetComponent<SpriteRenderer>().sortingLayerName = "Object";
        GameObject playerShadow = player.transform.GetChild(0).gameObject;
        playerShadow.layer = 0;
        playerShadow.GetComponent<SpriteRenderer>().sortingLayerName = "Object";
    }
}
