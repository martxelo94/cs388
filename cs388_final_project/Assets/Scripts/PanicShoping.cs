using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanicShoping : MonoBehaviour
{
    Supermarket[] supers;

    public float duration = 10.0f;
    private float current_duration = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        supers = FindObjectsOfType<Supermarket>();
        foreach (Supermarket s in supers) {
            s.SetPushHumans(false);
        }

        AudioSource audio = Camera.main.GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Sounds/Panic-Shoping");
        audio.PlayOneShot(audio.clip);
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
    }
}
