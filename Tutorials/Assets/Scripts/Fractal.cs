using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {

	public float childScale;
	public int maxDepth;
	private int depth;

	private static Vector3[] childDirections = {
		Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back, Vector3.down
	};
	private static Quaternion[] childOriantations = {
		Quaternion.identity,
		Quaternion.Euler (0f, 0f, -90f),
		Quaternion.Euler (0f, 0f, 90f),
		Quaternion.Euler (90f, 0f, 0f),
		Quaternion.Euler (-90f, 0f, 0f),
		Quaternion.Euler (180f, 0f, 0f)
	};
	private Material[,] materials;

	public float maxTwist;
	public float maxRotationSpeed;
	private float rotationSpeed;
	[Range(0f,1f)]
	public float spawnProbability;
	public Mesh[] meshes;
	public Material material;
	// Use this for initialization
	void Start () {
		transform.Rotate (Random.Range (-maxTwist, maxTwist), 0f, 0f);
		rotationSpeed = Random.Range (-maxRotationSpeed, maxRotationSpeed);
		if (materials == null)
			InitializeMaterials ();
		gameObject.AddComponent<MeshFilter> ().mesh = meshes[Random.Range (0, meshes.Length)];
		gameObject.AddComponent<MeshRenderer> ().material = materials[depth , Random.Range(0, 2)];
		if(depth < maxDepth){
			StartCoroutine(CreateChildren());
		}
	}

	private void Update(){
		transform.Rotate (0f, rotationSpeed * Time.deltaTime, 0f);
	}

	private void Initialize(Fractal parent, Vector3 direction, Quaternion orientation){
		meshes = parent.meshes;
		materials = parent.materials;
		maxDepth = parent.maxDepth;
		depth = parent.depth + 1;
		childScale = parent.childScale;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = direction * (0.5f + 0.5f * childScale);
		transform.localRotation = orientation;
		spawnProbability = parent.spawnProbability;
		maxRotationSpeed = parent.maxRotationSpeed;
		maxTwist = parent.maxTwist;
	}

	IEnumerator CreateChildren(){
		for(int i=0; i<childDirections.Length; i++){
			if(Random.value < spawnProbability){
				yield return new WaitForSeconds (Random.Range (0.1f, 0.5f));
				new GameObject ("Fractal Child").AddComponent<Fractal> ().Initialize(this, childDirections[i], childOriantations[i]);
			}
		}
	}

	private void InitializeMaterials(){
		materials = new Material[maxDepth + 1 , 2];
		for(int i = 0; i<= maxDepth; i++){
			float t = i/(maxDepth - 1f);
			t *= t;
			materials[i, 0] = new Material(material);
			materials[i, 0].color = Color.Lerp (Color.white, Color.yellow, t);
			materials[i, 1] = new Material(material);
			materials[i, 1].color = Color.Lerp (Color.white, Color.cyan, t);

		}
		materials [maxDepth, 0].color = Color.magenta;
		materials [maxDepth, 1].color = Color.red;
	}
}
