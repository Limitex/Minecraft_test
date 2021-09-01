using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map : MonoBehaviour
{
    public GameObject chank;
    public GameObject player;

    private static int DispChankSize = 16;
    private static float Seed = new System.Random().Next(-10000, 10000) / 1000f;
    
    private Dictionary<Vector2, GameObject> DispChankData = new Dictionary<Vector2, GameObject>();
    private Vector2[,] DispChankArea = new Vector2[DispChankSize, DispChankSize];

    private float GenerationTime = 0;

    void Start()
    {
        player.transform.position = ChankToPosition(new Vector2(DispChankSize, DispChankSize) / 2, 100);

        for (int x = 0; x < DispChankSize; x++)
        {
            for (int z = 0; z < DispChankSize; z++)
            {
                DispChankData.Add(new Vector2(x, z), ChankBuild(new Vector2(x, z)));
                DispChankArea[x, z] = new Vector2(x, z);
            }
        }
    }

    void Update()
    {
        var pos = PositionToChank(player.transform.position);

        GenerationTime += Time.deltaTime;

        for (int x = 0; x < DispChankSize; x++)
        {
            for (int y = 0; y < DispChankSize; y++)
            {
                DispChankArea[x, y] = new Vector2(
                    (int)(pos.x - (DispChankSize / 2)) + x,
                    (int)(pos.y - (DispChankSize / 2)) + y);
            }
        }

        if (GenerationTime > 0.02f)
        {
            //マップの生成のところ
            //ToDo プレイヤー中心にぐるぐる生成するようにしたい。
            var generateFlug = false;
            for (int x = 0; x < DispChankSize; x++)
            {
                for (int y = 0; y < DispChankSize; y++)
                {
                    if (!DispChankData.ContainsKey(DispChankArea[x, y]))
                    {
                        DispChankData.Add(DispChankArea[x, y], ChankBuild(DispChankArea[x, y]));
                        generateFlug = true;
                        break;
                    }
                }
                if (generateFlug) break;
            }
            //生成終わり
            GenerationTime = 0;
        }

        foreach (var keys in DispChankData.Keys)
        {
            if (!CheckChank(DispChankArea, keys))
            {
                Destroy(DispChankData[keys]);
                DispChankData.Remove(keys);
                break;
            }
        }
    }

    bool CheckChank(Vector2[,] range, Vector2 target)
    {
        var xmax = range.GetLength(0) - 1;
        var ymax = range.GetLength(1) - 1;
        bool x = range[0, 0].x <= target.x && target.x <= range[xmax, ymax].x;
        bool y = range[0, 0].y <= target.y && target.y <= range[xmax, ymax].y;
        return x && y;
    }

    Vector3 ChankToPosition(Vector2 chankpos, float y)
    {
        return new Vector3(chankpos.x * 16f, y, chankpos.y * 16f);
    }

    Vector2 PositionToChank(Vector3 position)
    {
        var x = Mathf.Round(position.x / 16f);
        var y = Mathf.Round(position.z / 16f);
        return new Vector2(x, y);
    }
    
    GameObject ChankBuild(Vector2 position)
    {
        var ch = Instantiate(chank);
        ch.GetComponent<chank>().PerlinNoiseValue = new Vector2(position.x * 16, position.y * 16);
        ch.GetComponent<chank>().SeedValue = Seed;
        ch.transform.position = new Vector3(position.x * 16, 0, position.y * 16);
        return ch;
    }
}
