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
        }

        if (other.CompareTag("Skill2") && bridgeStates == BridgeStates.Platforms)
        {
            anim.SetTrigger("Bridge");
        }
    }
}

public enum BridgeStates
{
    Wanderer, Platforms
}
