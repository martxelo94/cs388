using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Game : MonoBehaviour
{
    public int throws = 1;
    private bool is_throwing = false;
    public List<Human> humans;
    public Human patientZeroPrefab;
    private Human patientZeroInstance;
    public LineRenderer slingPrefab;
    private LineRenderer slingInstance;

    [HideInInspector]
    public int infected_count = 0;

    public int getInfectedCount() { return infected_count; }
    public int getHealthyCount() { return humans.Count - infected_count; }

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
        //Debug.Log("Touch pos = " + pos);
        return pos;
    }

    bool InputCancel()
    {
        if (Input.touchCount > 0)
        {
            return Input.touchCount == 2;
        }
        else {
            return Input.GetMouseButton(1);
        }

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
        // INVOKE ABILITIES
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            gameObject.AddComponent<MutantVirus>();
            Debug.Log("1 MutantVirus");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameObject.AddComponent<PanicShoping>();
            Debug.Log("2 PanicShoping");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gameObject.AddComponent<SociallyIrresponsible>();
            Debug.Log("3 SociallyIrresponsible");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gameObject.AddComponent<SaveTheEconomy>();
            Debug.Log("4 SaveTheEconomy");
        }

        // RECOVER HUMANS
        foreach (Human h in humans) {
            if (h.infected) {
                h.Recover();

                //mark stats as recover, +1, decrease infected -1
            }
        }

        if(throws > 0 || throws < 0)
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

                is_throwing = true;
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
                if (!InputCancel())
                {
                    // get velocity
                    Vector2 v = slingInstance.GetPosition(0) - slingInstance.GetPosition(1);
                    Rigidbody2D rig = patientZeroInstance.GetComponent<Rigidbody2D>();
                    rig.velocity = v;

                    patientZeroInstance.Infect();
                    humans.Add(patientZeroInstance);

                }
                else {
                   Destroy(patientZeroInstance.gameObject);
                }
                Destroy(slingInstance.gameObject);

                // update throws
                throws--;

                is_throwing = false;
            }

        }
    }
}
