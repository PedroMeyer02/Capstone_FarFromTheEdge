using System.Collections;
using UnityEngine;

public class ThirdAltar : MonoBehaviour
{
    Collider col;

    Player player;


    void Start()
    {
        col = GetComponent<Collider>();
        player = FindAnyObjectByType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
            StartCoroutine(Skill3Acquired());
            col.enabled = false;
    }

    IEnumerator Skill3Acquired()
    {
        GameManager.Instance.IsPlayerPaused = true;
        GameManager.Instance.A4Skill3Acquired = true;
        player.GetComponent<Animator>().SetTrigger("OrbAcquisition");

        yield return new WaitForSeconds(2f);
        GameManager.Instance.UIManager.skill3Acquired.SetActive(true);

        StopAllCoroutines();
    }
}
