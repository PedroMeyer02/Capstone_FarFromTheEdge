using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class TeleportingPlatform : MonoBehaviour
{
    Collider col;
    public Transform newPosition;

    Player player;

    //GameObject player;

    bool portalActive = false;

    VisualEffect teleportEffect;
    public Light activeLight;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<Player>();

        col = GetComponent<Collider>();
        teleportEffect = GetComponentInChildren<VisualEffect>();

        teleportEffect.Stop();

        //if(player == null )
        //{
        //   player = this.gameObject;
        //}
    }

    private void Update()
    {
        if (GameManager.Instance.A3PortalActive && !portalActive)
        {
            teleportEffect.Play();
            activeLight.enabled = true;
            portalActive = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.A3PortalActive && other.CompareTag("Player"))
        {
            //player = other.gameObject;
            StartCoroutine(A3PortalAction());
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
        AudioManager.Instance.PlayTeleport();
        yield return new WaitForSeconds(1f);
        player.transform.position = newPosition.position;
        //player = this.gameObject;
        yield return new WaitForSeconds(1f);
        GameManager.Instance.IsPlayerPaused = false;

        //teleportEffect.Stop();

        StopAllCoroutines();
    }
}
