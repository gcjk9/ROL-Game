using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class WaterEffect : MonoBehaviour
{
    public PostProcessingProfile underWater;
    public PostProcessingProfile normal;
    // Start is called before the first frame update
    void Start()
    {
        GameObject c= GameObject.FindWithTag("MainCamera");
        if (c != null)
        {
            PostProcessingProfile p = c.GetComponent<PostProcessingBehaviour>().profile;
            p.vignette.enabled = false;
            p.chromaticAberration.enabled = false;
            p.bloom.enabled = false;
            p.colorGrading = normal.colorGrading;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            PostProcessingProfile p = other.GetComponent<PostProcessingBehaviour>().profile;
            p.vignette.enabled = true;
            p.chromaticAberration.enabled = true;
            p.bloom.enabled = true;
            p.vignette = underWater.vignette;
            p.chromaticAberration = underWater.chromaticAberration;
            p.colorGrading = underWater.colorGrading;
            p.bloom = underWater.bloom;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            PostProcessingProfile p = other.GetComponent<PostProcessingBehaviour>().profile;
            p.vignette.enabled = false;
            p.chromaticAberration.enabled = false;
            p.bloom.enabled = false;
            p.colorGrading = normal.colorGrading;
        }
    }
}
