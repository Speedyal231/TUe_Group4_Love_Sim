using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullObj : MonoBehaviour
{
    [SerializeField] float cullDistance;
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject gameObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cullDistance < DistanceCalc())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private float DistanceCalc()
    {
        return (playerTransform.position - transform.position).magnitude;
    }
}
