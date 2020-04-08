using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supermarket : MonoBehaviour
{
    private Game game;
    private CircleCollider2D collider;

    //[HideInInspector]
    public bool pushHumans = true;

    public float pushForce = 50.0f;
    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Game>();
        collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pushHumans == false) {
            // pull humans
            foreach (Human h in game.humans) {
                Vector2 dir = transform.position - h.transform.position;
                h.rig.velocity += dir.normalized * Time.deltaTime * game.speed;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (pushHumans) {
            if (other.isTrigger == false) {
                Human h = other.GetComponent<Human>();
                if (h != null) {
                    Vector2 dir = other.transform.position - transform.position;
                    h.rig.velocity += dir.normalized * Time.deltaTime * game.speed;
                    Debug.Log("PushHuman from Supermarket");
                }
            }
        }
    }

    public void SetPushHumans(bool push) {
        pushHumans = push;
        GetComponent<BoxCollider2D>().enabled = !push;

    }
}
