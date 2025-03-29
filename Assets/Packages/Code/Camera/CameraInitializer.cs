using UnityEngine;

public class CameraInitializer : MonoBehaviour
{
    public string cameraName; // �Զ�������ͷ����
    private CameraManager cameraManager; // ����ͷ����������

    void Start()
    {
        // ��ȡ������Ψһ�� CameraManager ʵ��
        cameraManager = GameObject.FindWithTag("Manager")?.GetComponent<CameraManager>();

        if (cameraManager != null)
        {
            // ע�ᵱǰ����ͷ��������
            cameraManager.RegisterCamera(this.GetComponent<Camera>());
        }
        else
        {
            Debug.LogError("CameraManager not found in the scene.");
        }
    }

    // ����/��������ͷ
    public void Activate(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private bool isInitialize = true;
    private void OnEnable()
    {
        // ������ͷ������ʱִ�е�ָ��
        isInitialize = true;
    }

    private void OnDisable()
    {
        // ������ͷ������ʱִ�е�ָ���ѡ��
        isInitialize = false;
    }

    public bool IsInitialized() 
    {
        bool num = isInitialize;
        isInitialize = false;
        return num;
    }
}
