using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{
    //��Ϸ����Ʒ
    public GameObject[] outWallArray;
    public GameObject[] floorArray;
    public GameObject[] wallArray;
    public GameObject[] foodArray;
    public GameObject[] enemyArray;
    public GameObject exitPrefab;

    //��ͼ�ķ�Χ
    public int rows = 10;
    public int cols = 10;

    //�����ϰ������С - ���
    public int minCountWall = 2;
    public int maxCountWall = 8;

    //�洢λ�õ���Ϣ
    private Transform mapHolder;
    //�洢���ɵ���Ʒ
    private List<Vector2> positionList = new List<Vector2>();

    //�ؿ�����
    private Game_Manager gameManager;





    // Update is called once per frame
    void Update()
    {
        
    }

    //��ʼ����ͼ
    public void InitMap()
    {
        gameManager = this.GetComponent<Game_Manager>();

        //����һ�������� - ���ڳе�������ͼ
        mapHolder = new GameObject("Map").transform;
        //������ǽ 10*10
        for(int x = 0; x < cols; x++)
        {
            for(int y = 0; y < rows; y++)
            {
                //����ڱ߽���������ǵ���ǽ
                if(x == 0 || y == 0 || x == cols-1 || y == rows - 1)
                {
                    //������������ǽ�������ҵ�һ��ǽ����ȥ
                    int index = Random.Range(0, outWallArray.Length);
                    //�����Ӷ�������ά����ǽ
                    GameObject go0 =  GameObject.Instantiate(outWallArray[index],new Vector3(x,y,0),Quaternion.identity);
                    //���Ӷ����븸�������
                    go0.transform.SetParent(mapHolder);
                }
                //���ڱ߽���������ǵĵذ�
                else
                {
                    //������ذ�
                    int index = Random.Range(0, outWallArray.Length);
                    //�����Ӷ���ά���ذ�
                    GameObject go0 =  GameObject.Instantiate(floorArray[index], new Vector3(x, y, 0), Quaternion.identity);
                    //�нӵ����ǵĸ�������
                    go0.transform.SetParent(mapHolder);
                }
            }
        }

        //��Ϊ������ɵ�ͼ֮���������ֵ - ��������
        positionList.Clear();
        //������Ʒ
        for(int x = 2; x < cols - 2; x++)
        {
            for(int y = 2; y < rows - 2; y++)
            {
                //������һ��λ��
                positionList.Add(new Vector2(x, y));
            }
        }

        //�����������

        //��������ϰ���
        int wallCount = Random.Range(minCountWall, maxCountWall+1);
        InstantiateItems(wallCount, wallArray);

        //�������ʳ�� - ���չؿ��ı������
        int foodCount = Random.Range(2, gameManager.level * 2 + 1);
        InstantiateItems(foodCount, foodArray);

        //�������� - level/2 - ������������/2
        int enemyCount = gameManager.level/2;
        InstantiateItems(enemyCount,enemyArray);
        //��������
        GameObject go4 =  Instantiate(exitPrefab, new Vector2(cols - 2, rows - 2), Quaternion.identity) as GameObject;
        go4.transform.SetParent(mapHolder);


    }


    //������Ʒ���� - ����ĵڶ�����������Ʒ��Ӧ������
    private void InstantiateItems(int count, GameObject[] prefabs)
    {
        for (int i = 0; i < count; i++)
        {
            //���ȡ��λ��
            Vector2 pos = RandomPosition();
            //�������Ʒ������ȡ��һ�����������ڵ�ͼ��
            GameObject enemyPrefab = RandomPrefab(prefabs);
            //Instantiate ���������������ڳ�����ʵ������������һ���µ���Ϸ����
            //as ����ִ������ת�������Ϊǿ������ת����
            GameObject go = Instantiate(enemyPrefab, pos, Quaternion.identity) as GameObject;
            go.transform.SetParent(mapHolder);
        }


    }

    //����ڵ�ͼȡ��һ��λ��
    private Vector2 RandomPosition()
    {
        int positionIndex = Random.Range(0, positionList.Count);
        Vector2 pos = positionList[positionIndex];
        positionList.RemoveAt(positionIndex);
        return pos;
    }
    //���ѡ��һ�����壬��������
    private GameObject RandomPrefab(GameObject[] prefabs)
    {
        int index = Random.Range(0, prefabs.Length);
        return prefabs[index];
    }

}
