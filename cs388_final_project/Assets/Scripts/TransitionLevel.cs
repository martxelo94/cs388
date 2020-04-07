using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionLevel : MonoBehaviour
{
    //public Image img;
    public RawImage img;
    public bool fade_in = true;
    public string level_name = "MainMenu";

    public void changeMainMenu()
    {
        //apply fade in an amount of time

        // fades the image out when you click
       StartCoroutine(FadeImage(fade_in));

    }

    public void ChangeLevelName(string name) { level_name = name; }

    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 2; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 2; i += Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return new WaitForSeconds(Time.deltaTime); 
            }
        }
        SceneManager.LoadScene(level_name);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        changeMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
