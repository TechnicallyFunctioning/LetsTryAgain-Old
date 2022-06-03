using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponPicker : MonoBehaviour
{
    [SerializeField] private InputActionReference button;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject weaponWheelR;
    [SerializeField] private Image grabberPoint;
    [SerializeField] private Image rayPoint;
    [SerializeField] private Image pokerPoint;
    
    private GlobalVariables global;
    private float rotation;

    private bool grabberSelected = false;
    private bool raySelected = false;
    private bool pokerSelected = false;

    private void Start()
    {
        global = GameObject.Find("Global Variables").GetComponent<GlobalVariables>();
        button.action.performed += WeaponWheel;
        button.action.canceled += CloseWeaponWheel;

    }

    private void Update()
    {
        rotation = rightHand.transform.localEulerAngles.z;
        if (rotation > 340 || rotation < 30)
        { grabberPoint.color = Color.green; grabberSelected = true; } else { grabberPoint.color = Color.white; grabberSelected = false; }

        if(rotation > 30 && rotation < 120)
        { pokerPoint.color = Color.green; pokerSelected = true; } else { pokerPoint.color = Color.white; pokerSelected = false; }

        if(rotation > 180 && rotation < 340)
        { rayPoint.color = Color.green; raySelected = true; } else { rayPoint.color = Color.white; raySelected = false; }
    }

    //OnButtonPress
    private void WeaponWheel(InputAction.CallbackContext context)
    {
        weaponWheelR.SetActive(true);
    }

    //OnButtonRelease
    private void CloseWeaponWheel(InputAction.CallbackContext context)
    {
        global.currentRight.SetActive(false);
        if (grabberSelected) { global.grabberR.SetActive(true); global.currentRight = global.grabberR; }
        else if(pokerSelected) { global.pokerR.SetActive(true); global.spikeR.SetActive(true); global.currentRight = global.pokerR; }
        else if (raySelected) { global.rayR.SetActive(true); global.currentRight = global.rayR; }
        else { global.grabberR.SetActive(true); global.currentRight = global.grabberR; }

        weaponWheelR.SetActive(false);
    }
}
