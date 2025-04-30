using UnityEngine;


namespace Eloi.LiftoffWrapper {
    public class LiftoffMono_TelemetryToSoloDrone : MonoBehaviour
    {
        public Transform m_whatToMove;

        public void PushIn(STRUCT_Telemetry telemetry)
        {
            telemetry.GetPosition(out Vector3 position);
            telemetry.GetRotation(out Quaternion rotation);
            m_whatToMove.localPosition = position;
            m_whatToMove.localRotation = rotation;
        }
    }
}
