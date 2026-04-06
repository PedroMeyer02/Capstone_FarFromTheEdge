using UnityEngine;

public class A4Portal : MonoBehaviour
{
    Collider col;
    Animator anim;
    bool isActive = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.A4PortalsActives == 0)
        {
            anim.SetInteger("Activation", 0);
        }
        if (GameManager.Instance.A4PortalsActives == 1)
        {
            anim.SetInteger("Activation", 1);
        }
        if (GameManager.Instance.A4PortalsActives == 2)
        {
            anim.SetInteger("Activation", 2);
        }
        if (GameManager.Instance.A4PortalsActives == 3)
        {
            anim.SetInteger("Activation", 3);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill3") && !isActive && GameManager.Instance.A4PortalsActives >= 3)
        {
            anim.SetTrigger("Active");
            isActive = true;
            col.enabled = false;
        }
    }
}
