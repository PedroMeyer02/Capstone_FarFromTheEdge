using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    // change starting points based on quest state
    public int startIndexInProgressPhase = 4;
    public int startIndexInCompletedPhase = 5;

    public GameObject[] dialogueText;
    public TextMeshProUGUI[] textComponent;

    public Sprite[] sprites;
    public Image dialogueSprite;

    public GameObject[] characterName;
    public GameObject[] characterIcon;

    public int index = 0;

    public DialogueControl control;
    private bool isTyping = false;
    private string text;
    private Coroutine typingCoroutine;

    // Variables to handle dialogue lines per quest stage
    int endDialogueCount = 0;

    public void StartDialogue()
    {
        // Set boundaries for player dialogues
        endDialogueCount = dialogueText.Length - 1;

        index = 0;
        DeactivateTexts();
        dialogueText[0].SetActive(true);
        dialogueSprite.sprite = sprites[0];
        characterName[0].SetActive(true);
        characterIcon[0].SetActive(true);

        text = textComponent[0].text;
        textComponent[0].text = "";
        typingCoroutine = StartCoroutine(TypeLine(text));
    }

    // Deactivate all texts to get to the next one correctly
    public void DeactivateTexts()
    {
        for (int i = 0; i < dialogueText.Length; i++)
        {
            dialogueText[i].SetActive(false);
        }

        for (int i = 0; i < characterName.Length; i++)
        {
            characterName[i].SetActive(false);
        }

        for (int i = 0; i < characterIcon.Length; i++)
        {
            characterIcon[i].SetActive(false);
        }
    }

    public void NextText()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            textComponent[index].text = text;
            isTyping = false;
            return;
        }

        if (index < endDialogueCount)
        {
            index++;
            DeactivateTexts();

            dialogueSprite.sprite = sprites[index];
            characterName[index].SetActive(true);
            dialogueText[index].SetActive(true);
            characterIcon[index].SetActive(true);

            text = textComponent[index].text;
            textComponent[index].text = "";
            typingCoroutine = StartCoroutine(TypeLine(text));
        }
        else
        {
            control.EndDialogue();
            StopAllCoroutines();
        }
    }

    /// <summary>
    /// Types the text character by character using the text speed in the game manager.
    /// The text speed is in the game manager so it can be changed in the settings later.
    /// </summary>
    /// <param name="text">temporary one just so we can write the text up in the inspector and it will not be cut out</param>
    IEnumerator TypeLine(string text)
    {
        GameManager.Instance.IsPlayerPaused = true;

        isTyping = true;
        textComponent[index].text = "";

        foreach (char c in text.ToCharArray())
        {
            textComponent[index].text += c;
            yield return new WaitForSeconds(GameManager.Instance.textSpeed);
        }


        isTyping = false;
    }
}
