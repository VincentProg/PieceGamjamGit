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

    [SerializeField]
    Camera cam1, cam2;

    Vector3 camInitialPosition;
    float camInitialSize;

    [HideInInspector]
    public Transform bed;

    // COMA
    bool isComa = true;

    private void Start()
    {
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        postProcessVolume.profile.TryGet<LensDistortion>(out lensDistortion);
        postProcessVolume.profile.TryGet<Vignette>(out vignette);
        postProcessVolume.profile.TryGet<ColorAdjustments>(out colorAdjustements);

        camInitialPosition = cam1.transform.position;
        camInitialSize = cam1.orthographicSize;
    }

    private void Update()
    {

        if (isAnimation)
        {
            
            float t = (Time.time - initialTime) * speedAnimation;
            if (isComa)
            {
                AnimationZoomInBed(t);
                if (t > 1)
                {
                    SwapCamera();
                    isAnimation = false;
                }
            } else
            {
                t = 1 - t;
                AnimationZoomInBed(t);
                
            }
        }

    }

    public void ActivateTransition()
    {
        isAnimation = true;
        isComa = !isComa;
        if (!isComa) {
            SwapCamera();
        }
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
        cam1.orthographicSize = camInitialSize * curveCamZoom.Evaluate(t);
        cam1.transform.position = Vector3.Lerp(camInitialPosition, camInitialPosition + bed.position, curveCamMove.Evaluate(t));
    }


    private void ResetPostProcessValues()
    {
        chromaticAberration.intensity.value = 0;
        lensDistortion.intensity.value = 0;
        vignette.intensity.value = 0;
        colorAdjustements.postExposure.value = 0;
    }

    private void SwapCamera()
    {
        cam1.enabled = !cam1.enabled;
        cam2.enabled = !cam2.enabled;
    }
}
