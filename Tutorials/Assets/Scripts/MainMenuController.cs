using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public GameObject mainMenu;
    public RawImage img;
    public bool fade_in = true;
    public string level_name = "PongGame";

    public void playGame()
    {
        level_name = "PongGame";
        StartCoroutine(FadeImage(fade_in));
        //SceneManager.LoadScene("PongGame");
    }

    public void playGame2P()
    {
        level_name = "PongGame2";
        StartCoroutine(FadeImage(fade_in));
        //SceneManager.LoadScene("PongGame2");
    }


    public void exitGame()
    {
        Application.Quit();
    }

    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
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
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        SceneManager.LoadScene(level_name);
    }

    // Start is called before the first frame update
    void Start()
    {  
    }

    // Update is called once per frame
    void Update()
    {   
    }
}
