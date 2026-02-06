using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class TeleportingPlatform : MonoBehaviour
{
    Collider col;
    public Transform newPosition;
    GameObject player;
    VisualEffect teleportEffect;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
        teleportEffect = GetComponentInChildren<VisualEffect>();

        teleportEffect.Stop();

        if(player == null )
        {
           player = this.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.A3PortalActive)
        {
            player = other.gameObject;
            StartCoroutine(A3PortalAction());
            teleportEffect.Play();
        }
        else
        {
            Debug.Log("Portals are not active");
        }
    }

    IEnumerator A3PortalAction()
    {
        GameManager.Instance.IsPlayerPaused = true;
        player.GetComponent<Animator>().SetTrigger("Teleport");
        yield return new WaitForSeconds(1f);
        player.transform.position = newPosition.position;
        player = this.gameObject;
        yield return new WaitForSeconds(1f);
        GameManager.Instance.IsPlayerPaused = false;

        teleportEffect.Stop();

        StopAllCoroutines();
    }
}
