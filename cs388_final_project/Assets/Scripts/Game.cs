using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Game : MonoBehaviour
{
    public int throws = 1;
    public List<Human> humans;
    public Human patientZeroPrefab;
    private Human patientZeroInstance;
    public LineRenderer slingPrefab;
    private LineRenderer slingInstance;

    Vector2 TouchPos()
    {
        Vector2 screen_pos = Vector2.zero;
        if (Input.touchCount > 0) {
            screen_pos = Input.touches[0].position;
        }
        else {
            screen_pos = Input.mousePosition;
        }
        Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector3(screen_pos.x, screen_pos.y, -Camera.main.transform.position.z));
        Debug.Log("Touch pos = " + pos);
        return pos;
    }

    // Start is called before the first frame update
    void Start()
    {
        Human[] human_array = FindObjectsOfType<Human>();
        humans = new List<Human>(human_array);
    }

    // Update is called once per frame
    void Update()
    {
        // RECOVER HUMANS
        foreach (Human h in humans) {
            if (h.infected) {
                h.Recover();
            }
        }

        if(throws > 0)
        {
            // INPUT LAUNCH
            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 0.0f;
                // spawn patient ZERO
                Vector2 pos2D = TouchPos();
                Vector3 pos3D = new Vector3(pos2D.x, pos2D.y, -1.0f);
                slingInstance = Instantiate<LineRenderer>(slingPrefab, Vector3.zero, new Quaternion());
                slingInstance.SetPosition(0, pos3D);
                patientZeroInstance = Instantiate<Human>(patientZeroPrefab, pos3D, new Quaternion());
                patientZeroInstance.Infect();
                humans.Add(patientZeroInstance);

            }
            if (Input.GetMouseButton(0))
            {
                Vector2 pos2D = TouchPos();
                Vector3 pos3D = new Vector3(pos2D.x, pos2D.y, -1.0f);
                slingInstance.SetPosition(1, pos3D);
            }
            if (Input.GetMouseButtonUp(0))
            {
                Time.timeScale = 1.0f;
                // get velocity
                Vector2 v = slingInstance.GetPosition(0) - slingInstance.GetPosition(1);
                Rigidbody2D rig = patientZeroInstance.GetComponent<Rigidbody2D>();
                rig.velocity = v;
                Destroy(slingInstance.gameObject);

                // update throws
                throws--;
            }

        }
    }
}
