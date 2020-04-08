using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject panel;
    public UnityEngine.UI.Text infected_text;
    public UnityEngine.UI.Text death_text;
    public UnityEngine.UI.Text healthy_text;

    private Game game;

    private void Start()
    {
        game = FindObjectOfType<Game>();
    }

    private void Update()
    {
        infected_text.text = game.getInfectedCount().ToString();
        healthy_text.text = game.getHealthyCount().ToString();
    }

    public void OpenPanel()
    {
        if(panel != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive);
        }
    }
}
