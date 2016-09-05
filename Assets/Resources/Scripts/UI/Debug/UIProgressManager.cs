﻿using Impulse;
using Impulse.Progress;
using UnityEngine;
using UnityEngine.UI;

namespace Impulse.UI
{
    public class UIProgressManager : MonoBehaviour
    {
        public void Load()
        {
            ProgressManager.LoadProgressData();
        }

        public void Save()
        {
            ProgressManager.SaveProgressData();
        }

        public void Clear()
        {
            //ProgressManager.ClearScores();
        }

        public void CreateScoreboard()
        {
        }
    }
}