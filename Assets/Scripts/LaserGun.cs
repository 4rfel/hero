using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour {

    [SerializeField] private Transform attachmentPoint;
    [SerializeField] private float range = 50.0f;

    private ParticleSystem laserParticles;
    private bool isLaserOn = false;

    void Start() {
        laserParticles = GetComponent<ParticleSystem>();
    }

    void Raycast() {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, range)) {
            Destructable destructable = hit.collider.gameObject.GetComponent<Destructable>();
            if (destructable != null) {
                destructable.DoDamage();
            }
        }
    }

    void MoveGun() {
        // Test if the player is grounded
        float mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(Vector3.right * -mouseY);
    }

    void Update() {
        MoveGun();

        if (Input.GetMouseButtonDown(1)) {
            if (isLaserOn) {
                laserParticles.Clear();
                laserParticles.Pause();
                isLaserOn = false;
            } else {
                laserParticles.Play();
                isLaserOn = true;
            }
            Raycast();
        }
    }

    // Draw the ray using gizmos
    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * range);
    }
}
