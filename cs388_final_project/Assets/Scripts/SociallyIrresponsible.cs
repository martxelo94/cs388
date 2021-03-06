﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SociallyIrresponsible : MonoBehaviour
{
    private Game game;
    private PanelOpener panelOpener;

    public float duration = 5.0f;
    private float current_duration = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Game>();
        panelOpener = FindObjectOfType<PanelOpener>();

        game.repulsion *= -1;

        // activate trigger
        foreach (Human h in game.humans) {
            h.EnableTriggerCollider(true);
        }

        AudioSource audio = Camera.main.GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Sounds/Socially-Irresponsible");
        audio.PlayOneShot(audio.clip);

        // deactivate buttons
        foreach (UnityEngine.UI.Button b in panelOpener.buttons_to_deactivate)
        {
            b.interactable = false;
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
        if (game.humans[0] != null) {
            foreach (Human h in game.humans)
            {
                if(!h.infected)
                    h.EnableTriggerCollider(false);
            }
        }
        if (panelOpener != null)
        {
            foreach (UnityEngine.UI.Button b in panelOpener.buttons_to_deactivate)
            {
                b.interactable = true;
            }
            panelOpener.buttons_to_deactivate.Clear();
        }
    }
}
