using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private List<Camera> cameras = new List<Camera>();
    private int activeCameraIndex = 0;

    // ��̬�ֶΣ�����Ψһ��ʵ��
    public static CameraManager Instance { get; private set; }

    void Awake()
    {
        // ����Ѿ���һ��ʵ�����ڣ����ٵ�ǰ����ȷ��ֻ��һ�� CameraManager ʵ��
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ���ٵ�ǰ��ʵ��
            return;
        }

        // ��������Ϊ��ǰʵ��
        Instance = this;

        // ȷ���ڳ����л�ʱ�ö��󲻱�����
        DontDestroyOnLoad(gameObject);


    }

    void Start()
    {
        // ���Ҵ����ض���ǩ�����ж���
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("MainCamera");

        foreach (GameObject obj in taggedObjects)
        {
            Camera cam = obj.GetComponent<Camera>();
            if (cam != null)
            {
                RegisterCamera(cam);
            }
        }
        if (cameras.Count > 0)
        {
            ActivateCamera(activeCameraIndex); // �����һ������ͷ
        }
        //else
        //{
        //    Debug.LogWarning("No cameras found in the scene.");
        //}
    }


    // ע������ͷ
    public void RegisterCamera(Camera cam)
    {
        if (!cameras.Contains(cam))
        {
            cameras.Add(cam);
            //Debug.Log($"Camera {cam.name} registered.");
        }
    }

    // ����ָ������ͷ
    public void ActivateCamera(int index)
    {
        if (index < 0 || index >= cameras.Count)
        {
            Debug.LogError("Invalid camera index.");
            return;
        }

        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].gameObject.SetActive(i == index); // ����Ŀ������ͷ��������������ͷ
        }

        activeCameraIndex = index;
        //Debug.Log($"Activated camera: {cameras[index].name}");
    }

    void Update()
    {
        // ���� Q ���л�����һ������ͷ
        if (Input.GetKeyDown(KeyCode.Q))
        {
            activeCameraIndex = (activeCameraIndex - 1 + cameras.Count) % cameras.Count;
            ActivateCamera(activeCameraIndex);
        }

        // ���� E ���л�����һ������ͷ
        if (Input.GetKeyDown(KeyCode.E))
        {
            activeCameraIndex = (activeCameraIndex + 1) % cameras.Count;
            ActivateCamera(activeCameraIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // ���� Esc ��
        {
            Application.Quit(); // �˳���Ϸ
        }
    }
}
