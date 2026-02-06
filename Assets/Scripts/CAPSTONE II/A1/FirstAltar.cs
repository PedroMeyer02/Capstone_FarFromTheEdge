using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FirstAltar : MonoBehaviour
{
    Collider col;
    [SerializeField] GameObject standingPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        if (GameManager.Instance.A1Skill1Acquired)
        {
            col.enabled = false;
            standingPosition.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        standingPosition.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        standingPosition.SetActive(false);
    }

}
