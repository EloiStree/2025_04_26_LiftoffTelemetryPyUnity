using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class LiftoffMono_LineRenderingPath : MonoBehaviour
{

    public Transform m_targetToFollow;
    public Transform m_targetToGenerateRadius;
    public LineRenderer m_lineRenderer;
    public int m_pointsCount = 60;
    public float m_timeBetweenPointsSave = 0.5f;

    private Vector3 [] m_points = new Vector3[0];
    public Color m_colorToApply = Color.yellow;
        

    public void Awake()
    {
        m_lineRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Unlit")) { color = m_colorToApply };

    }
    public void Reset()
    {
        m_targetToFollow = GetComponent<Transform>();
        m_lineRenderer = GetComponent<LineRenderer>();
        if (m_lineRenderer != null)
        {
            m_lineRenderer.positionCount = m_pointsCount;
            m_points = new Vector3[m_pointsCount];
            m_lineRenderer.SetPositions(m_points);
        }
    }
    private void OnEnable()
    {
        StartCoroutine(Coroutine_SavePointsCoroutine());
    }

    private IEnumerator Coroutine_SavePointsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_timeBetweenPointsSave);
            SavePoints();
        }
    }
    public void LateUpdate()
    {
        if (m_targetToFollow == null || m_lineRenderer == null) return;

        if (m_targetToGenerateRadius != null) { 
        
            float radius = Vector3.Distance(m_targetToFollow.position, m_targetToGenerateRadius.position)*0.5f;
            m_lineRenderer.startWidth = radius;
            m_lineRenderer.endWidth = radius;

        }

        // Ensure the LineRenderer has enough positions
        if (m_lineRenderer.positionCount < m_pointsCount)
        {
            m_lineRenderer.positionCount = m_pointsCount;
        }
        // Update the last position of the LineRenderer to follow the target
        Vector3 newPoint = m_targetToFollow.position;
        if (m_points.Length > 0) { 
            m_points[0] = newPoint;
        }
        m_lineRenderer.SetPositions(m_points);
       

    }

    private void SavePoints()
    {
        if (m_lineRenderer == null) return;
        if (m_points.Length != m_pointsCount)
        {
            m_points = new Vector3[m_pointsCount];
            m_lineRenderer.SetPositions(m_points);
        }
        Vector3 newPoint = m_targetToFollow.position;

        for (int i = m_pointsCount - 1; i > 1; i--)
        {
            m_points[i] = m_points[i - 1]; // Shift points to the right
        }
        if (m_points.Length > 1)
            m_points[1] = newPoint;

        m_lineRenderer.SetPositions(m_points);
    }

}
