using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }

    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private float ropeSegLen = 0.25f;
    public int segmentLength = 35;
    public float lineWidth = 0.1f;

    void Start()
    {
        this.lineRenderer = GetComponent<LineRenderer>();

        // Rope initial pos = Mouse Pos
        Vector3 ropeStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < segmentLength; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }
    }

    void Update()
    {
        DrawRope();
    }
    void FixedUpdate()
    {
        Simulate();
    }

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePosition = new Vector3[this.segmentLength];
        for (int i = 0;i < this.segmentLength; i++)
        {
            ropePosition[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePosition.Length;
        lineRenderer.SetPositions(ropePosition);
    }

    private void Simulate()
    {
        //Simulation

        Vector2 forceGravity = new Vector2(0f,-1f);

        for (int i = 0; i < this.segmentLength; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.deltaTime;
            this.ropeSegments[i] = firstSegment;
        }    

        //Constraints
        for (int i = 0; i< 50; i++)
        {
            ApplyConstraint();
        }

    }

    private void ApplyConstraint()
    {
        RopeSegment firstSegment = this.ropeSegments[0];
        //Rope 1st point Location
        firstSegment.posNow = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.ropeSegments[0] = firstSegment;

        for(int i = 1; i < this.segmentLength - 1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if(dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }
            Vector2 changeAmount = changeDir * error;
            if(i!= 0)
            {
                firstSeg.posNow -= changeAmount * error;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg; 
            }
        }
    }
   
}
