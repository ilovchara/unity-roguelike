using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{
    //游戏内物品
    public GameObject[] outWallArray;
    public GameObject[] floorArray;
    public GameObject[] wallArray;
    public GameObject[] foodArray;
    public GameObject[] enemyArray;
    public GameObject exitPrefab;

    //地图的范围
    public int rows = 10;
    public int cols = 10;

    //生成障碍物的最小 - 最大
    public int minCountWall = 2;
    public int maxCountWall = 8;

    //存储位置的信息
    private Transform mapHolder;
    //存储生成的物品
    private List<Vector2> positionList = new List<Vector2>();

    //关卡变量
    private Game_Manager gameManager;





    // Update is called once per frame
    void Update()
    {
        
    }

    //初始化地图
    public void InitMap()
    {
        gameManager = this.GetComponent<Game_Manager>();

        //创建一个父对象 - 用于承担整个地图
        mapHolder = new GameObject("Map").transform;
        //创建外墙 10*10
        for(int x = 0; x < cols; x++)
        {
            for(int y = 0; y < rows; y++)
            {
                //如果在边界就生成我们的外墙
                if(x == 0 || y == 0 || x == cols-1 || y == rows - 1)
                {
                    //随机在外面的外墙数组中找到一个墙按上去
                    int index = Random.Range(0, outWallArray.Length);
                    //创建子对象用于维护外墙
                    GameObject go0 =  GameObject.Instantiate(outWallArray[index],new Vector3(x,y,0),Quaternion.identity);
                    //将子对象与父对象关联
                    go0.transform.SetParent(mapHolder);
                }
                //不在边界就生成我们的地板
                else
                {
                    //随机安地板
                    int index = Random.Range(0, outWallArray.Length);
                    //创建子对象维护地板
                    GameObject go0 =  GameObject.Instantiate(floorArray[index], new Vector3(x, y, 0), Quaternion.identity);
                    //承接到我们的父对象上
                    go0.transform.SetParent(mapHolder);
                }
            }
        }

        //因为创建完成地图之后，数组存在值 - 复用清零
        positionList.Clear();
        //生成物品
        for(int x = 2; x < cols - 2; x++)
        {
            for(int y = 2; y < rows - 2; y++)
            {
                //随机添加一个位置
                positionList.Add(new Vector2(x, y));
            }
        }

        //下面的升级了

        //随机生成障碍物
        int wallCount = Random.Range(minCountWall, maxCountWall+1);
        InstantiateItems(wallCount, wallArray);

        //随机生成食物 - 按照关卡的标号增加
        int foodCount = Random.Range(2, gameManager.level * 2 + 1);
        InstantiateItems(foodCount, foodArray);

        //创建敌人 - level/2 - 生成人数敌人/2
        int enemyCount = gameManager.level/2;
        InstantiateItems(enemyCount,enemyArray);
        //创建出口
        GameObject go4 =  Instantiate(exitPrefab, new Vector2(cols - 2, rows - 2), Quaternion.identity) as GameObject;
        go4.transform.SetParent(mapHolder);


    }


    //生成物品函数 - 引入的第二个变量是物品对应的数组
    private void InstantiateItems(int count, GameObject[] prefabs)
    {
        for (int i = 0; i < count; i++)
        {
            //随机取得位置
            Vector2 pos = RandomPosition();
            //随机从物品数组中取得一个变量，按在地图上
            GameObject enemyPrefab = RandomPrefab(prefabs);
            //Instantiate 函数，其作用是在场景中实例化（创建）一个新的游戏对象
            //as 用于执行类型转换（或称为强制类型转换）
            GameObject go = Instantiate(enemyPrefab, pos, Quaternion.identity) as GameObject;
            go.transform.SetParent(mapHolder);
        }


    }

    //随机在地图取得一个位置
    private Vector2 RandomPosition()
    {
        int positionIndex = Random.Range(0, positionList.Count);
        Vector2 pos = positionList[positionIndex];
        positionList.RemoveAt(positionIndex);
        return pos;
    }
    //随机选择一个物体，在数组中
    private GameObject RandomPrefab(GameObject[] prefabs)
    {
        int index = Random.Range(0, prefabs.Length);
        return prefabs[index];
    }

}
