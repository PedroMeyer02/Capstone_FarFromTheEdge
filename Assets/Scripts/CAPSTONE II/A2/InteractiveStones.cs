using UnityEngine;

public class InteractiveStones : MonoBehaviour
{
    public EasingController[] easingControllers;
    public bool isActive = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        easingControllers = GetComponentsInChildren<EasingController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill1"))
        {
            if (!isActive)
            {
                foreach (EasingController controller in easingControllers)
                {
                    controller.enabled = true;
                }
                isActive = true;
            }
            else
            {
                foreach (EasingController controller in easingControllers)
                {
                    controller.enabled = false;
                }
                isActive = false;
            }

        }
    }
}
