using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMaker : MonoBehaviour
{
    [Serializable]
    public class LevelSettings
    {
        [Serializable]
        public class BrickArragement
        {
            public Vector3 pos;
            public Vector3 rot = Vector3.zero;
            public Vector3 scale = new Vector3(3, 1, 1);

            public Brick.Type type = Brick.Type.clay;
        }

        public List<BrickArragement> brickArragements = new List<BrickArragement>();
    }

    [Serializable]
    public class BrickSettings
    {
        public Brick.Type type;
        public GameObject prefab;
    }


    [Header("LevelSettings")]
    [SerializeField] private List<LevelSettings> levels = new List<LevelSettings>();
    public int levelCount { get { return levels.Count; } }

    
    [Header("BrickSettings")]
    [SerializeField] private List<BrickSettings> bricks = new List<BrickSettings>();

    private Dictionary<Brick.Type, BrickSettings> _brickDict;
    public Dictionary<Brick.Type, BrickSettings> brickDict
    {
        get
        {
            if(_brickDict==null)
            {
                _brickDict = new Dictionary<Brick.Type, BrickSettings>();
                foreach(BrickSettings b in bricks) _brickDict[b.type] = b;
            }
            return _brickDict;
        }
    }




    public List<Brick> InitializeLevel(int level)
    {
        List<Brick> bricksSpawned = new List<Brick>();

        foreach(var brick in levels[level].brickArragements)
        {
            GameObject instance = Instantiate(brickDict[brick.type].prefab, brick.pos, Quaternion.Euler(brick.rot));
            instance.transform.localScale = brick.scale;

            bricksSpawned.Add(instance.GetComponent<Brick>());
        }

        return bricksSpawned;
    }
}
