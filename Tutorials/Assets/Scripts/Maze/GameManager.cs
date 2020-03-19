using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Player playerInstance;

	public Maze mazePrefab;
	private Maze mazeInstance;

	public Key keyPrefab;
	private Key keyInstance;

	// Use this for initialization
	void Start () {
        playerInstance.controlled = false;
		StartCoroutine(BeginGame ());
	}
	
	// Update is called once per frame
	void Update () {

		if(playerInstance.hasKey)
		{
			keyInstance.Hide();
		}


		if(Input.GetKeyDown(KeyCode.Space)){
			RestartGame();
		}
	}

	private IEnumerator BeginGame(){
		//Camera.main.clearFlags = CameraClearFlags.Skybox;
		//Camera.main.rect = new Rect (0f, 0f, 1f, 1f);
		mazeInstance = Instantiate (mazePrefab) as Maze;
        // set maze to player
        playerInstance.maze = mazeInstance;
		yield return StartCoroutine(mazeInstance.Generate ());
		playerInstance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));

		keyInstance = Instantiate(keyPrefab) as Key;
		keyInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

		playerInstance.Set_Key_Cell_Location(keyInstance.currentCell);
        playerInstance.controlled = true;
		//Camera.main.clearFlags = CameraClearFlags.Depth;
		//Camera.main.rect = new Rect (0f, 0f, 0.5f, 0.5f);
	}

	private void RestartGame(){
		StopAllCoroutines ();
		Destroy (mazeInstance.gameObject);
		if(playerInstance != null){
			Destroy (playerInstance.gameObject);
		}

		if (keyInstance != null)
		{
			Destroy(keyInstance.gameObject);
		}

		StartCoroutine(BeginGame ());
	}
}
