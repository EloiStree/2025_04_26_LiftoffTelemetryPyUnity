namespace Eloi.LiftoffWrapper
{

    using UnityEngine;
    using System.IO;
    using System.Text;

    public class LiftoffMono_LineToMeshExporter : MonoBehaviour
    {
        public LineRenderer m_target;
        public string m_nameWhenExported = "LineMesh";
        [ContextMenu("Export LineRenderer Mesh")]
        void ExportLineRendererMesh()
        {
            if (!m_target) return;

            Mesh mesh = new Mesh();
            m_target.BakeMesh(mesh, true);

            string obj = MeshToObj(mesh);
            string path = Path.Combine(Application.dataPath, "../"+ m_nameWhenExported + ".obj"); // outside Assets folder
            File.WriteAllText(path, obj);
            Debug.Log("Exported mesh to: " + path);
        }

        string MeshToObj(Mesh mesh)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Vector3 v in mesh.vertices)
                sb.AppendLine($"v {v.x} {v.y} {v.z}");

            foreach (Vector3 n in mesh.normals)
                sb.AppendLine($"vn {n.x} {n.y} {n.z}");

            foreach (Vector2 uv in mesh.uv)
                sb.AppendLine($"vt {uv.x} {uv.y}");

            int[] triangles = mesh.triangles;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                // OBJ uses 1-based indices
                int i1 = triangles[i + 0] + 1;
                int i2 = triangles[i + 1] + 1;
                int i3 = triangles[i + 2] + 1;
                sb.AppendLine($"f {i1}/{i1}/{i1} {i2}/{i2}/{i2} {i3}/{i3}/{i3}");
            }

            return sb.ToString();
        }
    }
}
