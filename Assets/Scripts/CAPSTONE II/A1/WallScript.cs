using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class WallScript : MonoBehaviour
{
    public GameObject newCamera;

    Animator wallAnim;
    VisualEffect wallEffect;

    bool isWallOpen = false;
    public Collider col;
    Animator anim;

    bool isWallOpening = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wallAnim = GetComponent<Animator>();
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        wallEffect = GetComponentInChildren<VisualEffect>();
    }

    private void Update()
    {
        if(isWallOpening)
        {
            GameManager.Instance.IsPlayerPaused = true;
        }
        else if (isWallOpen && !isWallOpening)
        {
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Skill1") && !isWallOpen)
        {
            StartCoroutine(WallOpen());
            isWallOpening = true;
            isWallOpen = true;
            col.enabled = false;
        }
    }
    
    IEnumerator WallOpen()
    {
        GameManager.Instance.IsPlayerPaused = true;
        GameManager.Instance.CameraToObject(newCamera);

        yield return new WaitForSeconds(1f);

        wallAnim.SetTrigger("Activate");
        wallEffect.Play();

        yield return new WaitForSeconds(3f);

        wallEffect.Stop();

        yield return new WaitForSeconds(1f);

        GameManager.Instance.CameraToCharacter();
        isWallOpening = false;
        GameManager.Instance.IsPlayerPaused = false;
        StopAllCoroutines();
    }





}
