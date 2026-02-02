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
        player = this.gameObject;
    }


    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(OrbAcquistion());
        player = other.gameObject;
    }


    IEnumerator OrbAcquistion()
    {
        player.GetComponent<Animator>().SetTrigger("Teleport");
        GameManager.Instance.IsPlayedPaused = true;
        orbAnimator.SetTrigger("Activate");
        orbEffect.Play();
        yield return new WaitForSeconds(4f);
        orbEffect.Stop();
        mistEffect.Stop();
        mistDissipate.Play();
        yield return new WaitForSeconds(1f);
        GameManager.Instance.IsPlayedPaused = false;

        player = this.gameObject;
        StopAllCoroutines();
    }
}
