using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SurvivalMechanic : MonoBehaviour
{
    //Post-processing settings
    [SerializeField] Volume renderVolume;
    Vignette vignette;
    ChromaticAberration chromaticAberration;
    Bloom bloom;
    FilmGrain filmGrain;
    float chromaticIntensity = 0f;
    float vignetteIntensity = 0f;

    Collider col;

    public bool groundCheckLP = false;
    public bool groundCheckPA = false;

    public bool activateDamage = false;

    public float survivalTimer = 0f;
    public int survivalCount = 0;

    Animator anim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponentInParent<Animator>();

        renderVolume.profile.TryGet(out  vignette);
        renderVolume.profile.TryGet(out chromaticAberration);
        renderVolume.profile.TryGet(out bloom);
        renderVolume.profile.TryGet(out filmGrain);
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


            if(survivalTimer >= 3f)
            {
                survivalCount++;
                survivalTimer = 0f;
                anim.SetTrigger("Glitch");
            }

            if (survivalCount >= 7)
            {
                SceneManager.LoadScene("02_LoseScene");
            }

            chromaticIntensity = Mathf.Lerp(0f, 0.1f, 1f);
            chromaticAberration.intensity.value = chromaticIntensity;

            vignetteIntensity = Mathf.Lerp(0f, 0.4f, 1f);
            vignette.intensity.value = vignetteIntensity;

            filmGrain.intensity.value = 1;
            bloom.intensity.value = 1;
        }
        else
        {
            survivalTimer = 0f;
            survivalCount = 0;

            chromaticIntensity = 0;
            chromaticAberration.intensity.value = chromaticIntensity;

            vignetteIntensity = 0;
            vignette.intensity.value = vignetteIntensity;

            filmGrain.intensity.value = 0;
            bloom.intensity.value = 0;
        }
       
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LP"))
        {
            groundCheckLP = true;
        }

        else if (other.CompareTag("PA"))
        {
            groundCheckPA = true;
        }

        CheckCondition();

        //if(other.CompareTag("LP"))
        //{
        //   groundCheckLP = true;
        //   //groundCheckPA = false;
        //}
        
        //else if(other.CompareTag("PA"))
        //{
        //    groundCheckLP = false;
        //    //groundCheckPA = true;
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("LP") && groundCheckLP)
        //{
        //    activateDamage = true;

        //}

        //else if (other.CompareTag("PA"))
        //{
        //    activateDamage = false;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("LP"))
        {
            groundCheckLP = false;
        }

        else if (other.CompareTag("PA"))
        {
            groundCheckPA = false;
        }

            CheckCondition();

    }

    private void CheckCondition()
    {
        Debug.Log("ConditionChecked");
        
        if(groundCheckLP && groundCheckPA)
        {
            activateDamage = false;
            Debug.Log("ConditionChecked _ 01");

        }

        else if (!groundCheckLP && groundCheckPA)
        {
            activateDamage = false;
            Debug.Log("ConditionChecked _ 02");
        }

        else if (groundCheckLP && !groundCheckPA)
        {
            activateDamage = true;
            Debug.Log("ConditionChecked _ 03");
        }

        else 
        { 
            activateDamage = false;
            Debug.Log("ConditionChecked _ 04");
        }
    }
}
