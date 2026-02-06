using UnityEngine;
using UnityEngine.VFX;

public class BreakWallScript : MonoBehaviour
{

    bool isWallOpen = false;
    public Collider col;
    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill2") && !isWallOpen)
        {
            anim.SetTrigger("Activate");
            isWallOpen = true;
            col.enabled = false;
        }
    }
}
