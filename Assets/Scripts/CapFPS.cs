using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapFPS : MonoBehaviour
{
    public int fps = 60;
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = fps;
    }
}
