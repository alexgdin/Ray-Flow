using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100;
    
    public Transform m_lightSource;
    public LineRenderer m_lineRenderer;
    
    Transform m_transform;



    private void Awake()
    {
        m_transform = GetComponent<Transform>();
    }

    void Update()
    {
        ThrowRay();
    }

    void ThrowRay()
    {
        if (Physics2D.Raycast(m_transform.position, m_transform.right, defDistanceRay))
        {
            RaycastHit2D hit = Physics2D.Raycast(m_transform.position, m_transform.right, defDistanceRay);
            Draw2DRay(m_transform.position, hit.point);
        }
        else
        {
            Draw2DRay(m_lightSource.position, m_lightSource.position + m_lightSource.right * defDistanceRay);
        }
    }

    void Draw2DRay(Vector2 start, Vector2 end)
    {
        m_lineRenderer.SetPosition(0, start);
        m_lineRenderer.SetPosition(1, end);
    }
}
