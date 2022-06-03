using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public Material Custom;

    public bool grabberUnlocked = true;
    public bool rayUnlocked = true;
    public bool pokerUnlocked = true;

    public GameObject grabberL;
    public GameObject rayL;
    public GameObject pokerL;
    public GameObject spikeL;

    public GameObject grabberR;
    public GameObject rayR;
    public GameObject pokerR;
    public GameObject spikeR;

    public GameObject currentLeft;
    public GameObject currentRight;

    // Start is called before the first frame update
    void Start()
    {
        if (grabberL.activeInHierarchy) { currentLeft = grabberL; }
        else if(rayL.activeInHierarchy) { currentLeft = rayL; }
        else if (pokerL.activeInHierarchy) { currentLeft = pokerL; }

        if(grabberR.activeInHierarchy) { currentRight = grabberR; }
        else if (rayR.activeInHierarchy) { currentRight = rayR; }
        else if (pokerR.activeInHierarchy) { currentRight = pokerR; }
    }

    public void colorPink()
    {
        Custom.SetColor("_BaseColor", Color.magenta);
    }

    public void colorBlue()
    {
        Custom.SetColor("_BaseColor", Color.blue);
    }
}
