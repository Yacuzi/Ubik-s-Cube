using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider))]

public class Coin : MonoBehaviour {
		public Vector3[] newVertices = new Vector3[12];
		public Vector3[] newNormals = new Vector3[12];
		public Vector2[] newUV = new Vector2[12];
		public int[] newTriangles;
		void Start() { 

				//Création du Mesh
				Mesh mesh = new Mesh ();

				//définition des sommets
				newVertices [0] = new Vector3 (-0.5F, -0.5F, -0.5F);
				newVertices [1] = new Vector3 (0.5F, -0.5F, -0.5F);
				newVertices [2] = new Vector3 (-0.5F, 0.5F, -0.5F);
				newVertices [3] = new Vector3 (0.5F, 0.5F, -0.5F);
				newVertices [4] = new Vector3 (-0.5F, -0.5F, -0.5F);
				newVertices [5] = new Vector3 (0.5F, -0.5F, -0.5F);
				newVertices [6] = new Vector3 (-0.5F, -0.5F, 0.5F);
				newVertices [7] = new Vector3 (0.5F, -0.5F, 0.5F);
				newVertices [8] = new Vector3 (-0.5F, -0.5F, 0.5F);
				newVertices [9] = new Vector3 (-0.5F, -0.5F, -0.5F);
				newVertices [10] = new Vector3 (-0.5F, 0.5F, 0.5F);
				newVertices [11] = new Vector3 (-0.5F, 0.5F, -0.5F);

				mesh.vertices = newVertices;

				//définition des normales
				newNormals [0] = new Vector3 (0,0,1);
				newNormals [1] = new Vector3 (0,0,1);
				newNormals [2] = new Vector3 (0,0,1);
				newNormals [3] = new Vector3 (0,0,1);
				newNormals [4] = new Vector3 (0,1,0);
				newNormals [5] = new Vector3 (0,1,0);
				newNormals [6] = new Vector3 (0,1,0);
				newNormals [7] = new Vector3 (0,1,0);
				newNormals [8] = new Vector3 (1,0,0);
				newNormals [9] = new Vector3 (1,0,0);
				newNormals [10] = new Vector3 (1,0,0);
				newNormals [11] = new Vector3 (1,0,0);

				mesh.normals = newNormals;

				//définition des triangles
				newTriangles = new int[18] {0,1,2,1,3,2,4,6,5,6,7,5,8,9,10,9,11,10};

				mesh.triangles = newTriangles;

				//définition de l'UV map
				newUV [0] = new Vector2 (1,0);
				newUV [1] = new Vector2 (0,0);
				newUV [2] = new Vector2 (1,1);
				newUV [3] = new Vector2 (1,0);
				newUV [4] = new Vector2 (1,1);
				newUV [5] = new Vector2 (1,0);
				newUV [6] = new Vector2 (0,1);
				newUV [7] = new Vector2 (0,0);
				newUV [8] = new Vector2 (0,1);
				newUV [9] = new Vector2 (0,0);
				newUV [10] = new Vector2 (1,1);
				newUV [11] = new Vector2 (1,0);

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

				//Sauvegarde du mesh
				//AssetDatabase.CreateAsset(mesh, "Assets/Meshes/Coin.asset");
				//AssetDatabase.SaveAssets();

		}
}