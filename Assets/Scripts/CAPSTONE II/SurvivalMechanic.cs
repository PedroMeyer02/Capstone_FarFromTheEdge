using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class SurvivalMechanic : MonoBehaviour
{
    //Post-processing settings
    [SerializeField] Volume renderVolume;
    Vignette vignette;
    ChromaticAberration chromaticAberration;
    Bloom bloom;
    FilmGrain filmGrain;

    Collider col;

    public bool groundCheckLP = false;
    public bool groundCheckPA = false;

    public bool activateDamage = false;

    public float survivalTimer = 0f;
    public float safeTimer = 0f;
    public int survivalCount = 0;

    public float currentVignette = 0f;
    public float currentCA = 0f;
    public float currentBlue = 0f;

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
            safeTimer = 0;


            if (GameManager.Instance.IsPlayerPaused)
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
                StartCoroutine(LoseConditionActivated());
            }

            //chromaticIntensity = Mathf.Lerp(0f, 0.1f, 1f);
            //chromaticAberration.intensity.value = chromaticIntensity;

            //vignetteIntensity = Mathf.Lerp(0f, 0.4f, 1f);
            //vignette.intensity.value = vignetteIntensity;

            //filmGrain.intensity.value = 1;
            //bloom.intensity.value = 1;
        }
        else
        {
            survivalTimer = 0f;
            safeTimer += Time.deltaTime;
            survivalCount = 0;

            //chromaticIntensity = 0;
            //chromaticAberration.intensity.value = chromaticIntensity;

            //vignetteIntensity = 0;
            //vignette.intensity.value = vignetteIntensity;

            //filmGrain.intensity.value = 0;
            //bloom.intensity.value = 0;
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
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("LP"))
        {
            groundCheckLP = true;
        }

        if (other.CompareTag("PA"))
        {
            groundCheckPA = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
       if(other.CompareTag("LP"))
       {
           groundCheckLP = false;
       }

       if (other.CompareTag("PA"))
       {
            groundCheckPA = false;
       }

            CheckCondition();

    }

    private void CheckCondition()
    {
        
        if(groundCheckLP && groundCheckPA)
        {
            activateDamage = false;
            //Debug.Log("ConditionChecked _ 01");
        }

        else if (!groundCheckLP && groundCheckPA)
        {
            activateDamage = false;
            //Debug.Log("ConditionChecked _ 02");
        }

        else if (groundCheckLP && !groundCheckPA)
        {
            activateDamage = true;
            //Debug.Log("ConditionChecked _ 03");
        }

        else 
        { 
            activateDamage = false;
            //Debug.Log("ConditionChecked _ 04");
        }

        ChangeState();
    }

    private void ChangeState()
    {
        //Debug.Log("StateActivated");

        if (activateDamage && survivalTimer <= 0.05f)
        {
            StopAllCoroutines();
            StartCoroutine(FadeInCurse());

            //Debug.Log("Bad State Activated");

        }

        else if (!activateDamage && safeTimer <= 0.05f)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutCurse());

            //Debug.Log("Good State Activated");

        }

        else
        {
            //Debug.Log("No State change");

            return;
        }
    }

    IEnumerator FadeInCurse()
    {        
        
        filmGrain.intensity.value = 1;
        bloom.intensity.value = 1;

        float firstTimer = 0;
        float firstDuration = 0.5f;

        while (firstTimer < firstDuration)
        {
            float t = firstTimer / firstDuration;
            chromaticAberration.intensity.value = Mathf.Lerp(0f, 0.3f, t);
            currentCA = chromaticAberration.intensity.value;
            vignette.intensity.value = Mathf.Lerp(0f, 0.3f, t);
            currentVignette = vignette.intensity.value;

            firstTimer += Time.deltaTime;
            yield return null;
        }

        float timeElapsed = 0;
        float duration = 21;

        while (timeElapsed < duration)
        {
            while (GameManager.Instance.IsPlayerPaused)
            {
                yield return null;
            }

            float t = timeElapsed / duration;
            chromaticAberration.intensity.value = Mathf.Lerp(0.3f, 1f, t);
            currentCA = chromaticAberration.intensity.value;
            vignette.intensity.value = Mathf.Lerp(0.3f, 1f, t);
            currentVignette = vignette.intensity.value;

            float colorR = vignette.color.value.r;
            float colorG = vignette.color.value.g;
            float colorB = Mathf.Lerp(1f, 0, t);

            vignette.color.value = new Color(colorR, colorG, colorB);
            currentBlue = colorB;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        vignette.intensity.value = 0.9f;

        chromaticAberration.intensity.value = 0.8f;
    }

    IEnumerator FadeOutCurse()
    {

        filmGrain.intensity.value = 0;
        bloom.intensity.value = 0;

        float timeElapsed = 0;
        float duration = 1;

        while (timeElapsed < duration)
        {

            float t = timeElapsed / duration;
            chromaticAberration.intensity.value = Mathf.Lerp(currentCA, 0f, t);
            vignette.intensity.value = Mathf.Lerp(currentVignette, 0f, t);


            float colorR = vignette.color.value.r;
            float colorG = vignette.color.value.g;
            float colorB = Mathf.Lerp(currentBlue, 1f, t);

            vignette.color.value = new Color(colorR, colorG, colorB);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        vignette.intensity.value = 0f;

        chromaticAberration.intensity.value = 0f;
    }

    IEnumerator LoseConditionActivated()
    {
        float timeElapsed = 0;
        float duration = 2;

        GameManager.Instance.IsPlayerPaused = true;

        while (timeElapsed < duration)
        {
 
            float t = timeElapsed / duration;
            chromaticAberration.intensity.value = Mathf.Lerp(currentCA, 1f, t);

            vignette.intensity.value = Mathf.Lerp(currentVignette, 1f, t);
            currentVignette = vignette.intensity.value;

            float colorR = Mathf.Lerp(vignette.color.value.r, 1, t);
            float colorG = Mathf.Lerp(vignette.color.value.g, 0, t);
            float colorB = Mathf.Lerp(vignette.color.value.b, 0, t);

            vignette.color.value = new Color(colorR, colorG, colorB);
            currentBlue = colorB;

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        SceneManager.LoadScene("02_LoseScene");

        StopAllCoroutines();
    }
}
