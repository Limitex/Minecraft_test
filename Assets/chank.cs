using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chank : MonoBehaviour
{
    private Vector3 ChankSize = new Vector3(16, 256, 16);

    public Vector2 PerlinNoiseValue;
    public float SeedValue;

    private GameObject parent;
    private Mesh mesh;
    private int[,,] block;

    void Start()
    {
        block = new int[(int)ChankSize.x + 2, (int)ChankSize.y + 2, (int)ChankSize.z + 2];
        
        for (int y = 0; y < ChankSize.y + 2; y++)
        {
            for (int x = 0; x < ChankSize.x + 2; x++)
            {
                for (int z = 0; z < ChankSize.z + 2; z++)
                {
                    var den = 0.01f;
                    var xC = (x + 1 + PerlinNoiseValue.x) * den + SeedValue;
                    var zC = (z + 1 + PerlinNoiseValue.y) * den + SeedValue;
                    var yC = Mathf.PerlinNoise(xC, zC) * 50f;
                    den = 0.02f;
                    xC = (x + 1 + PerlinNoiseValue.x) * den;
                    zC = (z + 1 + PerlinNoiseValue.y) * den;
                    yC += Mathf.PerlinNoise(xC, zC) * 10f;
                    den = 0.05f;
                    xC = (x + 1 + PerlinNoiseValue.x) * den;
                    zC = (z + 1 + PerlinNoiseValue.y) * den;
                    yC += Mathf.PerlinNoise(xC, zC) * 10f;
                    if (yC < y)
                    {
                        block[x, y, z] = 0;
                    }
                    else
                    {
                        block[x, y, z] = 1;
                    }
                }
            }
        }


        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        int count = 0;
        for (int y = 1; y <= ChankSize.y; y++)
        {
            for (int x = 1; x <= ChankSize.x; x++)
            {
                for (int z = 1; z <= ChankSize.z; z++)
                {
                    if (block[x, y, z] != 0)
                    {
                        int enable = 0;
                        if (block[x, y, z - 1] == 0)
                        {
                            //-Z
                            vertices.Add(new Vector3(0f, 0f, 0f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(0f, 1f, 0f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(1f, 1f, 0f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(1f, 0f, 0f) + new Vector3(x, y, z));

                            uvs.Add(new Vector2(0f, 0f));
                            uvs.Add(new Vector2(0f, 1f));
                            uvs.Add(new Vector2(1f, 1f));
                            uvs.Add(new Vector2(1f, 0f));

                            enable++;
                        }
                        if (block[x + 1, y, z] == 0)
                        {
                            //+X
                            vertices.Add(new Vector3(1f, 0f, 0f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(1f, 1f, 0f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(1f, 1f, 1f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(1f, 0f, 1f) + new Vector3(x, y, z));

                            uvs.Add(new Vector2(0f, 0f));
                            uvs.Add(new Vector2(0f, 1f));
                            uvs.Add(new Vector2(1f, 1f));
                            uvs.Add(new Vector2(1f, 0f));

                            enable++;
                        }
                        if (block[x, y, z + 1] == 0)
                        {
                            //+Z
                            vertices.Add(new Vector3(1f, 0f, 1f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(1f, 1f, 1f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(0f, 1f, 1f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(0f, 0f, 1f) + new Vector3(x, y, z));

                            uvs.Add(new Vector2(0f, 0f));
                            uvs.Add(new Vector2(0f, 1f));
                            uvs.Add(new Vector2(1f, 1f));
                            uvs.Add(new Vector2(1f, 0f));

                            enable++;
                        }
                        if (block[x - 1, y, z] == 0)
                        {
                            //-X
                            vertices.Add(new Vector3(0f, 0f, 1f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(0f, 1f, 1f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(0f, 1f, 0f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(0f, 0f, 0f) + new Vector3(x, y, z));

                            uvs.Add(new Vector2(0f, 0f));
                            uvs.Add(new Vector2(0f, 1f));
                            uvs.Add(new Vector2(1f, 1f));
                            uvs.Add(new Vector2(1f, 0f));

                            enable++;
                        }
                        if (block[x, y + 1, z] == 0)
                        {
                            //+Y
                            vertices.Add(new Vector3(0f, 1f, 0f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(0f, 1f, 1f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(1f, 1f, 1f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(1f, 1f, 0f) + new Vector3(x, y, z));

                            uvs.Add(new Vector2(0f, 0f));
                            uvs.Add(new Vector2(0f, 1f));
                            uvs.Add(new Vector2(1f, 1f));
                            uvs.Add(new Vector2(1f, 0f));

                            enable++;
                        }
                        if (block[x, y - 1, z] == 0)
                        {
                            //-Y
                            vertices.Add(new Vector3(0f, 0f, 0f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(1f, 0f, 0f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(1f, 0f, 1f) + new Vector3(x, y, z));
                            vertices.Add(new Vector3(0f, 0f, 1f) + new Vector3(x, y, z));

                            uvs.Add(new Vector2(0f, 0f));
                            uvs.Add(new Vector2(0f, 1f));
                            uvs.Add(new Vector2(1f, 1f));
                            uvs.Add(new Vector2(1f, 0f));

                            enable++;
                        }
                        for (int i = 0; i < enable; i++)
                        {
                            int a = i * 4;
                            triangles.AddRange(new int[] {
                            0 + a + count, 1 + a + count, 2 + a + count,
                            0 + a + count, 2 + a + count, 3 + a + count
                        });
                        }
                        count += enable * 4;
                    }
                }
            }
        }

        
        GetComponent<MeshRenderer>().material.color = new Color(0, 0.5f, 0);
        mesh.WriteMesh(vertices.ToArray(), uvs.ToArray(), triangles.ToArray());

        var meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        parent = GameObject.Find("map");
        this.transform.parent = parent.transform;
    }

    void Update()
    {
        
    }
}


static class Exe
{
    public static void WriteMesh(this Mesh mesh, Vector3[] vertices, Vector2[] uvs, int[] triangles)
    {
        //　頂点の設定
        mesh.vertices = vertices;
        //　テクスチャのUV座標設定
        mesh.uv = uvs;
        //　三角形メッシュの設定
        mesh.triangles = triangles;
        //　Boundsの再計算
        mesh.RecalculateBounds();
        //　NormalMapの再計算
        mesh.RecalculateNormals();
    }
}


//public Mesh mesh;

//public Vector3 chankSize;



//chankSize = new Vector3(16, 16, 16);

//mesh = GetComponent<MeshFilter>().mesh;
//mesh.Clear();

//int[,,] block = new int[(int)chankSize.x, (int)chankSize.y, (int)chankSize.z];


//List<Vector3> vertices = new List<Vector3>();
//List<Vector2> uvs = new List<Vector2>();
//List<int> triangles = new List<int>();
//for (int x = 0; x < chankSize.x; x++)
//{
//	for (int y = 0; y < chankSize.y; y++)
//	{
//		for (int z = 0; z < chankSize.z; z++)
//		{
//                  if (y < 4)
//                  {
//				block[x, y, z] = 1;
//                  }
//                  else
//                  {
//				block[x, y, z] = 0;
//			}
//		}
//	}
//}
//int c = 0;
//int count = 0;
//      for (int x = 1; x < chankSize.x - 1; x++)
//      {
//	for (int y = 1; y < chankSize.y - 1; y++)
//	{
//		for (int z = 1; z < chankSize.z - 1; z++)
//		{
//			if (block[x, y, z - 1] == 1) 
//			{
//				// 手前
//				vertices.Add(new Vector3(0f, 0f, 0f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(0f, 1f, 0f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(1f, 1f, 0f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(1f, 0f, 0f) + new Vector3(x, y, z));

//				uvs.Add(new Vector2(0f, 0f));
//				uvs.Add(new Vector2(0f, 1f));
//				uvs.Add(new Vector2(1f, 1f));
//				uvs.Add(new Vector2(1f, 0f));
//				count += 4;
//				c += 4;
//			}
//			if (block[x + 1, y, z] == 1)
//			{
//				//右
//				vertices.Add(new Vector3(1f, 0f, 0f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(1f, 1f, 0f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(1f, 1f, 1f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(1f, 0f, 1f) + new Vector3(x, y, z));

//				uvs.Add(new Vector2(0f, 0f));
//				uvs.Add(new Vector2(0f, 1f));
//				uvs.Add(new Vector2(1f, 1f));
//				uvs.Add(new Vector2(1f, 0f));
//				count += 4;
//				c += 4;
//			}
//			if (block[x, y, z + 1] == 1)
//			{
//				//後ろ
//				vertices.Add(new Vector3(1f, 0f, 1f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(1f, 1f, 1f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(0f, 1f, 1f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(0f, 0f, 1f) + new Vector3(x, y, z));

//				uvs.Add(new Vector2(0f, 0f));
//				uvs.Add(new Vector2(0f, 1f));
//				uvs.Add(new Vector2(1f, 1f));
//				uvs.Add(new Vector2(1f, 0f));
//				count += 4;
//				c += 4;
//			}
//			if (block[x - 1, y, z] == 1)
//			{
//				//左
//				vertices.Add(new Vector3(0f, 0f, 1f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(0f, 1f, 1f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(0f, 1f, 0f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(0f, 0f, 0f) + new Vector3(x, y, z));

//				uvs.Add(new Vector2(0f, 0f));
//				uvs.Add(new Vector2(0f, 1f));
//				uvs.Add(new Vector2(1f, 1f));
//				uvs.Add(new Vector2(1f, 0f));
//				count += 4;
//				c += 4;
//			}
//			if (block[x, y + 1, z] == 1)
//			{
//				//上
//				vertices.Add(new Vector3(0f, 1f, 0f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(0f, 1f, 1f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(1f, 1f, 1f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(1f, 1f, 0f) + new Vector3(x, y, z));

//				uvs.Add(new Vector2(0f, 0f));
//				uvs.Add(new Vector2(0f, 1f));
//				uvs.Add(new Vector2(1f, 1f));
//				uvs.Add(new Vector2(1f, 0f));
//				count += 4;
//				c += 4;
//			}
//			if (block[x, y - 1, z] == 1)
//			{
//				//した
//				vertices.Add(new Vector3(0f, 0f, 0f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(1f, 0f, 0f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(1f, 0f, 1f) + new Vector3(x, y, z));
//				vertices.Add(new Vector3(0f, 0f, 1f) + new Vector3(x, y, z));

//				uvs.Add(new Vector2(0f, 0f));
//				uvs.Add(new Vector2(0f, 1f));
//				uvs.Add(new Vector2(1f, 1f));
//				uvs.Add(new Vector2(1f, 0f));
//				count += 4;
//				c += 4;
//			}
//                  for (int i = 0; i <= count; i+=4)
//                  {
//				triangles.AddRange(new int[] { 0 + i + c, 1 + i + c, 2 + i + c, 0 + i + c, 2 + i + c, 3 + i + c });
//			}
//			count = 0;
//		}
//	}
//}
//GetComponent<MeshRenderer>().material.color = new Color(0.4f, 0.4f, 0.9f);
//mesh.WriteMesh(vertices.ToArray(), uvs.ToArray(), triangles.ToArray());
