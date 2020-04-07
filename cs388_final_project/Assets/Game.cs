using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Game : MonoBehaviour
{
    public BoxCollider2D[] players;
    public bool[] ai_players;

    private int[] directions;

    public CircleCollider2D ball;

    public float player_speed = 1.0f;
    public float ball_speed = 10.0f;
    public Text score_left;
    int points_left = 0;
    public Text score_right;
    int points_right = 0;

    float boundary_left = 0;
    float boundary_right = 0;

    bool is_saque = true;   // quien lanza la bola?
    int saca_player = 0;

    public int win_score = 1;

    void ai_player(int player_idx) {
        var pl = players[player_idx];
        // controll Vertical velocity by ball position
        float yDiff = ball.transform.position.y - pl.transform.position.y;
        yDiff = Mathf.Clamp(yDiff, -1, 1);
        pl.attachedRigidbody.velocity = new Vector2(0, yDiff * player_speed);
    }

    void get_closest_player(Vector2 screen_pos, ref int idx, ref int[] directions) {
        //RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(screen_pos);
        //if (Physics.Raycast(ray, out hit)) {
            //Vector2 pos = Camera.main.ScreenToWorldPoint(screen_pos);
         Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector3(screen_pos.x, screen_pos.y, -Camera.main.transform.position.z));
        // if AI, get first player only
        int loop_end = players.Length;
        if (ai_players[1] == true)
            loop_end = 1;

        // find closest player
        float min_dist2 = float.MaxValue;
        for (int i = 0; i < players.Length; ++i)
        {
            Vector2 player_pos = new Vector2(players[i].transform.position.x, players[i].transform.position.y);
            float dist2 = Vector2.SqrMagnitude(pos - player_pos);
            if (dist2 < min_dist2)
            {
                min_dist2 = dist2;
                idx = i;
                directions[idx] = pos.y > player_pos.y ? 1 : (pos.y < player_pos.y ? -1 : 0);
            }
        }
        //}
    }

    void keyboard_controll() {
        // wasd -> left player
        if (Input.GetKey(KeyCode.W)) {
            players[0].attachedRigidbody.velocity = new Vector2(0, player_speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            players[0].attachedRigidbody.velocity = new Vector2(0, -player_speed);
        }
        // arrows -> right player
        if (Input.GetKey(KeyCode.UpArrow))
        {
            players[1].attachedRigidbody.velocity = new Vector2(0, player_speed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            players[1].attachedRigidbody.velocity = new Vector2(0, -player_speed);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        directions = new int[players.Length];
        // give ball a velocity
        ball.attachedRigidbody.velocity = new Vector2(ball_speed, 0);
        // set boundaries
        boundary_left = players[0].transform.position.x - players[0].transform.localScale.x * 2;
        boundary_right = players[1].transform.position.x + players[1].transform.localScale.x * 2;
        // set scores
        score_left.text = points_left.ToString();
        score_right.text = points_right.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        // ai players
        for (int i = 0; i < players.Length; ++i) {
            if(ai_players[i])
                ai_player(i);
        }

        if (is_saque)
        {
            var pl = players[saca_player];
            // attach ball to player
            float offset = pl.transform.position.x > 0 ? -pl.transform.localScale.x : pl.transform.localScale.x;
            ball.attachedRigidbody.position = new Vector2(pl.transform.position.x + offset, pl.transform.position.y);
            // saca la bola
            if ((Input.touchCount > 0 && Input.GetMouseButtonUp(0)) || Input.GetKeyUp(KeyCode.Space)) {
                is_saque = false;
                if (offset > 0)
                    ball.attachedRigidbody.velocity += new Vector2(ball_speed, 0);
                else
                    ball.attachedRigidbody.velocity += new Vector2(-ball_speed, 0);
            }
        }
        // CONTROL PLAYERS, SCORE
        {
            // reduce player speed
            foreach (var pl in players) {
                pl.attachedRigidbody.velocity *= 0.87f;
            }

            // get input touch & MOVE PLAYERS
            if (Input.touchCount > 0)
             {
                
                foreach (Touch t in Input.touches)
                {
                    int current_player_idx = -1;
                    // find closest player
                    get_closest_player(t.position, ref current_player_idx, ref directions);
                    // set velocities
                    players[current_player_idx].attachedRigidbody.velocity = new Vector2(0, player_speed * directions[current_player_idx]);
                }
            }
            else
                keyboard_controll();

            // fixed ball speed on X
            Vector2 vel = ball.attachedRigidbody.velocity;
            if (vel.x > 0)
                ball.attachedRigidbody.velocity = new Vector2(ball_speed, vel.y);
            else
                ball.attachedRigidbody.velocity = new Vector2(-ball_speed, vel.y);
            // detect GOAL -> ball < -width OR ball > width
            if (ball.transform.position.x > boundary_right)
            {
                // left score
                points_left++;
                score_left.text = "Player 1: " + points_left.ToString();
                is_saque = true;
                saca_player = 1;
                if (points_left >= win_score)
                {
                    GetComponent<TransitionLevel>().changeMainMenu();
                }
            }
            if (ball.transform.position.x < boundary_left)
            {
                // right score
                points_right++;
                score_right.text = "Player 2: " + points_right.ToString();
                is_saque = true;
                saca_player = 0;
                if (points_right >= win_score)
                {
                    GetComponent<TransitionLevel>().changeMainMenu();
                }
            }
        }

    }
}
