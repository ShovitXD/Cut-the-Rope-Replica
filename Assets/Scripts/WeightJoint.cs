using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightJoint : MonoBehaviour
{
    public float DistanceFromChain = 1.5f;
    public void ConnectedRopeEnd (Rigidbody2D endRB)
    {
       HingeJoint2D joint = gameObject.AddComponent<HingeJoint2D>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = endRB;
        joint.anchor = Vector2.zero;
        joint.connectedAnchor = new Vector2 (0f, - DistanceFromChain);

    }
}
