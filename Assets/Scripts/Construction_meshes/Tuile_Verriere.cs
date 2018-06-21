using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider))]

public class Tuile_Verriere : MonoBehaviour {
		public Vector3[] newVertices = new Vector3[4];
		public Vector3[] newNormals = new Vector3[4];
		public Vector2[] newUV = new Vector2[4];
		public int[] newTriangles;
		void Start() {

				//Création du Mesh
				Mesh mesh = new Mesh ();

				//définition des sommets
				newVertices [0] = new Vector3 (-0.5F, -0.5F, -0.5F);
				newVertices [1] = new Vector3 (0.5F, -0.5F, -0.5F);
				newVertices [2] = new Vector3 (-0.5F, 0.5F, -0.5F);
				newVertices [3] = new Vector3 (0.5F, 0.5F, -0.5F);

				mesh.vertices = newVertices;

				//définition des normales
				newNormals [0] = new Vector3 (0,0,1);
				newNormals [1] = new Vector3 (0,0,1);
				newNormals [2] = new Vector3 (0,0,1);
				newNormals [3] = new Vector3 (0,0,1);

				mesh.normals = newNormals;

				//définition des triangles
				newTriangles = new int[6] {2,1,0,2,3,1};

				mesh.triangles = newTriangles;

				//définition de l'UV map
				newUV [0] = new Vector2 (1,1);
				newUV [1] = new Vector2 (1,0);
				newUV [2] = new Vector2 (0,1);
				newUV [3] = new Vector2 (0,0);

				mesh.uv = newUV;


				//Calculer les normales
				//mesh.RecalculateNormals ();


				//grab our filter.. set the mesh
				MeshFilter filter = GetComponent<MeshFilter> ();
				filter.mesh = mesh;

				//Mesh Collider
				MeshCollider collider = GetComponent<MeshCollider>();
				collider.sharedMesh = mesh;

				//you can do your material stuff here...
				//MeshRenderer r = GetComponent<MeshRenderer>();
				//r.material = new Material(Shader.Find("my_shader_here"));

		}
}