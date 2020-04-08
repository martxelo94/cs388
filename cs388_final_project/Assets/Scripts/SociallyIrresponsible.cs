using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SociallyIrresponsible : MonoBehaviour
{
    private Game game;

    public float duration = 10.0f;
    private float current_duration = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Game>();
        game.repulsion *= -1;

        // activate trigger
        foreach (Human h in game.humans) {
            h.EnableTriggerCollider(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        current_duration += Time.deltaTime;
        if (current_duration > duration)
            Destroy(this);
    }

    private void OnDestroy()
    {
        // deactivate trigger
        game.repulsion *= -1;
        if (game.humans[0] != null)
            foreach (Human h in game.humans)
            {
                if(!h.infected)
                    h.EnableTriggerCollider(false);
            }
    }
}
