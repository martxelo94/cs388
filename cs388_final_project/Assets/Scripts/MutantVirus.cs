using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantVirus : MonoBehaviour
{
    private Game game;
    private PanelOpener panelOpener;

    public float duration = 10.0f;
    private float current_duration = 0.0f;

    public float recoverFactor = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Game>();
        panelOpener = FindObjectOfType<PanelOpener>();

        game.recover_time *= recoverFactor;

        AudioSource audio = Camera.main.GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Sounds/Mutant-Virus");
        audio.PlayOneShot(audio.clip);

        // deactivate buttons
        foreach (UnityEngine.UI.Button b in panelOpener.buttons_to_deactivate)
        {
            b.interactable = false;
        }

        Debug.Log("Mutant Virus Started");
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
        Debug.Log("Mutant Virus Ended");

        // reactivate buttons
        if (game != null) {
            game.recover_time /= recoverFactor;
        }
        if (panelOpener != null) {
            foreach (UnityEngine.UI.Button b in panelOpener.buttons_to_deactivate)
            {
                b.interactable = true;
            }
            panelOpener.buttons_to_deactivate.Clear();
        }
    }
}
