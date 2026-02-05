using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class FirstOrb : MonoBehaviour
{
    Collider col;
    [SerializeField] VisualEffect orbEffect;
    [SerializeField] VisualEffect mistEffect;
    [SerializeField] VisualEffect mistDissipate;
    [SerializeField] Animator orbAnimator;
    public GameObject newCamera;
    GameObject player;

    private void Start()
    {
        col = GetComponent<Collider>();

        if (player == null)
        {
            player = this.gameObject;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        player = other.gameObject;

        StartCoroutine(OrbAcquistion());
    }


    IEnumerator OrbAcquistion()
    {
        GameManager.Instance.IsPlayedPaused = true;
        GameManager.Instance.A1Skill1Acquired = true;
        GameManager.Instance.CameraToObject(newCamera);
        orbAnimator.SetTrigger("Activate");

        yield return new WaitForSeconds(1f);

        player.GetComponent<Animator>().SetTrigger("OrbAcquisition");
        orbEffect.Play();

        yield return new WaitForSeconds(3f);

        orbEffect.Stop();
        mistEffect.Stop();
        mistDissipate.Play();

        yield return new WaitForSeconds(1f);

        GameManager.Instance.CameraToCharacter();
        GameManager.Instance.IsPlayedPaused = false;
        col.gameObject.SetActive(false);
        player = this.gameObject;

        GameManager.Instance.UIManager.skill1Acquired.SetActive(true);

        StopAllCoroutines();
    }
}
