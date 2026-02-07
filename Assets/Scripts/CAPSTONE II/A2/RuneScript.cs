using UnityEngine;

public class RuneScript : MonoBehaviour
{
    public LoreStatues loreStatues;
    public GameObject UIDialogue;
    DialogueControl dialogueControl;

    Collider col;
    Animator anim;
    float timer;

    bool isActive = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        dialogueControl = GetComponentInChildren<DialogueControl>();
    }

    private void Update()
    {
        if (anim.GetBool("Active") == true)
        {
            timer += Time.deltaTime;
            if(timer > 5 && !UIDialogue.activeSelf)
            {
                anim.SetBool("Active", false);
                timer = 0;
                isActive = false;
                col.enabled = true;
            }
        }

        if (dialogueControl.selfDialogueEventComplete && loreStatues == LoreStatues.Wanderer)
        {
            dialogueControl.selfDialogueEventComplete = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill1") && !isActive)
        {
            anim.SetBool("Active", true);
            isActive = true;
            col.enabled = false;
        }
    }

}

public enum LoreStatues
{
    Wanderer, A3, A4, A5
}
