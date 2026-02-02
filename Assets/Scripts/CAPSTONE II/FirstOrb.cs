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
        player.GetComponent<Animator>().SetTrigger("OrbAcquisition");
        orbAnimator.SetTrigger("Activate");
        orbEffect.Play();
        yield return new WaitForSeconds(4f);
        orbEffect.Stop();
        mistEffect.Stop();
        mistDissipate.Play();
        yield return new WaitForSeconds(1f);
        GameManager.Instance.IsPlayedPaused = false;
        col.gameObject.SetActive(false);
        GameManager.Instance.A1OrbAcquired = true;

        player = this.gameObject;
        StopAllCoroutines();
    }
}
