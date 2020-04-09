using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTheEconomy : MonoBehaviour
{
    private Game game;
    private PanelOpener panelOpener;

    public float duration = 5.0f;
    private float current_duration = 0.0f;

    public float speedFactor = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Game>();
        panelOpener = FindObjectOfType<PanelOpener>();

        game.maxSpeed *= speedFactor;
        foreach (Human h in game.humans) {
            h.rig.velocity *= speedFactor;
        }

        AudioSource audio = Camera.main.GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Sounds/Save-The-Economy");
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
        if (game != null) {
            game.maxSpeed /= speedFactor;
            foreach (Human h in game.humans)
            {
                h.rig.velocity /= speedFactor;
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
