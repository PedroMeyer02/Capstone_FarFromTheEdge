using UnityEngine;
using UnityEngine.SceneManagement;

public class SurvivalMechanic : MonoBehaviour
{
    Collider col;

    public bool groundCheckLP = false;
    //bool groundCheckPA = false;

    public bool activateDamage = false;

    public float survivalTimer = 0f;
    public int survivalCount = 0;

    Animator anim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(activateDamage)
        {
            if(GameManager.Instance.IsPlayerPaused)
            {
                return;
            }

            if (!GameManager.Instance.IsPlayerPaused)
            {
                survivalTimer += Time.deltaTime;
            }


            if(survivalTimer >= 5f)
            {
                survivalCount++;
                survivalTimer = 0f;
                anim.SetTrigger("Glitch");
            }

            if (survivalCount >= 9)
            {
                SceneManager.LoadScene("EndScene");
            }
        }
        else
        {
            survivalTimer = 0f;
            survivalCount = 0;
        }
       
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LP"))
        {
            groundCheckLP = true;
            //groundCheckPA = false;
        }
        
        if(other.CompareTag("PA"))
        {
            groundCheckLP = false;
            //groundCheckPA = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("LP") && groundCheckLP)
        {
            activateDamage = true;
               
        }

        else if(other.CompareTag("PA"))
        {
            activateDamage = false;
        }
    }
}
