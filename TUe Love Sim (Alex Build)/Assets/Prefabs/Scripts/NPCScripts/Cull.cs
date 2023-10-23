using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cull : MonoBehaviour
{
    [SerializeField] float cullDistance;
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject gameObject;
    [SerializeField] GameObject gameObject2;
    [SerializeField] NPCData nPC;

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
            if (nPC.FetchDead()) 
            { 
                gameObject2.SetActive(false);
            }
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
