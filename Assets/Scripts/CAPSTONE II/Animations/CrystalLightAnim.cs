using UnityEngine;

public class CrystalLightAnim : MonoBehaviour
{
    public Light crystalLight;
    public float randomFactor;
    public float maxIntensity;

    public bool inverted;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(crystalLight == null) crystalLight = GetComponent<Light>();

        RandomizeTime();

        inverted = false;
    }

    // Update is called once per frame
    void Update()
    {
        crystalLight.intensity += randomFactor * Time.deltaTime;

        CheckLight();        
    }

    public void CheckLight()
    {
        if (crystalLight.intensity > maxIntensity && !inverted)
        {
            randomFactor *= -1;
            inverted = true;
        }

        else if (crystalLight.intensity == 0)
        {
            //randomFactor *= -1;

            RandomizeTime();
            inverted = false;
        }

        else
        {
            return;
        }
    }


    public void RandomizeTime()
    {
        randomFactor = Random.Range(3f, 8f);

        if (randomFactor > 5f)
        {
            maxIntensity = Random.Range(8f, 18f);
        }
        else
        {
            maxIntensity = Random.Range(6f, 12f);
        }
    }
}
