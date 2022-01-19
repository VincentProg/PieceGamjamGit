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
    float initialTime;
    [SerializeField] float speedAnimation;
    [SerializeField] AnimationCurve curveChrom, curveLens, curveVignette, curveColor, curveCamZoom, curveCamMove;

    Camera cam;
    Vector3 camInitialPosition;
    float camInitialSize;

    [HideInInspector]
    public Transform bed;

    private void Start()
    {
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        postProcessVolume.profile.TryGet<LensDistortion>(out lensDistortion);
        postProcessVolume.profile.TryGet<Vignette>(out vignette);
        postProcessVolume.profile.TryGet<ColorAdjustments>(out colorAdjustements);

        cam = Camera.main;
        camInitialPosition = cam.transform.position;
        camInitialSize = cam.orthographicSize;
    }

    private void Update()
    {

        if (isAnimation)
        {

            float t = (Time.time - initialTime) * speedAnimation;
            AnimationZoomInBed(t);
            if(t > 3)
            {
                isAnimation = false;
            }
        }

    }

    public void ActivateAnimationZoomInBed()
    {
        isAnimation = true;
        initialTime = Time.time;
    }

    private void AnimationZoomInBed(float t)
    {
        // POSTPROCESS
        chromaticAberration.intensity.value = curveChrom.Evaluate(t);
        lensDistortion.intensity.value = curveLens.Evaluate(t);
        vignette.intensity.value = curveVignette.Evaluate(t);
        colorAdjustements.postExposure.value = curveColor.Evaluate(t);

        // CAMERA
        cam.orthographicSize = camInitialSize * curveCamZoom.Evaluate(t);
        cam.transform.position = Vector3.Lerp(camInitialPosition, camInitialPosition + bed.position, curveCamMove.Evaluate(t));
    }


    private void ResetPostProcessValues()
    {
        chromaticAberration.intensity.value = 0;
        lensDistortion.intensity.value = 0;
        vignette.intensity.value = 0;
        colorAdjustements.postExposure.value = 0;
    }
}
