using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTheEconomy : MonoBehaviour
{
    private Game game;

    public float duration = 10.0f;
    private float current_duration = 0.0f;

    public float speedFactor = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Game>();
        foreach (Human h in game.humans) {
            h.rig.velocity *= speedFactor;
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
        if(game.humans[0] != null)
        foreach (Human h in game.humans)
        {
            h.rig.velocity /= speedFactor;
        }
    }
}
