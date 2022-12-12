using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using UnityEngine;

namespace AnalyticsLogic
{
    public class Analytics : MonoBehaviour
    {
        public static Analytics Instance;

        private Queue<AnalyticData> actions;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            actions = new Queue<AnalyticData>();
        }

        public void Write(string action)
        {
            AnalyticData analyticData = new AnalyticData()
            {
                Action = action,
                Time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };
            actions.Enqueue(analyticData);
        }


        public List<AnalyticData> GetActions()
        {
            return actions.ToList();
        }

        public void ClearQueue()
        {
            actions.Clear();
        }
    }
}