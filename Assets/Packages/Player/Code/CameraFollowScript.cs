using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    private Transform ourDrone;
    private void Awake()
    {
        ourDrone = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    private Vector3 velocityCameraFollow;
    public Vector3 behindPosition = new Vector3(0, 0, 0);

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, ourDrone.transform.TransformPoint(behindPosition) + Vector3.up * Input.GetAxis("Vertical"), ref velocityCameraFollow,0.1f);
        //transform.rotation = Quaternion.Euler(new Vector3(45, ourDrone.GetComponent<DroneMovementScript>().currentYRotation, 0));
    }
}
