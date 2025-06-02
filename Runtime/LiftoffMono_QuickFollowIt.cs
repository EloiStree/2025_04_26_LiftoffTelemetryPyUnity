using UnityEngine;

public class LiftoffMono_QuickFollowIt : MonoBehaviour
{

    public Transform m_whatToMove;
    public Transform m_whatToFollow;

    public bool m_usingLerp = true;
    public float m_moveLerpMultiplicator = 2f;
    public float m_rotationLerpMultiplicator = 3f;
    void LateUpdate()
    {

        if (m_usingLerp) {

            m_whatToMove.position = Vector3.Lerp(m_whatToMove.position, m_whatToFollow.position, m_moveLerpMultiplicator);
            m_whatToMove.rotation = Quaternion.Lerp(m_whatToMove.rotation, m_whatToFollow.rotation, m_rotationLerpMultiplicator);
        }
        else
        {
            m_whatToMove.position = m_whatToFollow.position;
            m_whatToMove.rotation = m_whatToFollow.rotation;
        }
        
    }
}
