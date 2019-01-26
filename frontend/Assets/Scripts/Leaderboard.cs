﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

struct LeaderboardItem
{
    public Color color;
    public int numBlocks;
}

public class Leaderboard : MonoBehaviour
{
    LeaderboardItem[] items = new LeaderboardItem[5];
    public Text text;
    public Image[] colors;

    // Start is called before the first frame update
    void Start()
    {
        Api.On("leaderboard", this.OnUpdateLeaderboard);
        UpdateText();
    }

    // Update is called once per frame
    void OnUpdateLeaderboard(SocketIOEvent e)
    {
        int position = -1;
        e.data.GetField(ref position, "position");

        if (position == -1) return;

        LeaderboardItem current = this.items[position];

        e.data.GetField(ref current.color.r, "r");
        e.data.GetField(ref current.color.g, "g");
        e.data.GetField(ref current.color.b, "b");
        current.color.a = 1;

        e.data.GetField(ref current.numBlocks, "numBlocks");

        this.items[position] = current;

        this.UpdateText();
    }

    private void UpdateText()
    {
        this.text.text = "Leaderboard:\n";

        int i = 0;
        foreach (LeaderboardItem item in this.items)
        {
            if (item.numBlocks > 0)
            {
                this.text.text += item.numBlocks + "\n";
                this.colors[i].color = item.color;
            }
            else
            {
                this.colors[i].color = new Color(0, 0, 0, 0);
            }
            i++;
        }
    }
}
