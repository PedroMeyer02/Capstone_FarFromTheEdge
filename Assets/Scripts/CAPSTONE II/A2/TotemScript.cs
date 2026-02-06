using System.Collections;
using UnityEngine;

public class TotemScript : MonoBehaviour
{
    Collider col;
    Animator anim;

    public GameObject newCamera;
    bool isActive = false;

    Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        player = FindAnyObjectByType<Player>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            isActive = true;
            anim.SetTrigger("Active");
        }

        if(other.CompareTag("Skill1"))
        {
            StartCoroutine(Skill2Acquired());
            col.enabled = false;
        }
    }

    IEnumerator Skill2Acquired()
    {
        GameManager.Instance.IsPlayerPaused = true;
        GameManager.Instance.A2Skill2Acquired = true;
        GameManager.Instance.CameraToObject(newCamera);
        player.GetComponent<Animator>().SetTrigger("OrbAcquisition");
        
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("Collected");
        GameManager.Instance.CameraToCharacter();

        yield return new WaitForSeconds(1f);
        GameManager.Instance.UIManager.skill2Acquired.SetActive(true);
        
        StopAllCoroutines();
    }
}
