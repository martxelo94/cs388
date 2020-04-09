using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject help_panel;
    public RawImage img;
    public bool fade_in = true;
    public string level_name = "Level0";

    public void playGame()
    {
        level_name = "Level0";
        StartCoroutine(FadeImage(fade_in));
        //SceneManager.LoadScene("PongGame");
    }

    public void LoadHelp()
    {
        level_name = "HelpMenu";
        StartCoroutine(FadeImage(fade_in));
    }

    public void LoadMainMenu()
    {
        level_name = "MainMenu";
        StartCoroutine(FadeImage(fade_in));
    }

    public void Open_Help_Panel()
    {
        if (help_panel != null)
        {
            bool isActive = help_panel.activeSelf;
            help_panel.SetActive(!isActive);
        }
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
}
