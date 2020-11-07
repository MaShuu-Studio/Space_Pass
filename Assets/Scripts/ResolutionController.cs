using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionController : MonoBehaviour
{
    float ratio;
    int width;
    int height;

    // Start is called before the first frame update
    void Start()
    {
        width = 1600;
        height = 900;
        Screen.SetResolution(width, height, false);
        
        ratio = (float) width / (float) height;
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.fullScreen == false)
        {
            int curWidth = Screen.width;
            int curHeight = Screen.height;
            
            if (width != curWidth || height != curHeight)
            {
                float curRatio = (float)curWidth / (float)curHeight;
                if (curRatio > ratio)
                {
                    curHeight = (int)(curWidth / ratio);
                }
                else if(curRatio < ratio)
                {
                    curWidth = (int)(curHeight * ratio);
                }
                Screen.SetResolution(curWidth, curHeight, false);
                width = curWidth;
                height = curHeight;
            }
        }                
    }
}
