using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    public bool infected = false;
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
        rig.velocity = initialDirection;
    }

    // Update is called once per frame
    void Update()
    {
        // cap speed
        float maxSpeed = game.maxSpeed;
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
        float recover_time = game.recover_time;
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
            particles.emissionRate = game.recover_time;
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
            Human other_human = other.GetComponent<Human>();
            if (other_human != null)
            {
                Vector2 force = other.transform.position - transform.position;
                other_human.rig.AddForce(force.normalized * game.repulsion);
                // chance to infect
                float rand = Random.Range(0.0f, 100.0f);
                if (rand <= game.infectChance) {
                    other_human.Infect();
                    Debug.Log("Infecting by proximity");
                }
            }
        }
    }
}
