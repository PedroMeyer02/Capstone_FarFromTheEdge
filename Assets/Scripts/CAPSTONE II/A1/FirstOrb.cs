using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class FirstOrb : MonoBehaviour
{
    Collider col;
    [SerializeField] VisualEffect orbEffect;
    [SerializeField] Animator orbAnimator;
    public GameObject newCamera;

    public bool isTriggered = false;

    Player player;

    private void Start()
    {
        col = GetComponent<Collider>();
        player = FindAnyObjectByType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isTriggered)
        {
            StartCoroutine(OrbAcquistion());
            isTriggered = true;
        }
    }


    IEnumerator OrbAcquistion()
    {
        GameManager.Instance.IsPlayerPaused = true;
        GameManager.Instance.A1Skill1Acquired = true;
        GameManager.Instance.CameraToObject(newCamera);
        orbAnimator.SetTrigger("Activate");

        AudioManager.Instance.PlayOrb();

        yield return new WaitForSeconds(1f);

        player.GetComponent<Animator>().SetTrigger("OrbAcquisition");
        orbEffect.Play();

        yield return new WaitForSeconds(3f);

        orbEffect.Stop();

        GameManager.Instance.UIManager.skill1Acquired.SetActive(true);

        yield return new WaitForSeconds(1f);

        GameManager.Instance.CameraToCharacter();
        col.gameObject.SetActive(false);


        StopAllCoroutines();
    }
}
