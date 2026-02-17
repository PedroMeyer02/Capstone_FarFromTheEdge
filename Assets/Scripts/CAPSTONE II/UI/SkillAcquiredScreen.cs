using System.Collections;
using UnityEngine;

public class SkillAcquiredScreen : MonoBehaviour
{
    Animator UIAnim;
    
    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.X))
        if(Input.anyKey)
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
        
        this.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        GameManager.Instance.IsPlayerPaused = false;
    }
}
