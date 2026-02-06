using UnityEngine;

public class BreatheAnim : MonoBehaviour
{
    public Animator animator;

    public float breatheTime = 0f;
    public float randomTime = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        randomTime = Random.Range(2f, 4f);
        breatheTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        breatheTime += Time.deltaTime;
        
        if (breatheTime > randomTime)
        {
            animator.SetTrigger("Breathe");
            RandomizeTime();
            breatheTime = 0f;
        }
    }

    public void RandomizeTime()
    {
        randomTime = Random.Range(3f, 6f);
    }
}
