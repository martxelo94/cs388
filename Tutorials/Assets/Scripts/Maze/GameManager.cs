﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameManager : MonoBehaviour {

	public Player playerInstance;
    public Camera mapCameraLeft;
    public Camera mapCameraRight;

	public Maze mazePrefab;
	private Maze mazeInstance;

	public Key keyPrefab;
	private Key keyInstance;

	public Exit_Door ExitPrefab;
	private Exit_Door ExitInstance;

		private bool playerInitialize = false;

    // Use this for initialization

    public void StartGame() {
        playerInstance.controlled = false;

        // set stats
        Transform cam_l = playerInstance.transform.GetChild(1);
        Transform cam_r = playerInstance.transform.GetChild(2);
        cam_l.localPosition = new Vector3(-Stats.eyeSeparation, cam_l.localPosition.y, cam_l.localPosition.z);
        cam_r.localPosition = new Vector3(Stats.eyeSeparation, cam_r.localPosition.y, cam_r.localPosition.z);
        UnityStandardAssets.ImageEffects.Fisheye fish_l = cam_l.GetComponent<UnityStandardAssets.ImageEffects.Fisheye>();
        UnityStandardAssets.ImageEffects.Fisheye fish_r = cam_r.GetComponent<UnityStandardAssets.ImageEffects.Fisheye>();
        fish_l.strengthX = fish_r.strengthX = Stats.eyeBarrel;

        StartCoroutine(BeginGame());
    }

	void Start () {
        StartGame();
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

        mapCameraLeft.clearFlags = CameraClearFlags.Skybox;
        mapCameraLeft.rect = new Rect (0f, 0f, 0.5f, 1f);
        mapCameraRight.clearFlags = CameraClearFlags.Skybox;
        mapCameraRight.rect = new Rect(0.5f, 0f, 1f, 1f);

        mazeInstance = Instantiate (mazePrefab) as Maze;
        // set maze to player
        playerInstance.maze = mazeInstance;
		yield return StartCoroutine(mazeInstance.Generate ());
		playerInstance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));


		keyInstance = Instantiate(keyPrefab) as Key;
		keyInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

		ExitInstance = Instantiate(ExitPrefab) as Exit_Door;
		ExitInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

		playerInstance.Set_Key_Cell_Location(keyInstance.currentCell);
		playerInstance.Set_Exit_Door_Cell_Location(ExitInstance.currentCell);

		playerInitialize = true;
        playerInstance.controlled = true;

        mapCameraLeft.clearFlags = CameraClearFlags.Depth;
        mapCameraLeft.rect = new Rect(0f, 0f, 0.5f, 0.5f);
        mapCameraRight.clearFlags = CameraClearFlags.Depth;
        mapCameraRight.rect = new Rect(0.5f, 0f, 1, 0.5f);

    }

    private void RestartGame(){
        StopAllCoroutines ();
		Destroy (mazeInstance.gameObject);
         // OJITO CON BORRAR TODO A MANO, mejor restart el Level entero!
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        /*
		if(playerInstance != null){
            // NO!
            //Destroy (playerInstance.gameObject);
            // Change level instead or restart full level
            //Application.LoadLevel(Application.loadedLevel);   // DEPRECATED
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
        */
	}
}
