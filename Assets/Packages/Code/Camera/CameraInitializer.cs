using UnityEngine;

public class CameraInitializer : MonoBehaviour
{
    public string cameraName; // 自定义摄像头名称
    private CameraManager cameraManager; // 摄像头管理者引用

    void Start()
    {
        // 获取场景中唯一的 CameraManager 实例
        cameraManager = GameObject.FindWithTag("Manager")?.GetComponent<CameraManager>();

        if (cameraManager != null)
        {
            // 注册当前摄像头到管理者
            cameraManager.RegisterCamera(this.GetComponent<Camera>());
        }
        else
        {
            Debug.LogError("CameraManager not found in the scene.");
        }
    }

    // 激活/禁用摄像头
    public void Activate(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private bool isInitialize = true;
    private void OnEnable()
    {
        // 当摄像头被激活时执行的指令
        isInitialize = true;
    }

    private void OnDisable()
    {
        // 当摄像头被禁用时执行的指令（可选）
        isInitialize = false;
    }

    public bool IsInitialized() 
    {
        bool num = isInitialize;
        isInitialize = false;
        return num;
    }
}
