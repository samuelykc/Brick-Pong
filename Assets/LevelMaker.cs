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

        [Serializable]
        public class PowerUpArragement
        {
            public int attachedBrick;   //index of brick in brickArragements, -1 if random
            public PowerUp.Type type;
        }

        public List<BrickArragement> brickArragements = new List<BrickArragement>();
        public List<PowerUpArragement> powerUpArragements = new List<PowerUpArragement>();
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
        List<Brick> bricksSpawned_breakable = new List<Brick>();

        //spawn bricks
        foreach (var brick in levels[level].brickArragements)
        {
            GameObject instance = Instantiate(brickDict[brick.type].prefab, brick.pos, Quaternion.Euler(brick.rot));
            instance.transform.localScale = brick.scale;

            Brick brickScript = instance.GetComponent<Brick>();
            bricksSpawned.Add(brickScript);
            if(brickScript.isBreakable()) bricksSpawned_breakable.Add(brickScript);
        }

        //add fixed power ups
        foreach(var powerUp in levels[level].powerUpArragements)
        {
            if(powerUp.attachedBrick>=0 && powerUp.attachedBrick<bricksSpawned_breakable.Count)
            {
                Brick targetBrick = bricksSpawned_breakable[powerUp.attachedBrick];
                targetBrick.containingPowerUps.Add(powerUp.type);

                bricksSpawned_breakable.Remove(targetBrick);
            }
        }

        //add random power ups
        foreach(var powerUp in levels[level].powerUpArragements)
        {
            if(powerUp.attachedBrick==-1 && bricksSpawned_breakable.Count>0)
            {
                Brick targetBrick = bricksSpawned_breakable[UnityEngine.Random.Range(0, bricksSpawned_breakable.Count - 1)];    //target random brick may not contain any other power up beforehand
                targetBrick.containingPowerUps.Add(powerUp.type);

                bricksSpawned_breakable.Remove(targetBrick);
            }
        }

        return bricksSpawned;
    }
}
