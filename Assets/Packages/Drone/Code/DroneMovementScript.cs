using UnityEngine;

public class DroneMovementScript : MonoBehaviour
{
    private Rigidbody ourDrone;
    private float upForce = 0;

    public GameObject uiPanel; // 拖拽 UI 面板（或按钮、文本等）到这里

    void Awake()
    {
        ourDrone = GetComponent<Rigidbody>();
        camera = ourDrone.transform.GetChild(0).GetChild(0).GetComponent<Camera>();
    }

    void Update()
    {
        // 只有当前角色的摄像头是激活的，才能控制
        if (Camera.main != camera)
        {
            if (Mathf.Abs(ourDrone.linearVelocity.x) < 0.1f && Mathf.Abs(ourDrone.linearVelocity.z) < 0.1f && upForce == 0)
            {
                ourDrone.linearVelocity = Vector3.zero;         // 清除速度
                ourDrone.angularVelocity = Vector3.zero; // 清除角速度

                ourDrone.constraints = RigidbodyConstraints.FreezePositionY;
            }
            uiPanel.SetActive(false);
            return;  // 退出，不接收输入
        }
        uiPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        MovementUpDown(0);
        MovementForward(0);
        MovementRight(0);
        StopMovement();
        Rotation();
        ClampingSpeedValues();
        SwitchCammera();

        if (Mathf.Abs(tiltAmountForward) < 0.1f)
        {
            tiltAmountForward = 0;
        }
        if (Mathf.Abs(tiltAmountRight) < 0.1f)
        {
            tiltAmountRight = 0f;
        }
        ourDrone.rotation = Quaternion.Euler(new Vector3(tiltAmountForward, currentYRotation, tiltAmountRight));
        if (Mathf.Abs(ourDrone.linearVelocity.x) < 0.1f && Mathf.Abs(ourDrone.linearVelocity.z) < 0.1f && upForce == 0)
        {
            ourDrone.linearVelocity = Vector3.zero;         // 清除速度
            ourDrone.angularVelocity = Vector3.zero; // 清除角速度
                                                     //ourDrone.constraints = RigidbodyConstraints.None;

            //ourDrone.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }

    void MovementUpDown(float power)
    {
        upForce = ourDrone.mass * Mathf.Abs(Physics.gravity.y) - ourDrone.linearVelocity.y * ourDrone.mass;
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.K))
            {
                ourDrone.linearVelocity = ourDrone.linearVelocity;
            }
            if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && !Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.L))
            {
                ourDrone.linearVelocity = new Vector3(ourDrone.linearVelocity.x, Mathf.Lerp(ourDrone.linearVelocity.y, 0, Time.deltaTime * 5), ourDrone.linearVelocity.z);
                upForce += 25;
                if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
                {
                    upForce += 40;
                }
            }
            if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L)))
            {
                ourDrone.linearVelocity = new Vector3(ourDrone.linearVelocity.x, Mathf.Lerp(ourDrone.linearVelocity.y, 0, Time.deltaTime * 5), ourDrone.linearVelocity.z);
                upForce += 25;
                if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
                {
                    upForce += 40;
                }
            }
        }

        if (Input.GetKey(KeyCode.I) || power > 0)
        {
            upForce = 450;
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
            {
                upForce = 500;
            }
        }
        else if (Input.GetKey(KeyCode.K) || power < 0)
        {
            upForce = -200;
        }
        else if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            upForce = 0;
        }
        if (power != 0)
        {
            ourDrone.AddRelativeForce(Vector3.up * upForce * 0.2f);
        }
        else 
        {
            ourDrone.AddRelativeForce(Vector3.up * upForce);
        }
    }

    private float movementForwardSpeed = 100.0f;
    private float tiltAmountForward = 0;
    private float tiltVelocityForward;
    void MovementForward(float power)
    {
        if (Input.GetAxis("Vertical") != 0 || power != 0)
        {
            if (Input.GetAxis("Vertical") != 0)
            {
                ourDrone.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * movementForwardSpeed);
            }
            else 
            {
                if (Mathf.Abs(power) < 0.5f) 
                {
                    power *= 2;
                }
                ourDrone.AddRelativeForce(Vector3.forward * power * movementForwardSpeed);
            }
            tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 20 * Input.GetAxis("Vertical"), ref tiltVelocityForward, 0.1f);
        }
        else 
        {
            tiltAmountForward = ourDrone.transform.rotation.x;
            tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 0, ref tiltVelocityForward, 0.1f);
        }
    }


    private float tiltAmountRight = 0;
    private float tiltVelocityRight;
    void MovementRight(float power) 
    {
        if (Input.GetAxis("Horizontal") != 0 || power != 0)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                ourDrone.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * movementForwardSpeed);
            }
            else 
            {
                if (Mathf.Abs(power) < 0.5f)
                {
                    power *= 2;
                }
                ourDrone.AddRelativeForce(Vector3.right * power * movementForwardSpeed);
            }
            tiltAmountRight = Mathf.SmoothDamp(tiltAmountRight, -20 * Input.GetAxis("Horizontal"), ref tiltVelocityRight, 0.1f);
        }
        else
        {
            tiltAmountRight = ourDrone.transform.rotation.z;
            tiltAmountRight = Mathf.SmoothDamp(tiltAmountRight, 0, ref tiltVelocityRight, 0.1f);
        }
    }

    private float wantedYRotation;
    [HideInInspector] public float currentYRotation;
    private float rotateAmoutByKay = 2.5f;
    private float rotationYVelocity;
    void Rotation()
    {
        if (Input.GetKey(KeyCode.J))
        {
            wantedYRotation -= rotateAmoutByKay;
        }
        if (Input.GetKey(KeyCode.L))
        {
            wantedYRotation += rotateAmoutByKay;
        }

        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
    }

    void StopMovement() 
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            ourDrone.linearVelocity = Vector3.zero;   // 清除速度
            ourDrone.angularVelocity = Vector3.zero; // 清除角速度
            ourDrone.AddForce(3f * Vector3.up, ForceMode.Impulse);
        }
    }

    private Vector3 velocityToSmoothDampToZero;
    void ClampingSpeedValues()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            ourDrone.linearVelocity = Vector3.ClampMagnitude(ourDrone.linearVelocity, Mathf.Lerp(ourDrone.linearVelocity.magnitude, 5.0f, Time.deltaTime * 5f));
        }
        else if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            ourDrone.linearVelocity = Vector3.ClampMagnitude(ourDrone.linearVelocity, Mathf.Lerp(ourDrone.linearVelocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        else if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            ourDrone.linearVelocity = Vector3.ClampMagnitude(ourDrone.linearVelocity, Mathf.Lerp(ourDrone.linearVelocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        else if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            ourDrone.linearVelocity = Vector3.SmoothDamp(ourDrone.linearVelocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.95f);
        }
    }

    private new Camera camera;
    private bool isInitialize = true;
    void SwitchCammera() 
    {
        // 检查摄像头的 GameObject 是否被激活
        if (camera != null && !camera.gameObject.activeSelf)
        {
            ourDrone.constraints = RigidbodyConstraints.FreezePositionY;
        }
        isInitialize = camera.GetComponent<CameraInitializer>().IsInitialized();
        if (isInitialize)
        {
            ourDrone.linearVelocity = Vector3.zero;  // 清除速度
            ourDrone.angularVelocity = Vector3.zero; // 清除角速度
            ourDrone.constraints = RigidbodyConstraints.None;
        }
    }

    public void TraveToPoint(Vector3 target) 
    {
        // 计算与目标的方向和距离
        Vector3 direction = target - camera.transform.position;
        float distance = direction.magnitude;

        // 如果接近目标，停止施加力
        if (distance < 1f) return;

        // 归一化方向
        direction.Normalize();

        // 动态调整力的大小（越近力越小）
        float force = Mathf.Lerp(0, 10f, distance / 1f);

        // 施加力
        ourDrone.AddForce(direction * force, ForceMode.Acceleration);
    }

    //private void JumpToPoint(Vector3 target) 
    //{
        // 计算方向
        //**Vector3 direction = (target - camera.transform.position).normalized;
        //Debug.Log(camera.transform.position + "==" + target);
        // 使用力推动物体移动
        //**if (Mathf.Abs(camera.transform.position.y - target.y) > 10f)
        //**{
        //**if ((ourDrone.constraints & RigidbodyConstraints.FreezePositionY) != 0)
        //**{
        //**ourDrone.constraints = RigidbodyConstraints.None;
        //**}
        //**}
        //**else
        //**{
        //**if ((ourDrone.constraints & RigidbodyConstraints.FreezePositionY) == 0)
        //**{
        //**ourDrone.constraints = RigidbodyConstraints.FreezePositionY;
        //**}
        //**}
        //**Vector3 postion = transform.position + direction * 50f * Time.fixedDeltaTime;
        //postion.y = (transform.position.y - camera.transform.position.y) + target.y;

        //**ourDrone.MovePosition(postion);
    //}
}