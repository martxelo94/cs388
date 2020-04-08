using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    public float repulsion = 100.0f;
    public float speed = 1.0f;
    public float maxSpeed = 3.0f;
    public float infectChance = 0.1f;
    public bool infected = false;
    public float recover_time = 5.0f;
    private float recover_current_time = 0.0f;
    public Vector2 initialDirection;

    private Game game;
    [HideInInspector]
    public Rigidbody2D rig;

    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Game>();

        //disable particle emmiter
        particles = GetComponent<ParticleSystem>();
        particles.Stop();

        if (infected)
            Infect();

        // apply velocity
        rig = GetComponent<Rigidbody2D>();
        rig.velocity = initialDirection.normalized * speed;
    }

    // Update is called once per frame
    void Update()
    {
        // cap speed
        if (rig.velocity.sqrMagnitude > maxSpeed * maxSpeed) {
            rig.velocity = rig.velocity.normalized * maxSpeed;
        }
    }

    public void EnableTriggerCollider(bool enable) {
        CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();
        colliders[1].enabled = false;
    }

    public bool Recover() {
        recover_current_time += Time.deltaTime;
        particles.emissionRate = recover_time - recover_current_time;
        if (recover_current_time > recover_time) {
            infected = false;
            recover_current_time = 0.0f;
            // change material
            GetComponent<MeshRenderer>().material.color = Color.white;
            EnableTriggerCollider(false);

            //disable particle emmiter
            particles.Stop();

            //update infected count
            game.infected_count--;
            return true;
        }
        return false;
    }

    public void Infect()
    {
        if (!infected) {
            infected = true;
            // change material
            GetComponent<MeshRenderer>().material.color = Color.red;
            // activate trigger collider
            EnableTriggerCollider(true);
            //activate particle emmiter
            particles.emissionRate = recover_time;
            particles.Play();
            
            //update infected count
            game.infected_count++;
        }
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
    private void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("Trigger");
        if (other.isTrigger == false) {
            Rigidbody2D other_rig = other.GetComponent<Rigidbody2D>();
            if (other_rig != null) {
                Vector2 force = other.transform.position - transform.position;
                other_rig.AddForce(force.normalized * repulsion);
            }
            
        }
    }

#if false   // no randomness
    private void OnTriggerStay2D(Collider2D other)
    {
        if (infected)
        {
            Human other_human = other.GetComponent<Human>();
            if (other_human != null)
            {
                Debug.Log("Infecting");
                // chance to infect
                float rand = Random.Range(0.0f, 100.0f);
                if (rand <= infectChance)
                    other_human.Infect();
            }

        }
    }
#endif
}
