using UnityEngine;

public class StatuesRevivalScript : MonoBehaviour
{
    Collider col;
    Animator anim;
    public GameObject obj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill3"))
        {
            anim.SetTrigger("Activate");
            col.enabled = false;
            Destroy(obj, 2f);
        }
    }
}
