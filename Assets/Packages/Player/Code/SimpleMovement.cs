using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float moveSpeed = 5f;         // ����ˮƽ�ƶ����ٶ�
    public float jumpForce = 2f;        // ������Ծ������
    private bool isGrounded = false;     // ��������Ƿ��ڵ�����
    CharacterController characterController;

    [SerializeField] Camera mainCamera;

    private bool isWalk, isLeft, isRight, isBack, isStanding, isJump;

    public GameObject uiPanel; // ��ק UI ��壨��ť���ı��ȣ�������

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private float hight = 0;
    //private new Camera camera;
    void Update()
    {
        // ֻ�е�ǰ��ɫ������ͷ�Ǽ���ģ����ܿ���
        if (Camera.main != mainCamera)
        {
            uiPanel.SetActive(false);
            return;  // �˳�������������
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
        // ��ȡˮƽ�ʹ�ֱ���������
        float horizontal = Input.GetAxis("Horizontal"); // A/D �� ��/�Ҽ�ͷ
        float vertical = Input.GetAxis("Vertical");     // W/S �� ��/�¼�ͷ

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

        // ����Ƿ��ڵ����ϣ�ʹ�� CharacterController �Դ��� `isGrounded`��
        //isGrounded = characterController.isGrounded;
        //Debug.Log("Is Grounded: " + isGrounded);

        // �����Ծ���루���¿ո����
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded || jumpTime != 0)
        {
            isWalk = false;
            isLeft = false;
            isRight = false;
            isBack = false;
            isStanding = false;
            isJump = true;
            jumpTime += Time.deltaTime;
            hight = Mathf.Sqrt(0.001f * jumpForce * jumpTime); // ������Ծ���ٶ�
            if (jumpTime > 1f) 
            {
                jumpTime = 0;
                hight = 0;
                isJump = false;
            }
        }

        // �����ƶ�����
        //Vector3 movement = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;
        //movement.y = hight;
        Vector3 movement = transform.right * horizontal + transform.forward * vertical;
        movement *= moveSpeed * Time.deltaTime;
        movement.y = hight;
        // ���ƶ�����Ӧ�õ�����
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