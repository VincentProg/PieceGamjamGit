using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShader : MonoBehaviour
{

     Shader shader;
    Material initialMaterial;

    Renderer renderer;


    bool isActivated = true;
    bool isHighLight;
    float initialTime;
    [SerializeField] float delayHighlight = 3;
    [SerializeField] float speedHighlight = 2;
    public float sens;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();       
        initialMaterial = renderer.material;

        shader = Shader.Find("Shader Graphs/ShaderInteractive");
        renderer.material = CreateShaderFromMaterial(initialMaterial, transform.name);
        StartCoroutine(SetDelayHighlight());
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            if (isHighLight)
            {
                float t = (Time.time - initialTime) * speedHighlight + 2;
                renderer.material.SetFloat("_currentTime", t);
                if (t > 4.5f)
                {
                    isHighLight = false;
                    StartCoroutine(SetDelayHighlight());
                }

            }
        }

    }

    private void ActivateHighlight()
    {
        isHighLight = true;
        renderer.material.SetFloat("_currentTime", 2);
        initialTime = Time.time;
    }
    public void ActivateShader()
    {
        isActivated = true;
        isHighLight = true;
        initialTime = Time.time;
        renderer.material.SetFloat("_currentTime", 2);
        SetDelayHighlight();
    }

    public void DeactivateShader()
    {
        isActivated = false;
        isHighLight = false;
        renderer = GetComponent<Renderer>();
        renderer.material.SetFloat("_currentTime", 2);
        StopAllCoroutines();
    }

    Material CreateShaderFromMaterial(Material material, string name)
    {
        Material mat = new Material(shader);
        mat.name = "Shader Disappear - " + name;

        Texture texture;
        if (texture = material.GetTexture("_BaseMap"))
            mat.SetTexture("_BaseMap", texture);
        //if (texture = material.GetTexture("_MetallicGlossMap"))
        //    mat.SetTexture("_MetallicMap", texture);
        if (texture = material.GetTexture("_BumpMap"))
            mat.SetTexture("_NormalMap", texture);
        //if (texture = material.GetTexture("_DetailMask"))
        //    mat.SetTexture("_MaskMap", texture);
        //if (texture = material.GetTexture("_OcclusionMap"))
        //    mat.SetTexture("_OcclusionMap", texture);
        //if (texture = material.GetTexture("_EmissionMap"))
        //    mat.SetTexture("_EmissionMap", texture);

        mat.SetColor("_Color", material.GetColor("_BaseColor"));
        mat.SetFloat("_currentTime", 2);
        mat.SetFloat("_Sens", sens);
        //mat.SetColor("_EmissionColor", material.GetColor("_EmissionColor"));
        //if (!isAppear) mat.SetColor("_ColorEdge", colorEdgeDisappear);
        //if (material.IsKeywordEnabled("_EMISSION")) mat.SetFloat("_Intensity", 1);
        //mat.SetFloat("_EdgeWidth", edgeWidth);
        //mat.SetFloat("_SpeedRotation", speedRotation);
        //mat.SetInt("_NoiseSize", noiseSize);
        //mat.SetFloat("_Fade", 1);
        return mat;
    }

    IEnumerator SetDelayHighlight()
    {
        yield return new WaitForSeconds(delayHighlight);
        print("yo");
        ActivateHighlight();
    }
}
