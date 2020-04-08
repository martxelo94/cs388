using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject stats_panel;
    public GameObject goal_panel;
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

    public void Open_Stats_Panel()
    {
        if(stats_panel != null)
        {
            bool isActive = stats_panel.activeSelf;
            stats_panel.SetActive(!isActive);
        }
    }

    public void Open_Goal_Panel()
    {
        if (goal_panel != null)
        {
            bool isActive = goal_panel.activeSelf;
            goal_panel.SetActive(!isActive);
        }
    }
}
