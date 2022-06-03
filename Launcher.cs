using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Launcher : MonoBehaviour
{
    [SerializeField] InputActionReference activate;
    [SerializeField] float force = 200;
    private Fruit fruitScript;
    private float finalForce;
    private XRDirectInteractor interactor;
    private GameObject heldItem;
    private XRGrabInteractable itemGrab;
    private Rigidbody itemRb;

    void Start()
    {
        activate.action.performed += Activate;
        activate.action.canceled += Cancel;

        interactor = GetComponent<XRDirectInteractor>();
    }

    public void GetHeldObject()
    {
        heldItem = interactor.selectTarget.gameObject;
        itemGrab = heldItem.GetComponent<XRGrabInteractable>();
        itemRb = heldItem.GetComponent<Rigidbody>();
        fruitScript = heldItem.GetComponent<Fruit>();
    }

   private void Activate(InputAction.CallbackContext context)
    {
        finalForce = force;
    }

    private void Cancel(InputAction.CallbackContext context)
    {
        finalForce = 0;
    }

    public void Launch()
    {
        itemRb.AddForce(transform.forward * finalForce, ForceMode.Impulse);
        if(fruitScript != null) { fruitScript.Launched(); }
    }
}
