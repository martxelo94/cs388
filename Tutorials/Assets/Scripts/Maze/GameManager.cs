using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Player playerInstance;

	public Maze mazePrefab;
	private Maze mazeInstance;

	public Key keyPrefab;
	private Key keyInstance;

	public Exit_Door ExitPrefab;
	private Exit_Door ExitInstance;

		private bool playerInitialize = false;

	// Use this for initialization
	void Start () {
        playerInstance.controlled = false;
		StartCoroutine(BeginGame ());
	}
	
	// Update is called once per frame
	void Update () {

		if (playerInitialize == true)
		{
			if (playerInstance.hasKey == true)
			{
				keyInstance.Hide();
			}

			if (playerInstance.maze_finish == true)
			{
				//FINISHHH -> GO TO START MENU
				RestartGame();
			}
		}

		if(Input.GetKeyDown(KeyCode.Space)){
			RestartGame();
		}
	}

	private IEnumerator BeginGame(){
		//Camera.main.clearFlags = CameraClearFlags.Skybox;
		//Camera.main.rect = new Rect (0f, 0f, 1f, 1f);
		mazeInstance = Instantiate (mazePrefab) as Maze;
		yield return StartCoroutine(mazeInstance.Generate ());
		playerInstance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));


		keyInstance = Instantiate(keyPrefab) as Key;
		keyInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

		ExitInstance = Instantiate(ExitPrefab) as Exit_Door;
		ExitInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

		playerInstance.Set_Key_Cell_Location(keyInstance.currentCell);
		playerInstance.Set_Exit_Door_Cell_Location(ExitInstance.currentCell);

		playerInitialize = true;
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

		if (ExitInstance != null)
		{
			Destroy(ExitInstance.gameObject);
		}

		StartCoroutine(BeginGame ());
	}
}
