using UnityEngine;
using UnityEngine.Events;

public class MouseRay : MonoBehaviour
{
    [SerializeField] UnityEvent<Vector3> OnMouseClicked;
    public float edgeThreshold = 50f; // ��Ե������ֵ�����أ�
    private float maxHight = 0;

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        // ��ȡ��Ļ��Ⱥ͸߶�
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Ray rayMidden = Camera.main.ScreenPointToRay(new Vector3(screenWidth / 2, screenHeight / 2, 0));
        Vector3 point = Vector3.zero;

        RaycastHit hitMidden;
        if (Physics.Raycast(rayMidden, out hitMidden))
        {
            if (hitMidden.collider != null )
            {
                point = hitMidden.point;
                point.y = point.y + 55f;
                //Debug.Log("Y:1 " + point.y);
            }
        }

        // ����Ƿ񿿽���Ļ���Ե // ����Ƿ񿿽���Ļ�ұ�Ե // ����Ƿ񿿽���Ļ�ϱ�Ե // ����Ƿ񿿽���Ļ�±�Ե
        if (mousePos.x <= edgeThreshold || mousePos.x >= screenWidth - edgeThreshold || mousePos.y >= screenHeight - edgeThreshold || mousePos.y <= edgeThreshold)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider != null)
                {
                    point.x = hitInfo.point.x;
                    point.z = hitInfo.point.z;
                    if (maxHight > point.y)
                    {
                        point.y = maxHight;
                        //Debug.Log("Y:2 " + point.y);
                    }
                    else
                    {
                        maxHight = point.y;
                        //Debug.Log("Y:3 " + point.y);
                    }
                }
            }
        }
        else 
        {
            if (Input.GetKey(KeyCode.Space))
            {
                maxHight = point.y;
            }
            if (maxHight != 0) 
            {
                point.y = Mathf.Min(maxHight, point.y);
            }
            maxHight = point.y;
        }

        OnMouseClicked.Invoke(point);
        //Debug.Log(point + " " + hitMidden.point);

        //if (Input.GetMouseButtonDown(0)) 
    }
}
