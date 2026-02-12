using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public enum CharacterType { Crafter, Alchemist, Player }

public class DialogueControl : MonoBehaviour
{
    [Header("Dialogue Type")]
    public CharacterType characterType;
    
    [Header("Dialogue Settings")]
    public GameObject dialogueBox;
    //public Quest quest;

    bool dialogueStarted = false;
    bool playerHit = false;
    bool canInteract = true;

    // For each player self-dialogue event
    public bool selfDialogueEventComplete = false;

    [Header("Collision Settings")]
    public float dialogueRange = 2f;
    public LayerMask playerLayer;
    private Vector3 position;

    [Header("Activate Object for This Dialogue - OPT.")]
    public GameObject objectToActivate;

    private Dialogue dialogueComponent;

    void Awake()
    {
        dialogueComponent = dialogueBox.GetComponent<Dialogue>();
    }

    void Update()
    {
        //if (characterType == CharacterType.Crafter)
        //{
        //    // Stop interaction if quest is completed (ONLY FOR THE CRAFTER)
        //    if (quest.state == QuestState.Completed && !GameManager.Instance.Quest1ReadytoComplete)
        //        return;
        //}
        //else if (characterType == CharacterType.Alchemist)
        //{
        //    //// Stop interaction if quest is completed (ONLY FOR THE ALCHEMIST)
        //    if (quest.state == QuestState.Completed && !GameManager.Instance.Quest2ReadytoComplete)
        //        return;
        //}


        ShowDialogue();

        if (characterType == CharacterType.Crafter && playerHit && !dialogueStarted && canInteract && Keyboard.current.eKey.wasPressedThisFrame)
        {
            GameManager.Instance.IsPlayerPaused = true;

            //if (quest.state == QuestState.InProgress)
            //{
            //    if (!selfDialogueEventComplete && objectToActivate != null)
            //        objectToActivate.SetActive(true);
            //}

            // check quest state for completion here
            //if (quest.state == QuestState.InProgress && GameManager.Instance.Quest1ReadytoComplete)
            //{
            //    quest.state = QuestState.Completed; // show the completion dialogue
            //    GameManager.Instance.Quest1ReadytoComplete = false;
            //}

            dialogueBox.SetActive(true);
            dialogueStarted = true;
            canInteract = false;

            // Start the dialogue
            //dialogueComponent.StartQuestDialogue(quest);

            var playerInput = GetComponent<PlayerInput>();
            if (playerInput != null)
                playerInput.DeactivateInput();
        }
        else if (characterType == CharacterType.Alchemist && playerHit && !dialogueStarted && canInteract && Keyboard.current.eKey.wasPressedThisFrame)
        {
            GameManager.Instance.IsPlayerPaused = true;

            //if (quest.state == QuestState.InProgress)
            //{
            //    if (!selfDialogueEventComplete && objectToActivate != null)
            //        objectToActivate.SetActive(true);
            //}

            // check quest state for completion here
            //if (quest.state == QuestState.InProgress && GameManager.Instance.Quest2ReadytoComplete)
            //{
            //    quest.state = QuestState.Completed; // show the completion dialogue
            //    GameManager.Instance.Quest2ReadytoComplete = false;
            //}

            dialogueBox.SetActive(true);
            dialogueStarted = true;
            canInteract = false;

            // Start the dialogue
            //dialogueComponent.StartQuestDialogue(quest);

            var playerInput = GetComponent<PlayerInput>();
            if (playerInput != null)
                playerInput.DeactivateInput();
        }
        else if (characterType == CharacterType.Player && playerHit && !dialogueStarted && !selfDialogueEventComplete)
        {
            dialogueBox.SetActive(true);
            dialogueStarted = true;

            // activate the object after dialogue (The Blue Orb for example) - OPTIONAL
            if (!selfDialogueEventComplete && objectToActivate != null)
                objectToActivate.SetActive(true);

            // Start the dialogue
            dialogueComponent.StartPlayerDialogue();

            var playerInput = GetComponent<PlayerInput>();
            if (playerInput != null)
                playerInput.DeactivateInput();
        }

        // PLAYER SELF-DIALOGUE ADVANCING
        if (dialogueStarted && characterType == CharacterType.Player)
        {
            GameManager.Instance.IsPlayerPaused = true;

            if (Keyboard.current.eKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
            {
                dialogueComponent.NextText();
            }
        }

        // Wait for E to be released before reenabling interaction (for NPCs)
        if (!Keyboard.current.eKey.isPressed && dialogueStarted == false)
        {
            canInteract = true;
        }
    }

    void ShowDialogue()
    {
        position = transform.position;
        Collider[] hit = Physics.OverlapSphere(position, dialogueRange, playerLayer);
        playerHit = hit.Length > 0;
    }

    public void EndDialogue()
    {
        GameManager.Instance.IsPlayerPaused = false;
        dialogueBox.SetActive(false);

        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
            playerInput.ActivateInput();

        // Update quest progress
        if (characterType == CharacterType.Player)
        {
            selfDialogueEventComplete = true;
        }
        else if (characterType == CharacterType.Crafter)
        {
            //if (quest.state == QuestState.NotStarted && dialogueStarted)
            //{
            //    quest.state = QuestState.InProgress;

            //    // Give the pickaxe here FOR THE CRAFTER
            //    GameManager.Instance.PickaxeItem += 1;
            //}
            //else if (quest.state == QuestState.InProgress && GameManager.Instance.CheckArea3Quest())
            //{
            //    GameManager.Instance.Quest1ReadytoComplete = true;
            //}
            //else if (quest.state == QuestState.Completed && dialogueStarted)
            //{
            //    // Quest fully done, give rewards
            //    GameManager.Instance.OreItemCount = 0;
            //    GameManager.Instance.FireOrbItem += 1;
            //}
        }
        else if (characterType == CharacterType.Alchemist)
        {
            //if (quest.state == QuestState.NotStarted && dialogueStarted)
            //{
            //    quest.state = QuestState.InProgress;

            //    // Alchemist takes this from you
            //    GameManager.Instance.FireOrbItem = 0;

            //    if (GameManager.Instance.PedalItemCount < 5 || GameManager.Instance.BlueOreItemCount < 5)
            //    {
            //        // Give initial items if player doesn't have enough
            //        GameManager.Instance.PedalItemCount = 5;
            //        GameManager.Instance.BlueOreItemCount = 5;
            //    }
            //}
            //else if (quest.state == QuestState.InProgress && GameManager.Instance.CheckAlchemistQuest())
            //{
            //    GameManager.Instance.Quest2ReadytoComplete = true;
            //}
            //else if (quest.state == QuestState.Completed && dialogueStarted)
            //{
            //    GameManager.Instance.BlueOreItemCount = 0;
            //    GameManager.Instance.PedalItemCount = 0;
            //    GameManager.Instance.SightAbilityUnlocked = true;
            //}
        }

        dialogueStarted = false;
    }
}
