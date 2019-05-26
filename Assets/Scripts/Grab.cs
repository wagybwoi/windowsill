using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public Vector3 startPosition;
    public GameObject[] linePoints = new GameObject[4];

    public Transform curtain1;
    public Transform curtain2;

    public ParticleSystem dirtParticles;
    private Vector3 mousePosition;

    void Start()
    {
        startPosition = linePoints[0].transform.position;

        ParticleSystem.EmissionModule em = dirtParticles.emission;
        em.enabled = false;

        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                mousePosition = hit.point;

                // Debug.DrawLine(ray.origin, hit.point);
                for (int i = 0; i < linePoints.Length; i++)
                {
                    // Disable physics
                    // linePoints[i].GetComponent<Rigidbody>().isKinematic = true;

                    if (linePoints[i].GetComponent<ConfigurableJoint>())
                    {
                        linePoints[i].transform.eulerAngles = new Vector3(0f, 0f, 0f);

                        // Disable locked joint position
                        linePoints[i].GetComponent<ConfigurableJoint>().autoConfigureConnectedAnchor = false;

                        // Set locked joint position
                        linePoints[i].GetComponent<ConfigurableJoint>().connectedAnchor = Vector3.Lerp(linePoints[i].GetComponent<ConfigurableJoint>().connectedAnchor, new Vector3(0f, (hit.point.y - startPosition.y) / (linePoints.Length - 1), linePoints[i].GetComponent<ConfigurableJoint>().connectedAnchor.z), 1f * Time.deltaTime);

                        // Enable locked joint position
                        linePoints[i].GetComponent<ConfigurableJoint>().autoConfigureConnectedAnchor = false;

                        ParticleSystem.EmissionModule em = dirtParticles.emission;
                        em.enabled = true;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Nudge plant stem
            linePoints[2].GetComponent<Rigidbody>().AddForce(new Vector3(mousePosition.x < linePoints[2].transform.position.x ? -1 : 1, 0f, 0f), ForceMode.Impulse);
            linePoints[3].GetComponent<Rigidbody>().AddForce(new Vector3(mousePosition.x < linePoints[3].transform.position.x ? -2 : 2, 0f, 0f), ForceMode.Impulse);

            // Turn off particle system
            ParticleSystem.EmissionModule em = dirtParticles.emission;
            em.enabled = false;
        }
    }
}
