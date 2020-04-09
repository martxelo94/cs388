using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject win_panel;
    public GameObject lose_panel;
    public GameObject stats_panel;
    public GameObject goal_panel;
    public UnityEngine.UI.Text goal_text;
    public UnityEngine.UI.Text infected_text;
    public UnityEngine.UI.Text healthy_text;
    public UnityEngine.UI.Text time_text;
    public UnityEngine.UI.Text throws_text;

    private Game game;

    [HideInInspector]
    public List<UnityEngine.UI.Button> buttons_to_deactivate;


    public void AddButtonToDeactivate(UnityEngine.UI.Button button)
    {
        buttons_to_deactivate.Add(button);
    }
    private void Start()
    {
        game = FindObjectOfType<Game>();
    }

    private void Update()
    {
        infected_text.text = game.getInfectedCount().ToString();
        healthy_text.text = game.getHealthyCount().ToString();
        time_text.text = ((int)(game.game_time - game.current_game_time)).ToString();
        throws_text.text = game.throws.ToString();
        if (game.infected_goal < 0)
        {
            goal_text.text = "Infect all humans!";
        }
        else {
            goal_text.text = "Infect " + game.infected_goal.ToString() + " humans!";
        }

        // check win condition
        if (game.game_ended == false) {
            if (game.CompletedGoal())
            {
                // WIN
                win_panel.SetActive(true);
                game.game_ended = true;
            }
            else if (game.current_game_time > game.game_time)
            {
                // LOSE
                lose_panel.SetActive(true);
                time_text.color = Color.red;
                game.game_ended = true;
            }
        }
    }

    public void SwitchActive(GameObject obj) {
        obj.SetActive(!obj.activeSelf);
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
