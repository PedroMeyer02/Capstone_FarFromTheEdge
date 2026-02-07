using UnityEngine;

public class ShiftingAltar : MonoBehaviour
{
    Collider col;
    Animator anim;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill3"))
        {
            GameManager.Instance.A4PortalsActives++;
            anim.SetTrigger("Activate");
            col.enabled = false;
        }
    }
}
