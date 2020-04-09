using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanicShoping : MonoBehaviour
{
    private PanelOpener panelOpener;

    Supermarket[] supers;

    public float duration = 5.0f;
    private float current_duration = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        panelOpener = FindObjectOfType<PanelOpener>();
        supers = FindObjectsOfType<Supermarket>();
        foreach (Supermarket s in supers) {
            s.SetPushHumans(false);
        }

        AudioSource audio = Camera.main.GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Sounds/Panic-Shoping");
        audio.PlayOneShot(audio.clip);

        // deactivate buttons
        foreach (UnityEngine.UI.Button b in panelOpener.buttons_to_deactivate) {
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
        foreach (Supermarket s in supers)
        {
            if(s != null)
                s.SetPushHumans(true);
        }
        if (panelOpener != null)
        {
            // reactivate buttons
            foreach (UnityEngine.UI.Button b in panelOpener.buttons_to_deactivate)
            {
                b.interactable = true;
            }
            panelOpener.buttons_to_deactivate.Clear();
        }
    }
}
