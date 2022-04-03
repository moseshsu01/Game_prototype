using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 0.003f;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private Story currentStory;
    public static DialogueManager dialogueManager;

    public bool dialogueIsPlaying { get; private set; }

    private PlayerMovement player;

    private Coroutine displayLineCoroutine;

    private bool finishedAutotyping = false;

    private void Awake()
    {
        if (dialogueManager != null)
        {
            Debug.LogWarning("Found more than one dialogue manager in the scene");
        }
        dialogueManager = this;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (Input.GetButtonDown("AButton") || Input.GetButtonDown("BButton"))
        {
            if (finishedAutotyping)
            {
                ContinueStory();
            } else
            {
                dialogueText.text = currentStory.currentText;
                StopCoroutine(displayLineCoroutine);
                finishedAutotyping = true;
            }
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        player.freezeMovement();
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.1f);

        player.unfreezeMovement();
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";
        finishedAutotyping = false;

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        finishedAutotyping = true;
    }
}
