using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    // game state
    public int infected_goal = -1;  // -1 means all infected
    public float game_time = 20.0f;
    private float current_game_time = 0.0f;
    public int throws = 1;
    private bool is_throwing = false;
    [HideInInspector]
    public int infected_count = 0;

    // objects
    public List<Human> humans;
    public Human patientZeroPrefab;
    private Human patientZeroInstance;
    public LineRenderer slingPrefab;
    private LineRenderer slingInstance;

    // human stats
    public float repulsion = 100.0f;
    public float speed = 1.0f;
    public float maxSpeed = 3.0f;
    public float infectChance = 0.1f;
    public float recover_time = 5.0f;

    public int getInfectedCount() { return infected_count; }
    public int getHealthyCount() { return humans.Count - infected_count; }

    bool CompletedGoal() {
        if (infected_goal < 0)
        {
            return getHealthyCount() == 0;
        }
        else {
            return infected_count == infected_goal;
        }
    }

    // screen space
    Vector2 RawTouchPos()
    {
        if (Input.touchCount > 0)
        {
            return Input.touches[0].position;
        }
        else
        {
            return Input.mousePosition;
        }
    }

    // world space
    Vector2 TouchPos()
    {
        Vector2 screen_pos = RawTouchPos();
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

    public void Ability_MutantVirus() {
        gameObject.AddComponent<MutantVirus>();
    }
    public  void Ability_PanicShoping() {
        gameObject.AddComponent<PanicShoping>();
    }
   public void Ability_SociallyIrresponsible() {
        gameObject.AddComponent<SociallyIrresponsible>();
    }
    public void Ability_SaveTheEconomy() {
        gameObject.AddComponent<SaveTheEconomy>();
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
        // update game time
        current_game_time += Time.deltaTime;
        if (CompletedGoal()) {
            // WIN
            
        }
        if (current_game_time > game_time) {
            // LOSE
        }

        // INVOKE ABILITIES (HAX)
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Ability_MutantVirus();
            Debug.Log("1 MutantVirus");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Ability_PanicShoping();
            Debug.Log("2 PanicShoping");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Ability_SociallyIrresponsible();
            Debug.Log("3 SociallyIrresponsible");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Ability_SaveTheEconomy();
            Debug.Log("4 SaveTheEconomy");
        }

        // RECOVER HUMANS
        foreach (Human h in humans) {
            if (h.infected) {
                //mark stats as recover, +1, decrease infected -1
                h.Recover();
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
                // do not spawn in colliders
                RaycastHit2D hit = Physics2D.Raycast(pos2D, Vector2.zero, Mathf.Infinity, 1 << 8);
                if (hit.collider == null)
                {
                    slingInstance = Instantiate<LineRenderer>(slingPrefab, Vector3.zero, new Quaternion());
                    slingInstance.SetPosition(0, pos3D);
                    patientZeroInstance = Instantiate<Human>(patientZeroPrefab, pos3D, new Quaternion());

                    is_throwing = true;

                }
                else {
                    Debug.Log("Hit: " + hit.collider.gameObject.name);
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (is_throwing) {
                    Vector2 pos2D = TouchPos();
                    Vector3 pos3D = new Vector3(pos2D.x, pos2D.y, -1.0f);
                    slingInstance.SetPosition(1, pos3D);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                Time.timeScale = 1.0f;
                if (is_throwing) {
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
}
