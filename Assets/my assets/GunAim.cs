using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAim : MonoBehaviour
{
    public Camera cam;
    public GameObject lookPositionEmpty;
    public float depth = 10f;
    public bool debug = false;

    void FixedUpdate()
    {
        // CameraDepthAim();
        RaycastAim();
    }

    void CameraDepthAim() {
        Vector3 cameraForwardPoint = cam.transform.position + (cam.transform.forward * depth);

        lookPositionEmpty.transform.position = cameraForwardPoint;
        transform.LookAt(lookPositionEmpty.transform);

        if (debug) {
            Debug.DrawRay(transform.position, Vector3.Scale(transform.forward, new Vector3(depth, depth, depth)), Color.red);
            Debug.DrawRay(cam.transform.position, cam.transform.forward * depth, Color.blue);
        }
    }

    void RaycastAim() {
        RaycastHit hit;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0), Camera.MonoOrStereoscopicEye.Mono);

        if (Physics.Raycast(ray, out hit)) {
            Vector3 objectHit = hit.point;

            Vector3 cameraForwardPoint = cam.transform.position + (cam.transform.forward * depth);

            lookPositionEmpty.transform.position = objectHit;
            transform.LookAt(lookPositionEmpty.transform);

            if (debug) {
                Debug.DrawRay(transform.position, Vector3.Scale(transform.forward, new Vector3(depth, depth, depth)), Color.red);
                Debug.DrawRay(cam.transform.position, cam.transform.forward * depth, Color.blue);
            }
        } else {
            CameraDepthAim();
        }

    }
}
