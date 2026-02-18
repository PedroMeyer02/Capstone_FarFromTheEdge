using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class DialogueControl : MonoBehaviour
{
    public GameObject dialogueBox;
    public Collider col;

    public bool dialogueStarted = false;

    private Dialogue dialogueComponent;

    public float keyCooldown = 0;
    public bool canType = false;

    void Awake()
    {
        dialogueComponent = dialogueBox.GetComponent<Dialogue>();
    }

    void Update()
    {
        if (dialogueStarted)
        {
            keyCooldown += Time.deltaTime;

            if(keyCooldown >= 0.5f)
            {
                canType = true;
            }

            if (canType && (Input.GetMouseButtonDown(0) || Input.anyKeyDown))
            {
                dialogueComponent.NextText();
                keyCooldown = 0;
                canType = false;
            }
        }
        else
        {
            canType = false;
            keyCooldown = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && !dialogueStarted)
        {
            DialogueStart();
        }
    }

    public void DialogueStart()
    {
        GameManager.Instance.IsPlayerPaused = true;
        dialogueBox.SetActive(true);
        dialogueStarted = true;

        dialogueComponent.StartDialogue();
    }

    public void EndDialogue()
    {
        GameManager.Instance.IsPlayerPaused = false;
        dialogueBox.SetActive(false);

        dialogueStarted = false;
        this.gameObject.SetActive(false);
    }
}
