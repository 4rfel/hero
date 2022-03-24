using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LaserGun : MonoBehaviour {

    [SerializeField] private float range = 50.0f;

    private ParticleSystem laserParticles;
    private bool propulsion = false;
    [SerializeField] private SteamVR_Action_Boolean botao = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("InteractUI");
    [SerializeField] private SteamVR_Action_Boolean grip = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    SteamVR_Behaviour_Pose trackedObj;

    void Start() {
        laserParticles = GetComponent<ParticleSystem>();
        trackedObj = GetComponent<SteamVR_Behaviour_Pose>();
    }

    void Raycast() {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, range)) {
            Destructable destructable = hit.collider.gameObject.GetComponent<Destructable>();
            if (destructable != null) {
                destructable.DoDamage(transform.position, propulsion);
            }
        }
    }

    void FixedUpdate() {

        if (botao.GetState(trackedObj.inputSource)) {
            laserParticles.startColor = propulsion ? Color.red : Color.blue;
            laserParticles.Play();
            Raycast();
        } else {
            laserParticles.Clear();
            laserParticles.Pause();
        }

        if (grip.GetStateDown(trackedObj.inputSource)) {
            propulsion = !propulsion;
        }
    }

    // Draw the ray using gizmos
    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * range);
    }
}
