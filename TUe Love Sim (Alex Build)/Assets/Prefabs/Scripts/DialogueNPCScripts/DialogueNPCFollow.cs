using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPCFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    // Update is called once per frame
    void Update()
    {
        transform.forward = DirectionCalc();
    }

    private Vector3 DirectionCalc()
    {
        return Vector3.ProjectOnPlane((playerTransform.position - transform.position).normalized, transform.up);
    }
}
