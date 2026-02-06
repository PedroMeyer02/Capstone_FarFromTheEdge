using UnityEngine;
using UnityEngine.VFX;

public class SecondAltar : MonoBehaviour
{
    VisualEffect altarEffect;

    bool isAltarActive = false;
    public Collider col;
    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        altarEffect = GetComponentInChildren<VisualEffect>();
        altarEffect.Stop();
        
        col = GetComponent<Collider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill1") && !isAltarActive)
        {
            GameManager.Instance.A3PortalActive = true;
            altarEffect.Play();
            isAltarActive = true;
            col.enabled = false;
        }
    }
}
