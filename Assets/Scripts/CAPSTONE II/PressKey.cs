using UnityEngine;

public class PressKey : MonoBehaviour
{
    Collider col;
    [SerializeField] GameObject KeyObject;
    public KeyPressed keyPressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        if (GameManager.Instance.A1WallOpen && keyPressed == KeyPressed.one)
        {
            col.enabled = false;
            KeyObject.SetActive(false);
        }

        if(!GameManager.Instance.A2Skill2Acquired && keyPressed == KeyPressed.two)
        {
            col.enabled = false;
        }
        
        if (GameManager.Instance.A2Skill2Acquired && keyPressed == KeyPressed.two)
        {
            col.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        KeyObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        KeyObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill2"))
        {
            col.enabled = false;
            KeyObject.SetActive(false);
        }

        if (other.CompareTag("Skill3"))
        {
            col.enabled = false;
            KeyObject.SetActive(false);
        }
    }
}

public enum KeyPressed
{
    one, two, three
}
