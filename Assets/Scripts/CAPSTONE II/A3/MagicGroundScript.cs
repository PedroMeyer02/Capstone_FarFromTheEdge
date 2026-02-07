using Unity.VisualScripting;
using UnityEngine;

public class MagicGroundScript : MonoBehaviour
{
    Collider col;
    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        anim.SetBool("Active", true);
    }

    private void OnTriggerExit(Collider other)
    {
        anim.SetBool("Active", false);

    }
}
