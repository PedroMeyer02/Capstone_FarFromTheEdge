using System.Collections;
using UnityEngine;

public class SkillAcquiredScreen : MonoBehaviour
{
    Animator UIAnim;

    float timer = 0;

    public int skillID = 0;

    [SerializeField] GameObject skill1;
    [SerializeField] GameObject skill2;
    [SerializeField] GameObject skill3;

    
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //if(Input.GetKeyDown(KeyCode.X))
        if(Input.anyKey && timer >= 2f)
        {
            StartCoroutine(FadeOutAnim());
        }
    }
    private void OnEnable()
    {
        UIAnim = GetComponent<Animator>();
        GameManager.Instance.IsPlayerPaused = true;
    }

    IEnumerator FadeOutAnim()
    {
        UIAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);

        if(skillID == 2)
        {
            GameManager.Instance.IsPlayerPaused = true;
            GameManager.Instance.A2FinalDialogueActivate = true;
            TotemScript script = FindAnyObjectByType<TotemScript>();
            script.ActivateDialogue();
        }

        if(skillID == 3)
        {
            GameManager.Instance.A4FinalDialogueActivate = true;
        }

        this.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        if(skillID == 1)
        {
            skill1.SetActive(true);
            GameManager.Instance.IsPlayerPaused = false;
        }
        if (skillID == 2)
        {
            skill2.SetActive(true);
            GameManager.Instance.IsPlayerPaused = true;
        }
        if (skillID == 3)
        {
            skill3.SetActive(true);
            GameManager.Instance.IsPlayerPaused = false;
        }
    }
}
