using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;

public class LiftoffMono_QuickRotatingBlade : MonoBehaviour
{

    public bool m_isActive = true; 
    public Vector3 m_rotationAxis = Vector3.up;
    public bool m_inverseBlade = false;
    public float m_rotationSpeed = 360f; 
    public float m_multiplicator = 1f; 
    public Space m_space = Space.Self;

    void Update()
    {
        if (m_isActive)
        {
            float rotationAngle = m_rotationSpeed * Time.deltaTime * m_multiplicator;
            if (m_inverseBlade)
            {
                rotationAngle = -rotationAngle;
            }
            transform.Rotate(m_rotationAxis, rotationAngle, m_space);
        }

    }
}
