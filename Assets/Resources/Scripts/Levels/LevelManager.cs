﻿using Sliders.Levels;
using Sliders.Progress;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/*
* This class manages all incoming calls of other game components regarding leveldata modification and saving.
* Levels are serializable and saved to a file.
*/

namespace Sliders.Levels
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager _instance;
        public static LevelChangeEvent onLevelChange = new LevelChangeEvent();

        public class LevelChangeEvent : UnityEvent<Level> { }

        public AudioClip changeLevelSound;
        public Level defaultlevel;
        private Level activeLevel = new Level();

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            Reload();
        }

        public void LoadLevel(int levelID)
        {
        }

        public static Level GetLevel()
        {
            return _instance.activeLevel;
        }

        public static Level GetLevel(int id)
        {
            return LevelLoader.LoadLevel(id);
        }

        public static void Reload()
        {
            int lastID = ProgressManager.GetProgress().GetLastPlayedID();
            Debug.Log("[LevelManager]: Reload() lastID: " + lastID);
            SetLevel(lastID);
        }

        public static int GetID()
        {
            return _instance.activeLevel.id;
        }

        public static int GetDefaultID()
        {
            return _instance.defaultlevel.id;
        }

        public static Vector3 GetSpawnPosition()
        {
            return GetSpawn().GetPosition();
        }

        public static Spawn GetSpawn()
        {
            return _instance.activeLevel.spawn;
        }

        public void NextLevel()
        {
            Debug.Log("[LevelManager]: NextLevel()");
            int nextID = activeLevel.id + 1;
            if (Resources.Load("Prefabs/Levels/" + nextID))
            {
                SetLevel(nextID);
            }
            else
                Debug.Log("[LevelManager]: NextLevel() could not be found.");
        }

        public void LastLevel()
        {
            Debug.Log("[LevelManager]: LastLevel()");
            int nextID = activeLevel.id - 1;
            if (Resources.Load("Prefabs/Levels/" + nextID))
            {
                SetLevel(nextID);
            }
            else
                Debug.Log("[LevelManager]: LastLevel() could not be found.");
        }

        //Try to Place Level with ID newID, destroying all other levels in the scene
        public static void SetLevel(int newID)
        {
            if (LevelLoader.LoadLevel(newID) != null)
            {
                _instance.activeLevel = LevelLoader.LoadLevel(newID);
                _instance.activeLevel = LevelPlacer.Place(GetLevel());
                ProgressManager.GetProgress().SetLastPlayedID(GetID());
                onLevelChange.Invoke(GetLevel());
            }
            else
            {
                Debug.Log("[LevelManager]: SetLevel(int) Level trying to be set does not exist!");
            }
        }

        public static Vector2 GetFinish()
        {
            Vector2 finishlocation = new Vector2();
            return finishlocation;
        }
    }
}