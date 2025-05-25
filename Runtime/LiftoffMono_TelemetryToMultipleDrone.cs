using UnityEngine;


namespace Eloi.LiftoffWrapper {
    public class LiftoffMono_TelemetryToMultipleDrone : MonoBehaviour {

        public IndexToDrone[] m_indexToDroneTransform = new IndexToDrone[0];
        public STRUCT_Telemetry m_lastTelemetry;
        public void PushIn(STRUCT_Telemetry telemetry)
        {
            m_lastTelemetry = telemetry;
            telemetry.GetPosition(out Vector3 position);
            telemetry.GetRotation(out Quaternion rotation);
            telemetry.GetPlayerIndex(out int index);
            for (int i = 0; i < m_indexToDroneTransform.Length; i++)
            {
                if (m_indexToDroneTransform[i].m_index == index)
                {
                    m_indexToDroneTransform[i].m_whatToMove.localPosition = position;
                    m_indexToDroneTransform[i].m_whatToMove.localRotation = rotation;
                    m_indexToDroneTransform[i].m_telemetry = telemetry;
                }
            }
        }
        [System.Serializable]
        public class IndexToDrone
        {
            public int m_index;
            public Transform m_whatToMove;
            public STRUCT_Telemetry m_telemetry;
        }

    }
}
