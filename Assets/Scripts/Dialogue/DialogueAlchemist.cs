using UnityEngine;

public class DialogueAlchemist : MonoBehaviour
{
    public GameObject dialogueBox;
    public GameObject dialogueBox2;
    public GameObject dialogueBox3;

    public Collider col;

    public bool dialogueStarted = false;

    public DialogueFromAlchemist dialogueComponent;
    Player player;

    public float keyCooldown = 0;
    public bool canType = false;

    public bool isFirstPartDone = false;
    public bool isSecondPartDone = false;

    public bool firstDialogueDone = false;

    public bool trapFree = false;

    void Awake()
    {
        dialogueComponent = dialogueBox.GetComponent<DialogueFromAlchemist>();
    }

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    void Update()
    {
        if (dialogueStarted)
        {
            keyCooldown += Time.deltaTime;

            if (keyCooldown >= 0.5f)
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

        if (GameManager.Instance.A4isFreeFromTrap && !trapFree)
        {
            DialogueStart();
            trapFree = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dialogueStarted)
        {     
            DialogueStart();

            if (!firstDialogueDone)
            {
                col.enabled = false;
                firstDialogueDone = true;
            }
        }
    }

    public void DialogueStart()
    {
        GameManager.Instance.IsPlayerPaused = true;

        if (!isFirstPartDone) dialogueBox.SetActive(true);
        else if (isFirstPartDone && !isSecondPartDone) dialogueBox2.SetActive(true);
        else dialogueBox3.SetActive(true);

        dialogueStarted = true;

        dialogueComponent.StartDialogue();
    }

    public void EndDialogue()
    {
        GameManager.Instance.IsPlayerPaused = false;
        dialogueBox.SetActive(false);
        dialogueBox2.SetActive(false);
        dialogueBox3.SetActive(false);

        dialogueStarted = false;

        if (!isFirstPartDone)
        {
            isFirstPartDone = true;
            dialogueComponent = dialogueBox2.GetComponent<DialogueFromAlchemist>();

        }
        else if(isFirstPartDone && !isSecondPartDone)
        {
            player.trapID = 2;
            player.playerIsTrapped = true;
            player.GetComponent<Animator>().SetTrigger("Trapped2");
            Animator anim = GetComponentInParent<Animator>();
            anim.SetTrigger("Attack");
            isSecondPartDone = true;
            dialogueComponent = dialogueBox3.GetComponent<DialogueFromAlchemist>();
        }
        else
        {
            Animator anim = GetComponentInParent<Animator>();
            anim.SetTrigger("Vanish");
            this.gameObject.SetActive(false);
        }
    }

    public void AlchemistTransform()
    {
        Animator anim = GetComponentInParent<Animator>();
        anim.SetTrigger("Transform");
    }
}
