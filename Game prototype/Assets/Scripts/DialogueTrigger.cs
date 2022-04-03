using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [SerializeField] private TextAsset inkJSON;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().playerAButton = TriggerDialogue;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().playerAButton -= TriggerDialogue;
        }
    }

    private void TriggerDialogue()
    {
        if (!DialogueManager.dialogueManager.dialogueIsPlaying)
        {
            DialogueManager.dialogueManager.EnterDialogueMode(inkJSON);
        }
    }
}
