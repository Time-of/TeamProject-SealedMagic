using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerObject : MonoBehaviour
{
    [Tooltip("플레이어의 최대 체력 변수")]
    public float maxHealth;
    [Tooltip("플레이어의 현재 체력 변수")]
    public float curHealth;
    [Tooltip("플레이어의 최대 마나 변수")]
    public float maxMana;
    [Tooltip("플레이어의 현재 마나 변수")]
    public float curMana;

    [Tooltip("플레이어의 최대 속도")]
    public float maxSpeed;
    [Tooltip("플레이어 기본 속도")]
    [SerializeField] private float runSpeed = 4;
    [Tooltip("점프를 했을떄 위로 올라가는 속도")]
    [SerializeField] private float jumpPower;
    [Tooltip("점프 카운트(더블 점프)")]
    [SerializeField] private float jumpcount = 2;
    [Tooltip("플레이어가 받은 데미지 이미지")]
    public GameObject AttackDamageText;
    [Tooltip("플레이어가 받은 데미지 위치")]
    public Transform hudPos;
    [Tooltip("제단의 위치를 받아오는 곳")]
    public static Vector3 Altarpos;
    [Tooltip("공격시 이동 활성화/비활성")]
    public bool bCanMove = true;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Altar obj;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Jump();
        chage_Move();
        KeyEnter();// 제단 집중
    }

    void FixedUpdate()
    {
        Move();
        Jump_Check();
    }

    // 플레이어가 움직일때
    void Move()
    {
        if (bCanMove)
        {
            if (!Input.GetButtonDown("Attack"))
            {
                // Left and right Move
                float h = Input.GetAxisRaw("Horizontal");
                rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
            }
        }
        else
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.001f, rigid.velocity.y);
        }
        // Max speed
        if (rigid.velocity.x > maxSpeed)// right max Speed
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1))// left max Speed
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            //run
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("isRun", true);
                maxSpeed = 7;
            }
            else
            {
                anim.SetBool("isRun", false);
                maxSpeed = runSpeed;
            }
        }

    }


    // 멈출때 이미지 변경과 좌우반전 이미지 변경
    void chage_Move()
    {
        // Stop speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.001f, rigid.velocity.y);
            anim.SetBool("isRun", false);
        }

        //Direction image chage
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            //spriteRenderer.flipX = true; // 21.05.26 추가
        }
        else if (Input.GetAxis("Horizontal") != 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            //spriteRenderer.flipX = false; // 21.05.26 추가
        }

        // Animation(classic and run)
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
        {
            anim.SetBool("isWalk", false);
        }
        else if (Mathf.Abs(rigid.velocity.x) > 0.001)
        {
            anim.SetBool("isWalk", true);
        }
    }

    // 점프 할때와 속도
    void Jump()
    {
        // Jump
        if (Input.GetButtonDown("Jump") && jumpcount > 0) // spease
        {
            if (jumpcount == 1)// double Jump
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower * 1f);
            }
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isRun", false);// 달리다가 점프 시 꺼짐.
            anim.SetBool("Jumpingdown", false);// 다시 점프를 했을떄, 내려가는 모션이 사라진다.
            anim.SetBool("Jumping", true);// jump image
            jumpcount--;
        }

        // Jump Max speed
        if (rigid.velocity.y > jumpPower)// right max Speed
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower * 0.8f);
        }
    }

    // 점프 중 이미지 와 바닥 체크 밑 카운터 초기화
    void Jump_Check()
    {
        // Jump image chage (Ground Contact)
        if (rigid.velocity.y <= 0)
        {
            anim.SetBool("Jumping", false);// jump UP image
            anim.SetBool("Jumpingdown", true);// jump Down image

            Vector2 frontVec = new Vector2(rigid.position.x + rigid.velocity.x * 0.03f, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1f, LayerMask.GetMask("Platform"));


            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.8f)
                {
                    anim.SetBool("Jumpingdown", false);// jump image
                    jumpcount = 2;
                }
            }
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Spikes")
        {
            OnDamage(10);// 데미지 받아와야 하는데 아직 못함
            OnDamaged(collision.transform.position);
        }
    }

    public void OnDamage(float damage)
    {
        GameObject AttackText = Instantiate(AttackDamageText);// 데미지 이미지 출력
        AttackText.transform.position = hudPos.position;// 데미지 위치
        AttackText.GetComponent<DmgText>().damage = damage;// 데미지 값 받기

        //Damage
        curHealth -= damage;
        OnDamaged(transform.position); // 21.05.26 추가
    }

    // 데미지를 입었을때 팅겨나는 힘
    void OnDamaged(Vector2 targetPos)
    {
        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;// Damaged Direction
        rigid.AddForce(new Vector2(dirc, 1) * 3, ForceMode2D.Impulse);

        //Change Layer (Immortal Active)
        gameObject.layer = 8;

        // Damaged motion coloer
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //Animation
        anim.SetTrigger("isDamaged");

        Invoke("OffDamaged", 2);// 무적 시간
    }

    // 데미지를 입고 무적
    void OffDamaged()
    {
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    /*
    작성: 20181220 이성수(P)

    설명: 제단에 있는 Interact 함수에 접근하게 해주는 컴포넌트
    기획서 상 상호작용 키 (제단 사용 키): F키 (default)
    */

    void KeyEnter()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (Input.GetKeyDown(KeyCode.F) && obj != null && gameManager.killedColoredMonster >= 3 && !obj.isInteracting)
        {
            obj.isInteracting = true;
            Altarpos = transform.position;
            Debug.Log("정신 집중!");
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Altar")
        {
            obj = FindObjectOfType<Altar>();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        obj = null;
    }

}
