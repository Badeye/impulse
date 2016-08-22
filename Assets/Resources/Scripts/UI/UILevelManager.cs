﻿using Sliders;
using Sliders.Progress;
using Sliders.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows and hides all levels, gets moved by the
/// </summary>
namespace Sliders.UI
{
    public class UILevelManager : MonoBehaviour
    {
        public static UILevelManager _instance;
        public static int currentPage = 0;

        private static List<Highscore> highscores;
        private static Highscore highscore;
        public Text levelCount;

        // Use this for initialization
        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            UpdateTexts();
        }

        public static void Show()
        {
            //animations fade in / bounce in
            _instance.gameObject.SetActive(true);
            UpdateTexts();
        }

        public static void Hide()
        {
            _instance.gameObject.SetActive(false);
            ProgressManager.SaveProgressData();
        }

        public static void UpdateTexts()
        {
            //edit to only update those visible in the cameraview, not neccessarily all UILevels
            highscores = ProgressManager.GetProgress().highscores;

            foreach (Highscore h in highscores)
            {
                UpdateText(h.levelId, h);
            }
        }

        //fire updated text event and send the changes that were made (1 to 2 stars, new bestTime etc)
        //in the animationManager, check for changes in star amount - update them with animations
        public static void UpdateText(int levelId, Highscore highscore)
        {
            Debug.Log("[UpdateText] of level: " + levelId);
            UILevel uiLevel = _instance.GetUILevel(levelId);

            if (uiLevel == null)
            {
                Debug.LogError("There is no UI Element to display this highscore: " + highscore.levelId);
            }
            else
            {
                string timeString = "--.--";
                double t = highscore.bestTime + 0.01F;

                int secs = (int)t;
                int milSecs = (int)((t - (int)t) * 100);
                //int milSecs = (int)(Mathf.Round(t * 10) / 10);

                Debug.Log("[UpdateText] bestTime: " + t + " milSecs: " + milSecs);
                timeString = string.Format(Constants.timerFormat, secs, milSecs);

                uiLevel.bestText.text = timeString;
            }
        }

        private UILevel GetUILevel(int id)
        {
            UILevel uiLevel = null;
            UILevel[] uiLevels = gameObject.GetComponentsInChildren<UILevel>();
            foreach (UILevel lvl in uiLevels)
            {
                if (lvl.id == id)
                {
                    uiLevel = lvl;
                    break;
                }
            }
            return uiLevel;
        }
    }
}