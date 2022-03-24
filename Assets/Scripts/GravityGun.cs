using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GravityGun : MonoBehaviour {

    [SerializeField] private Transform attachmentPoint;
    [SerializeField] private float throwForce = 10.0f;
    [SerializeField] private float distanceToGrab = 10.0f;
    [SerializeField] private float cooldownToGrab = 3.0f;
    [SerializeField] private SteamVR_Action_Boolean botao = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("InteractUI");


    SteamVR_Behaviour_Pose trackedObj;

    private float currentCooldown = 0.0f;
    private bool hasObject = false;

    private GameObject grabbedObject;

    void Start() {
        trackedObj = GetComponent<SteamVR_Behaviour_Pose>();
    }

    void Raycast() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanceToGrab)) {
            if (hit.collider.gameObject.tag == "Pickable") {

                grabbedObject = hit.collider.gameObject;
                // Move the object to the attachment point
                // grabbedObject.transform.position = attachmentPoint.position;
                hasObject = true;
                // Set the object as the child of the attachment point
                grabbedObject.transform.parent = attachmentPoint;
                // Disable the collider and gravity
                // hit.collider.enabled = false;
                grabbedObject.GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }

    void LerpObject() {
        if (hasObject) {
            grabbedObject.transform.position = Vector3.Lerp(grabbedObject.transform.position, attachmentPoint.position, Time.deltaTime * 10.0f);
        }
    }

    void ThrowObject() {
        if (hasObject) {
            // Get the object's rigidbody
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            // Add force to the object
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            // Remove the object from the attachment point
            grabbedObject.transform.parent = null;
            // Enable the collider and gravity
            rb.gameObject.GetComponent<Collider>().enabled = true;
            rb.gameObject.GetComponent<Rigidbody>().useGravity = true;
            // Reset the grabbed object
            grabbedObject = null;
            // Reset the hasObject variable
            hasObject = false;
            // Reset the cooldown
            currentCooldown = cooldownToGrab;
        }
    }

    void Update() {

        if (botao.GetState(trackedObj.inputSource)) {
            if (currentCooldown <= 0.0f && !hasObject) { Raycast(); }
        }

        if (hasObject && botao.GetStateUp(trackedObj.inputSource)) {
            ThrowObject();
        }

        if (currentCooldown > 0.0f) { currentCooldown -= Time.deltaTime; }
    }

    void FixedUpdate() {
        LerpObject();
    }

    // Draw the ray using gizmos
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * distanceToGrab);
    }
}
