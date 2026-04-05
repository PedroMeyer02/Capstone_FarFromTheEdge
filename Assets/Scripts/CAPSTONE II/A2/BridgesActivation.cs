using UnityEngine;

public class BridgesActivation : MonoBehaviour
{
    public BridgeStates bridgeStates;

    Collider col;
    Animator anim;

    bool playerEntered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
        
        if (bridgeStates == BridgeStates.Platforms) anim = GetComponentInChildren<Animator>();
        if (bridgeStates == BridgeStates.Wanderer) anim = GetComponent<Animator>();
        if (bridgeStates == BridgeStates.Portal)   anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && bridgeStates == BridgeStates.Wanderer && !playerEntered)
        {
            anim.SetBool("isDestroyed", true);
            playerEntered = true;
        }
        
        if (other.CompareTag("Skill2") && bridgeStates == BridgeStates.Wanderer)
        {
            anim.SetBool("isDestroyed", false);
            AudioManager.Instance.PlayWall();

        }

        if (other.CompareTag("Skill2") && bridgeStates == BridgeStates.Platforms)
        {
            anim.SetTrigger("Bridge");
            AudioManager.Instance.PlayWall();

        }

        if (other.CompareTag("Skill2") && bridgeStates == BridgeStates.Portal)
        {
            anim.SetTrigger("Activate");
            AudioManager.Instance.PlayWall();

        }
    }
}

public enum BridgeStates
{
    Wanderer, Platforms, Portal
}
