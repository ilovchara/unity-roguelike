using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    private static Game_Manager _instance;

    public static Game_Manager Instance
    {
        get
        {
            return _instance;
        }
    }

    public int level = 1;
    public int food = 100;
    public AudioClip dieClip;

    //生成敌人对象 - 调用敌人方法
    [HideInInspector]public List<Enemy> enemyList = new List<Enemy>();

    [HideInInspector]public bool isEnd = false; //是否到终点
    private bool sleepStep = true;
    //计数板
    private Text foodText;
    private Text failText;
    private Player player;
    private Map_Manager mapManager;
    private Image dayImage;
    private Text dayText;


    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    void InitGame()
    {
        //初始化地图        
        mapManager = GetComponent<Map_Manager>();
        mapManager.InitMap();
        //初始化UI
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        UpdateFoodText(0);
        failText = GameObject.Find("FialText").GetComponent<Text>();
        failText.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        dayImage = GameObject.Find("DayImage").GetComponent<Image>();
        dayText = GameObject.Find("DayText").GetComponent<Text>();
        dayText.text = "Day " +  level;
        //经过一秒
        Invoke("HideBlack",1);



        //初始化参数 - 新地图需要的东西
        isEnd = false;
        enemyList.Clear();
    }

    void UpdateFoodText(int foodChange)
    {
        if(foodChange == 0)
        {
            foodText.text = "Food:" + food;
        }
        else
        {
            string str = "";
            if (foodChange < 0)
            {
                str = foodChange.ToString();
            }else
            {
                str = "+" + foodChange;
            }
            foodText.text = str + "  Food:" + food;
        }
        
    }

    public void ReduceFood(int count)
    {
        food -= count;
        UpdateFoodText(-count);
        if(food <= 0)
        {
            failText.enabled=true;
            Audio_Manager.Instance.StopBgMusic();
            Audio_Manager.Instance.RandomPlay(dieClip);
        }
    }
    public void AddFood(int count)
    {
        food+=count;
        UpdateFoodText(count);
    }

    public void OnPlayerMove()
    {
        if (sleepStep == true)
        {
            sleepStep = false;
        }
        else
        {
            foreach(var enemy in enemyList)
            {
                enemy.Move();
            }
            sleepStep = true;
        }
        //检测有无到达终点
        if(player.targetPos.x == mapManager.cols - 2 && player.targetPos.y == mapManager.rows - 2)
        {
            isEnd = true;
            //加载下一个关卡
            Application.LoadLevel(Application.loadedLevel); //重新加载本官咖

        }
    }
    void OnLevelWasLoaded(int sceneLevel)
    {
        level++;
        InitGame(); //初始化游戏
    }
    void HideBlack()
    {
        dayImage.gameObject.SetActive(false);
    }
}
