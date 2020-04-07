using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    public float attraction = 100.0f;
    public float speed = 1.0f;
    public bool infected = false;
    public float recover_time = 5.0f;
    private float recover_current_time = 0.0f;
    public Vector2 initialDirection;

    private Game game;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Game>();

        if(infected)
            Infect();

        // apply velocity
        Rigidbody2D rig = GetComponent<Rigidbody2D>();
        rig.velocity = initialDirection.normalized * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Recover() {
        recover_current_time += Time.deltaTime;
        if (recover_current_time > recover_time) {
            infected = false;
            recover_current_time = 0.0f;
            Object mat = Resources.Load("Materials/mat_Blue");
            GetComponent<SpriteRenderer>().material = mat as Material;
            return true;
        }
        return false;
    }

    public void Infect()
    {
        infected = true;
        // change material
        Object mat = Resources.Load("Materials/mat_Red");
        GetComponent<SpriteRenderer>().material = mat as Material;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided");
        if (infected) {
            Human h = collision.gameObject.GetComponent<Human>();
            if (h != null) {
                h.Infect();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger");
        if (infected) {
            Rigidbody2D other_rig = other.GetComponent<Rigidbody2D>();
            if (other_rig != null) {
                Vector2 force = other.transform.position - transform.position;
                other_rig.AddForce(force * attraction);
            }
            
        }
    }
}
