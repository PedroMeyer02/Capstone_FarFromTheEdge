using UnityEngine;

public class TransitionScene : MonoBehaviour
{
    float timer = 0f;
    float keyCooldown = 0f;
    public int index = 0;


    bool timerEnd = false;

    Animator anim;
    BackgroundLoad bgLoad;

    public GameObject playImage;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        bgLoad = FindAnyObjectByType<BackgroundLoad>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!timerEnd)
        {
            timer += Time.deltaTime;
            keyCooldown += Time.deltaTime;

            if (timer > 2)
            {
                index++;
                UpdateAnimation();
                timer = 0f;
            }


            if (Input.anyKey && keyCooldown > 0.2f)
            {
                index++;
                UpdateAnimation();
                keyCooldown = 0f;
            }

        }
    }

    void UpdateAnimation()
    {
        if (index == 1) anim.SetTrigger("Image02");
        if (index == 2) anim.SetTrigger("Image03");
        if (index == 3) anim.SetTrigger("Image04");
        if (index == 4) anim.SetTrigger("Image05");

        if (index >= 5)
        {
            anim.SetTrigger("Image06");
            timerEnd = true;
            playImage.SetActive(true);
            bgLoad.isAnimationEnd = true; 
        }
    }
}
