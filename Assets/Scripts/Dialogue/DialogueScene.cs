using UnityEngine;

public class DialogueScene : MonoBehaviour
{
    public GameObject dialogueBox;
    public GameObject dialogueBox2;

    public Collider col;

    public bool dialogueStarted = false;

    public DialogueFromScene dialogueComponent;
    Player player;
    TotemScript totemScript;

    public float keyCooldown = 0;
    public bool canType = false;

    public bool isFirstPartDone = false;
    public bool isSecondPartDone = false;


    void Awake()
    {
        dialogueComponent = dialogueBox.GetComponent<DialogueFromScene>();
    }

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        totemScript = FindAnyObjectByType<TotemScript>();
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

        //if(GameManager.Instance.A2isFreeFromTrap && !dialogueStarted && !isSecondPartDone)
        //{

        //    dialogueComponent = dialogueBox2.GetComponent<DialogueFromScene>();

        //    isFirstPartDone = true;
        //    isSecondPartDone = true;
        //    DialogueStart();
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && GameManager.Instance.A2RuneWasActivated)
        {
            dialogueComponent = dialogueBox2.GetComponent<DialogueFromScene>();

            isFirstPartDone = true;
            isSecondPartDone = true;
            DialogueStart();
        }
        
        if (other.CompareTag("Player") && !dialogueStarted && !isFirstPartDone)
        {
            DialogueStart();
        }

    }

    public void DialogueStart()
    {
        GameManager.Instance.IsPlayerPaused = true;

        if (!isFirstPartDone) dialogueBox.SetActive(true);
        else dialogueBox2.SetActive(true);
        
        dialogueStarted = true;

        dialogueComponent.StartDialogue();
    }

    public void EndDialogue()
    {
        GameManager.Instance.IsPlayerPaused = false;
        dialogueBox.SetActive(false);
        dialogueBox2.SetActive(false);

        dialogueStarted = false;

        if(!isFirstPartDone)
        {
            //player.trapID = 1;
            //player.playerIsTrapped = true;
            //player.GetComponent<Animator>().SetTrigger("Trapped1");
            isFirstPartDone = true;

        }
        else
        {
            Animator anim = GetComponentInParent<Animator>();
            anim.SetTrigger("Attack");
            totemScript.isActive = true;
            totemScript.anim.SetTrigger("Active");
            this.gameObject.SetActive(false);
        }
    }
}
