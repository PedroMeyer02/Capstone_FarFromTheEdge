using System.Collections;
using UnityEngine;

public class TotemScript : MonoBehaviour
{
    Collider col;
    public Animator anim;

    public GameObject newCamera;
    public bool isActive = false;

    Player player;
    public DialogueControl control;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        player = FindAnyObjectByType<Player>();
    }

    public void ActivateDialogue()
    {
        control.DialogueStart();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (!isActive)
        //{
        //    isActive = true;
        //    anim.SetTrigger("Active");
        //}

        if(other.CompareTag("Skill1"))
        {
            StartCoroutine(Skill2Acquired());
            col.enabled = false;
        }
    }

    IEnumerator Skill2Acquired()
    {
        GameManager.Instance.A2Skill2Acquired = true;
        GameManager.Instance.IsPlayerPaused = true;
        GameManager.Instance.CameraToObject(newCamera);

        yield return new WaitForSeconds(1f);
        player.GetComponent<Animator>().SetTrigger("OrbAcquisition");
        
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.IsPlayerPaused = true;
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.IsPlayerPaused = true;
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.IsPlayerPaused = true;

        yield return new WaitForSeconds(3f);
        anim.SetTrigger("Collected");
        GameManager.Instance.CameraToCharacter();
        GameManager.Instance.UIManager.skill2Acquired.SetActive(true);
        
        StopAllCoroutines();
    }
}
