using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookRope : MonoBehaviour
{
    [Header("References")]
    public GameObject linkPrefab;
    public Rigidbody2D hook;
    public WeightJoint Weight;
    [Header("Values")]
    [SerializeField] int NoOfLinks = 7;

    void Start()
    {
        GenerateRope();
    }

    void GenerateRope()
    {
        Rigidbody2D previusRB = hook;
        for (int i = 0; i < NoOfLinks; i++)
        {
            GameObject link = Instantiate(linkPrefab, this.transform);
            HingeJoint2D joint = link.GetComponent<HingeJoint2D>();
            joint.connectedBody = previusRB;

            if(i< NoOfLinks - 1)
            {
                previusRB = link.GetComponent<Rigidbody2D>();
            }
            else
            {
                Weight.ConnectedRopeEnd(link.GetComponent<Rigidbody2D>());
            }

            previusRB = link.GetComponent<Rigidbody2D>();
        }
    }
}
