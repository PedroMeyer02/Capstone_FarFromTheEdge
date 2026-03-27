using UnityEngine;

public class PixelInteraction : MonoBehaviour
{
    [SerializeField]Collider col;
    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill1"))
        {
                anim.SetTrigger("StateChange");
        }
    }
}
