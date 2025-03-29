using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float moveSpeed = 5f;         // 控制水平移动的速度
    public float jumpForce = 2f;        // 控制跳跃的力度
    private bool isGrounded = false;     // 检测物体是否在地面上
    CharacterController characterController;

    [SerializeField] Camera mainCamera;

    private bool isWalk, isLeft, isRight, isBack, isStanding, isJump;

    public GameObject uiPanel; // 拖拽 UI 面板（或按钮、文本等）到这里

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private float hight = 0;
    //private new Camera camera;
    void Update()
    {
        // 只有当前角色的摄像头是激活的，才能控制
        if (Camera.main != mainCamera)
        {
            uiPanel.SetActive(false);
            return;  // 退出，不接收输入
        }
        uiPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Movement(hight);
        ActionStatus();
        MoveEyes();
    }

    private Vector2 turn;
    private void MoveEyes() 
    {
        //Cursor.lockState = CursorLockMode.Locked;
        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");
        if (turn.y > 60) 
        {
            turn.y = 60;
        }
        if (turn.y < -90) 
        {
            turn.y = -90;
        }
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
        //Cursor.visible = false;
    }

    private float jumpTime = 0f;
    private void Movement(float hight) 
    {
        // 获取水平和垂直方向的输入
        float horizontal = Input.GetAxis("Horizontal"); // A/D 或 左/右箭头
        float vertical = Input.GetAxis("Vertical");     // W/S 或 上/下箭头

        if (vertical > 0)
        {
            if (horizontal < 0)
            {
                isWalk = false;
                isRight = false;
                isBack = false;
                isLeft = true;
                isStanding = false;
            }
            else if (horizontal > 0)
            {
                isWalk = false;
                isLeft = false;
                isBack = false;
                isRight = true;
                isStanding = false;
            }
            else if (horizontal == 0)
            {
                isLeft = false;
                isRight = false;
                isBack = false;
                isWalk = true;
                isStanding = false;
            }
        }
        else if (vertical < 0)
        {
            isWalk = false;
            isLeft = false;
            isRight = false;
            isBack = true;
            isStanding = false;
        }
        else if (vertical == 0)
        {
            if (horizontal < 0)
            {
                isWalk = false;
                isRight = false;
                isBack = false;
                isLeft = true;
                isStanding = false;
            }
            else if (horizontal > 0)
            {
                isWalk = false;
                isLeft = false;
                isBack = false;
                isRight = true;
                isStanding = false;
            }
            else if( horizontal == 0)
            {
                isWalk = false;
                isLeft = false;
                isRight = false;
                isBack = false;
                isStanding = true;
            }
        }

        // 检测是否在地面上（使用 CharacterController 自带的 `isGrounded`）
        //isGrounded = characterController.isGrounded;
        //Debug.Log("Is Grounded: " + isGrounded);

        // 检测跳跃输入（按下空格键）
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded || jumpTime != 0)
        {
            isWalk = false;
            isLeft = false;
            isRight = false;
            isBack = false;
            isStanding = false;
            isJump = true;
            jumpTime += Time.deltaTime;
            hight = Mathf.Sqrt(0.001f * jumpForce * jumpTime); // 计算跳跃初速度
            if (jumpTime > 1f) 
            {
                jumpTime = 0;
                hight = 0;
                isJump = false;
            }
        }

        // 计算移动方向
        //Vector3 movement = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;
        //movement.y = hight;
        Vector3 movement = transform.right * horizontal + transform.forward * vertical;
        movement *= moveSpeed * Time.deltaTime;
        movement.y = hight;
        // 将移动方向应用到物体
        characterController.Move(movement);
    }

    private void ActionStatus()
    {
        GetComponent<Animator>().SetBool("isWalk", isWalk);
        GetComponent<Animator>().SetBool("isLeft", isLeft);
        GetComponent<Animator>().SetBool("isRight", isRight);
        GetComponent<Animator>().SetBool("isBack", isBack);
        GetComponent<Animator>().SetBool("isJump", isJump);
        GetComponent<Animator>().SetBool("isStanding", isStanding);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}