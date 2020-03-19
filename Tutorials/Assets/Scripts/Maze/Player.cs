﻿using UnityEngine;

public class Player : MonoBehaviour {

	private MazeCell currentCell;
	private MazeDirection currentDirection;

	private MazeCell keyCell;
	public bool hasKey = false;
    public bool controlled = false;
    public float height = 1;

	private MazeCell Exit_Door_Cell;
	public bool maze_finish = false;

	private void Rotate(MazeDirection direction){
		transform.localRotation = direction.ToRotation ();
		currentDirection = direction;
	}

	public void SetLocation(MazeCell cell){
		if(currentCell != null){
			currentCell.OnPlayerExited();
		}
		currentCell = cell;
		transform.localPosition = cell.transform.localPosition + new Vector3(0, height, 0);
		currentCell.OnPlayerEntered ();
	}

	public void Set_Key_Cell_Location(MazeCell cell)
	{
		keyCell = cell;
	}

	public void Set_Exit_Door_Cell_Location(MazeCell cell)
	{
		Exit_Door_Cell = cell;
	}

	private void Move(MazeDirection direction){
		MazeCellEdge edge = currentCell.GetEdge (direction);
		if(edge is MazePassage){
			SetLocation(edge.otherCell);
		}
	}

	// Update is called once per frame
	void Update () {
        if (!controlled)
            return;

		if (Input.GetKeyDown(KeyCode.W)) {
			Move(currentDirection);
		}
		else if (Input.GetKeyDown(KeyCode.D)) {
			Move(currentDirection.GetNextClockwise());
		}
		else if (Input.GetKeyDown(KeyCode.S)) {
			Move(currentDirection.GetOpposite());
		}
		else if (Input.GetKeyDown(KeyCode.A)) {
			Move(currentDirection.GetNextCounterclockwise());
		}
		else if (Input.GetKeyDown(KeyCode.Q)){
			Rotate (currentDirection.GetNextCounterclockwise());
		}
		else if (Input.GetKeyDown(KeyCode.E)){
			Rotate (currentDirection.GetNextClockwise());
		}

		if(currentCell == keyCell)
		{
			hasKey = true;
		}

		if((hasKey == true) && (currentCell == Exit_Door_Cell))
		{
			// finish game
			maze_finish = true;
		}
	}
}
