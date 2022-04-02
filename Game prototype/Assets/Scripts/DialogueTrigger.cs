using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private bool playerInRange = false;

    [SerializeField] private TextAsset inkJSON;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("AButton") && playerInRange &&
            !DialogueManager.dialogueManager.dialogueIsPlaying)
        {
            DialogueManager.dialogueManager.EnterDialogueMode(inkJSON);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
