using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAccelerator : MonoBehaviour
{
    bool isAccelerate;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            if (!isAccelerate) Time.timeScale = 10;
            else Time.timeScale = 1;

            isAccelerate = !isAccelerate;
        }
    }
}
