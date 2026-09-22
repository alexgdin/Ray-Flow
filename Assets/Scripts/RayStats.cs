using System.Collections.Generic;
using UnityEngine;

// One mirror bounce's worth of physics data.
public struct BounceReading
{
    public Vector2 point;
    public float incidenceAngle;  // degrees, measured from the surface normal
    public float reflectionAngle; // degrees, measured from the surface normal
}

// Everything measured during a single ThrowRay() pass.
public class RayStats
{
    public List<BounceReading> bounces = new List<BounceReading>();
    public float totalPathLength;
    public bool hitTarget;
}