using System.Collections.Generic;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 25f;
    [SerializeField] private int maxBounces = 8;
    [SerializeField] private float surfaceOffset = 0.001f; // avoids the next ray re-hitting the same collider

    public Transform m_lightSource;
    public LineRenderer m_lineRenderer;

    Transform m_transform;

    private void Awake()
    {
        m_transform = transform; // no need for GetComponent<Transform>()
    }

    void Update()
    {
        ThrowRay();
    }

    void ThrowRay()
    {
        List<Vector2> points = new List<Vector2> { m_transform.position };

        Vector2 origin = m_transform.position;
        Vector2 direction = m_transform.right;

        for (int bounce = 0; bounce < maxBounces; bounce++)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, defDistanceRay);

            if (!hit.collider)
            {
                points.Add(origin + direction * defDistanceRay);
                break;
            }

            if (hit.collider.CompareTag("mirror"))
            {
                points.Add(hit.point);
                direction = Vector2.Reflect(direction, hit.normal).normalized;
                origin = hit.point + direction * surfaceOffset;
                continue; // cast the next segment from the reflection point
            }

            if (hit.collider.CompareTag("prism"))
            {
                points.Add(hit.point);
                Disperse(hit.point, direction, hit.transform);
                break;
            }

            // any other surface just absorbs the ray
            points.Add(hit.point);
            break;
        }

        Draw2DRay(points);
    }

    void Draw2DRay(List<Vector2> points)
    {
        m_lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            m_lineRenderer.SetPosition(i, points[i]);
        }
    }

    void Disperse(Vector2 origin, Vector2 direction, Transform hitTransform)
    {
        // TODO: split into multiple sub-rays at different angles/colors (e.g. a simple RGB spread).
        // Left as a stub — the original didn't implement actual dispersion either.
    }
}