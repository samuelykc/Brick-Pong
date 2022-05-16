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
            public Brick.Type type = Brick.Type.clay;
        }

        public List<BrickArragement> brickArragements = new List<BrickArragement>();
    }

    [SerializeField] private List<LevelSettings> levels = new List<LevelSettings>();

    
    [Serializable]
    public class BrickSettings
    {
        public Brick.Type type;
        public GameObject prefab;
    }
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




    public void InitializeLevel(int level)
    {
        foreach(var brick in levels[level].brickArragements)
        {
            Instantiate(brickDict[brick.type].prefab, brick.pos, Quaternion.identity);
        }
    }
}
