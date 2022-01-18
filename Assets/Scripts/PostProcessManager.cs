using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PostProcessManager : MonoBehaviour
{

    Volume postProcessVolume;
    ChromaticAberration chromaticAberration;
    LensDistortion lensDistortion;
    Vignette vignette;
    ColorAdjustments colorAdjustements;

    bool isAnimation;
    [SerializeField] AnimationCurve curveChrom, curveLens, curveVignette, curveColor, curveCamZoom, curveCamMove;

    [SerializeField] float speedAnimation;
    float initialTime;

    [SerializeField] Transform bed;
    Camera cam;
    Vector3 camInitialPosition;
    float initCamSize;

    private void Start()
    {
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        postProcessVolume.profile.TryGet<LensDistortion>(out lensDistortion);
        postProcessVolume.profile.TryGet<Vignette>(out vignette);
        postProcessVolume.profile.TryGet<ColorAdjustments>(out colorAdjustements);

        cam = Camera.main;
        camInitialPosition = cam.transform.position;
        initCamSize = cam.orthographicSize;
        StartCoroutine(Test());
    }

    private void Update()
    {

        if (isAnimation)
        {

            float t = (Time.time - initialTime) * speedAnimation;
            SetPostProcessValues(t);


            SetPostProcessValues(t);
            ZoomCamera(t);
            LookAtCamera(t);

            if(t > 3)
            {
                isAnimation = false;
            }
        }

    }

    private void ActivateAnimation()
    {
        isAnimation = true;
        initialTime = Time.time;
    }

    private void SetPostProcessValues(float t)
    {
        chromaticAberration.intensity.value = curveChrom.Evaluate(t);
        lensDistortion.intensity.value = curveLens.Evaluate(t);
        vignette.intensity.value = curveVignette.Evaluate(t);
        colorAdjustements.postExposure.value = curveColor.Evaluate(t);
    }

    private void ZoomCamera(float t)
    {
        cam.orthographicSize = initCamSize * curveCamZoom.Evaluate(t);
    }

    private void LookAtCamera(float t)
    {
        cam.transform.position = Vector3.Lerp(camInitialPosition, camInitialPosition + bed.position, curveCamMove.Evaluate(t));
    }

    private void ResetPostProcessValues()
    {
        chromaticAberration.intensity.value = 0;
        lensDistortion.intensity.value = 0;
        vignette.intensity.value = 0;
        colorAdjustements.postExposure.value = 0;
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(1);
        ActivateAnimation();
    }
}
