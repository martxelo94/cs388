using UnityEngine;

public class Player : MonoBehaviour {

	private MazeCell currentCell;
	private MazeDirection currentDirection;

	private MazeCell keyCell;
    [HideInInspector]
    public Maze maze;
    public bool hasKey = false;
    public bool controlled = false;
    public float height = 1;
    public float width = 1.5f;
    public float speed = 1;
    public float maxSpeed = 5;

	private MazeCell Exit_Door_Cell;
	public bool maze_finish = false;


    private void Rotate(MazeDirection direction){
		transform.localRotation = direction.ToRotation ();
		currentDirection = direction;
	}
    public void UpdateLocation(MazeCell cell) {
        if (currentCell != null)
        {
            currentCell.OnPlayerExited();
        }
        currentCell = cell;
        currentCell.OnPlayerEntered();
    }

	public void SetLocation(MazeCell cell){
		if(currentCell != null){
			currentCell.OnPlayerExited();
		}
		currentCell = cell;
		transform.localPosition = cell.transform.localPosition + new Vector3(0, height, 0);
		currentCell.OnPlayerEntered ();
	}
    public void Set_Exit_Door_Cell_Location(MazeCell cell) {
        Exit_Door_Cell = cell;
    }
    public void Set_Key_Cell_Location(MazeCell cell)
	{
		keyCell = cell;
	}

	private bool Move(MazeDirection direction){
		MazeCellEdge edge = currentCell.GetEdge (direction);
		if(edge is MazePassage){
			UpdateLocation(edge.otherCell);
            return true;
		}
        return false;
	}

    private void Start()
    {
    }

    // Update is called once per frame
    void Update () {
        if (!controlled)
            return;

        Vector3 movement = new Vector3(transform.forward.x, 0, transform.forward.z) * speed * Time.deltaTime;
        Vector3 newPos = transform.position + movement;
        Vector3 volumePos = newPos + new Vector3(transform.forward.x, 0, transform.forward.z) * width;
        Vector3 localPos = volumePos - currentCell.transform.position;
        float cellHalfSize = 0.5f;
        // get direction
        MazeDirection direction = MazeDirection.None;
        if (localPos.z > cellHalfSize)
            direction = MazeDirection.North;
        else if (localPos.z < -cellHalfSize)
            direction = MazeDirection.South;
        else if (localPos.x > cellHalfSize)
            direction = MazeDirection.East;
        else if (localPos.x < -cellHalfSize)
            direction = MazeDirection.West;

        // move towards that direction
        if (direction != MazeDirection.None)
        {
            if (Move(direction))
            {
                //Instantiate(dropedObjectPrefab, transform.position, new Quaternion());
                // transform.position = newPos;
                Rigidbody rigidbody = GetComponent<Rigidbody>();
                rigidbody.velocity += movement;
                // cap velocity
                float len2 = rigidbody.velocity.sqrMagnitude;
                if (len2 > maxSpeed * maxSpeed)
                    rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
                
                
                Debug.Log("Moving to another cell");
            }
        }
        else {
            //transform.position = newPos;
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity += movement;
            // cap velocity
            float len2 = rigidbody.velocity.sqrMagnitude;
            if (len2 > maxSpeed * maxSpeed)
                rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
            Debug.Log("Moving on the same cell");
        }

        
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
