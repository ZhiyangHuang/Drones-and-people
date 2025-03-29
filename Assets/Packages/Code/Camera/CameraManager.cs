using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private List<Camera> cameras = new List<Camera>();
    private int activeCameraIndex = 0;

    // 静态字段，保存唯一的实例
    public static CameraManager Instance { get; private set; }

    void Awake()
    {
        // 如果已经有一个实例存在，销毁当前对象，确保只有一个 CameraManager 实例
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 销毁当前的实例
            return;
        }

        // 否则，设置为当前实例
        Instance = this;

        // 确保在场景切换时该对象不被销毁
        DontDestroyOnLoad(gameObject);


    }

    void Start()
    {
        // 查找带有特定标签的所有对象
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
            ActivateCamera(activeCameraIndex); // 激活第一个摄像头
        }
        //else
        //{
        //    Debug.LogWarning("No cameras found in the scene.");
        //}
    }


    // 注册摄像头
    public void RegisterCamera(Camera cam)
    {
        if (!cameras.Contains(cam))
        {
            cameras.Add(cam);
            //Debug.Log($"Camera {cam.name} registered.");
        }
    }

    // 激活指定摄像头
    public void ActivateCamera(int index)
    {
        if (index < 0 || index >= cameras.Count)
        {
            Debug.LogError("Invalid camera index.");
            return;
        }

        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].gameObject.SetActive(i == index); // 激活目标摄像头，禁用其他摄像头
        }

        activeCameraIndex = index;
        //Debug.Log($"Activated camera: {cameras[index].name}");
    }

    void Update()
    {
        // 按下 Q 键切换到上一个摄像头
        if (Input.GetKeyDown(KeyCode.Q))
        {
            activeCameraIndex = (activeCameraIndex - 1 + cameras.Count) % cameras.Count;
            ActivateCamera(activeCameraIndex);
        }

        // 按下 E 键切换到下一个摄像头
        if (Input.GetKeyDown(KeyCode.E))
        {
            activeCameraIndex = (activeCameraIndex + 1) % cameras.Count;
            ActivateCamera(activeCameraIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // 按下 Esc 键
        {
            Application.Quit(); // 退出游戏
        }
    }
}
