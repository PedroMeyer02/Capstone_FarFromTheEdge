using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public TrapState trapState;
    
    Collider col;
    Player player;
    bool trapActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
        player = FindAnyObjectByType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!trapActive && trapState == TrapState.Wanderer)
        {
            player.trapID = 1;
            player.playerIsTrapped = true;
            player.GetComponent<Animator>().SetTrigger("Trapped1");
            trapActive = true;
            col.enabled = false;
        }

        if (!trapActive && trapState == TrapState.Alchemist)
        {
            player.trapID = 2;
            player.playerIsTrapped = true;
            player.GetComponent<Animator>().SetTrigger("Trapped2");
            trapActive = true;
            col.enabled = false;
        }
    }
}

public enum TrapState
{
    Wanderer, Alchemist
}