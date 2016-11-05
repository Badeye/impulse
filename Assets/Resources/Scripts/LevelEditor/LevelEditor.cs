﻿using FlipFall.Levels;
using System.Collections;
using System.IO;
using UnityEngine;

namespace FlipFall.Editor
{
    public class LevelEditor : MonoBehaviour
    {
        //look at tileeditor sctipt to get an idea how a level editor could be build up like
        public static LevelEditor _instance;

        // select: select game objects/groups of objects to edit
        // move: move a group of selected objects
        // tool: depending on the selection each selected object can have it's own tool
        //       the tool mode tool can have its own sub-modes
        public enum EditorMode { select, move, tool }

        public static EditorMode editorMode = EditorMode.select;

        // current level getting edited
        public static LevelData editLevel;

        public static Mesh selectedMesh;

        public static bool changesAreSaved;

        private void Start()
        {
            changesAreSaved = true;

            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            _instance = this;

            if (editLevel != null)
                LevelPlacer._instance.PlaceCustom(editLevel);
        }

        // saves the changes made to the currently placed generated level to the .levelData format
        public static void SaveLevel()
        {
            if (editLevel != null)
            {
                LevelDataMono level = LevelPlacer.generatedLevel;
                LevelData l = new LevelData(level.levelData.id);

                // save level info
                l.author = level.levelData.author;
                l.id = level.levelData.id;
                l.presetTime = level.levelData.presetTime;
                l.title = level.levelData.title;
                l.custom = level.levelData.custom;

                // save moveArea mesh
                Vector3[] verts = level.moveArea.meshFilter.mesh.vertices;
                Position2[] posVerts = new Position2[verts.Length];
                for (int i = 0; i < verts.Length; i++)
                {
                    posVerts[i] = new Position2(verts[i].x, verts[i].y);
                }
                l.moveVerticies = posVerts;
                l.moveTriangles = level.moveArea.meshFilter.mesh.triangles;

                // save spawn
                Vector3 spawnPos = level.spawn.transform.localPosition;
                l.spawnPosition = new Position2(spawnPos.x, spawnPos.y);

                print("spawnpos saving: " + spawnPos);

                // save finish
                Vector3 finishPos = level.finish.transform.localPosition;
                l.finishPosition = new Position2(finishPos.x, finishPos.y);
                print("finish saving: " + finishPos);

                // save it
                LevelLoader.SaveCustomLevel(l);
            }
        }

        // loads a level by its id
        public void LoadLevel(string id)
        {
        }
    }
}