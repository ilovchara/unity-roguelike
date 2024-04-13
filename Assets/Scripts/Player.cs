using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    //移动速度
    public float smoothing = 0.5f;
    //休息时间
    public float restTime = 1;
    //计时器
    private float restTimer = 0;

    public AudioClip chop1Audio;
    public AudioClip chop2Audio;
    public AudioClip step1Audio;
    public AudioClip step2Audio;

    public AudioClip suda1Audio;
    public AudioClip suda2Audio;
    public AudioClip food1Audio;
    public AudioClip food2Audio;



    //起始位置
    [HideInInspector]public Vector2 targetPos = new Vector2(1,1);
    
    //命名的刚体和碰撞器
    private Rigidbody2D rigidbody;
    //自身的碰撞器
    private BoxCollider2D collider;
    //动画
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //从unity获取
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //物体运动逻辑
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPos, smoothing * Time.deltaTime));

        if (Game_Manager.Instance.food <= 0 || Game_Manager.Instance.isEnd == true)
        {
            return;
        }
        
        //休息时间间隔计算
        restTimer += Time.deltaTime;
        if (restTimer < restTime) return;

        //控制移动 - 垂直+水平
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        //水平优先
        if (h > 0)
        {
            v = 0;
        }


        if ( h != 0 || v != 0 )
        {
            Game_Manager.Instance.ReduceFood(1);

            //现将自身的碰撞器禁用
            collider.enabled = false;
            //目标位置 - 这个函数？
            RaycastHit2D hit = Physics2D.Linecast(targetPos,targetPos + new Vector2(h,v));
            //获取完成之后再次启用
            collider.enabled = true;

            //通过这个判断碰撞
            if(hit.transform == null)
            {
                targetPos += new Vector2(h, v);
                Audio_Manager.Instance.RandomPlay(step1Audio,step2Audio);
                
            }
            else
            {
                switch (hit.collider.tag)
                {
                    //碰撞墙就不能走了
                    case "OutWall":
                        break;
                    //碰撞障碍物就需要清理
                    case "Wall":
                        //播放触发器动画
                        animator.SetTrigger("Attack");
                        Audio_Manager.Instance.RandomPlay(chop1Audio,chop2Audio);
                        hit.collider.SendMessage("TakeDamage");
                        break;
                    case "Food":
                        Game_Manager.Instance.AddFood(10);
                        targetPos += new Vector2(h, v);
                        Audio_Manager.Instance.RandomPlay(food1Audio, food2Audio);
                        Destroy(hit.transform.gameObject);
                        break;
                    case "Soda":
                        Game_Manager.Instance.AddFood(10);
                        targetPos += new Vector2(h, v);
                        Audio_Manager.Instance.RandomPlay(suda1Audio, suda2Audio);
                        Destroy(hit.transform.gameObject);
                        break;
                    case "Enemy":
                        break;
                }
            }
            Game_Manager.Instance.OnPlayerMove();

            restTimer = 0; //攻击和移动都需要休息
        }

    }

    public void TakeDamage(int lossFood)
    {
        Game_Manager.Instance.ReduceFood(lossFood);
        animator.SetTrigger("Damage");
    }
}
