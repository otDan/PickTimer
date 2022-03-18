using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace PlayerMarkers.Util
{
    public static class ObjectManager
    {
        private static Mesh CreateMesh()
        {
            Vector3[] vertices = {
             new Vector3(-0.5f, -0.5f, 0),
             new Vector3(0.5f, -0.5f, 0),
             new Vector3(0f, 0.5f, 0)
         };

            Vector2[] uv = {
             new Vector2(0, 0),
             new Vector2(1, 0),
             new Vector2(0.5f, 1)
         };

            int[] triangles = { 0, 1, 2 };

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            return mesh;
        }

        public static GameObject CreateObject(string name, Color color)
        {
            var mesh = CreateMesh();

            var obj = new GameObject(name);

            var filter = obj.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            
            var renderer = obj.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = new Material(Shader.Find("Standard"));
            renderer.sharedMaterial.color = color;
            renderer.sharedMaterial.EnableKeyword("_EMISSION");
            renderer.sharedMaterial.SetColor("_EmissionColor", color * 3);
            
            return obj;
        }
    }
}
