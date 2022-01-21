using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComaScript : MonoBehaviour
{
    [SerializeField] float durationPress;
    [SerializeField] float speedUnload;
    bool isPressingButton;
    float initialTime;

    [SerializeField] Image loadImage;
    PostProcessManager postProcessManager;

    GhostScript ghostScript;
    private void Start()
    {
 
        postProcessManager = FindObjectOfType<PostProcessManager>();
        ghostScript = GetComponent<GhostScript>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            initialTime = Time.time;
            isPressingButton = true;
        } else if (Input.GetMouseButtonUp(0))
        {
            initialTime = Time.time;
            isPressingButton = false;
        }

        if (isPressingButton) Load(true);
        else if (loadImage.fillAmount > 0) Load(false);

    }



    void Load(bool isLoading)
    {


        if (isLoading)
        {
            float t = Time.time - initialTime;
            loadImage.fillAmount = t / durationPress;
            if (t >= durationPress) ExitComa();
        }
        else loadImage.fillAmount -= speedUnload * Time.deltaTime;
        

        
    }

    void ExitComa()
    {

        postProcessManager.ActivateTransition();
        
        ghostScript.enabled = true;
        ghostScript.EndInteraction();
        enabled = false;
    }

}
