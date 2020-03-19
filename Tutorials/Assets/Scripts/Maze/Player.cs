using UnityEngine;

public class Player : MonoBehaviour {

	private MazeCell currentCell;
	private MazeDirection currentDirection;

	private MazeCell keyCell;
    public Maze maze;
    public bool hasKey = false;
    public bool controlled = false;
    public float height = 1;
    public float speed = 1;

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

	private bool Move(MazeDirection direction){
		MazeCellEdge edge = currentCell.GetEdge (direction);
		if(edge is MazePassage){
			SetLocation(edge.otherCell);
            return true;
		}
        return false;
	}

	// Update is called once per frame
	void Update () {
        if (!controlled)
            return;

        Vector3 movement = new Vector3(transform.forward.x, 0, transform.forward.z) * speed * Time.deltaTime;
        Vector3 newPos = transform.position + movement;
        Vector3 localPos = newPos - currentCell.transform.position;
        float cellHalfSize = 0.5f;
        // get direction
        MazeDirection direction = MazeDirection.None;
        if (localPos.z > cellHalfSize)
            direction = MazeDirection.North;
        else if (localPos.z < cellHalfSize)
            direction = MazeDirection.South;
        else if (localPos.x > cellHalfSize)
            direction = MazeDirection.East;
        else if (localPos.x < cellHalfSize)
            direction = MazeDirection.West;

        // move towards that direction
        if (direction != MazeDirection.None)
        {
            if (Move(direction))
                transform.position = newPos;
        }
        else
            transform.position = newPos;

        
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
	}
}
