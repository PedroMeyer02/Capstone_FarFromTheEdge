using UnityEngine;

public class AltarDialogue : MonoBehaviour
{
    DialogueAlchemist alchemistDialogue;

    public bool dialogueActive = false;

    Collider col;

    private void Awake()
    {
        alchemistDialogue = FindAnyObjectByType<DialogueAlchemist>();
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dialogueActive)
        {
            alchemistDialogue.DialogueStart();
            alchemistDialogue.AlchemistTransform();
            dialogueActive = true;
            col.enabled = false;
        }
    }
}
