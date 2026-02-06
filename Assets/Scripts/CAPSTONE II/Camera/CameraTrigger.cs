using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public GameObject newCamera;
    Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.CameraToObject(newCamera);
    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.Instance.CameraToCharacter();
    }
}
