using System.Collections.Generic;
using Data.Models;
using TMPro;
using UnityEngine;

namespace Leaderboards
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Leader leaderPrefab;
        [SerializeField] private Transform leaderContainer;

        public void Init(string title, List<LeaderData> leaders)
        {
            titleText.text = title;
            
            for (int i = 0; i < leaders.Count; i++)
            {
                var leader = Instantiate(leaderPrefab, leaderContainer);
                leader.Init(i+1, leaders[i].Login, leaders[i].Record);
            }
            
            for (int i = leaders.Count; i < 5; i++)
            {
                var leader = Instantiate(leaderPrefab, leaderContainer);
                leader.Init(i+1, "---", 0);
            }
        }
    }
}