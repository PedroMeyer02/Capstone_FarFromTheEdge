using UnityEngine;

public class RandomizeAnim : MonoBehaviour
{
    Animator anim;
    float timer = 0f;
    float goalTime = 0f;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > goalTime)
        {
            anim.SetTrigger("Activate");
            RandomizeTimer();
            timer = 0f;
        }
    }

    void RandomizeTimer()
    {
        goalTime = Random.Range(.5f, 1.0f);
    }
}
