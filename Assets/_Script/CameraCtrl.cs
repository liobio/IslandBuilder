using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using Cinemachine;
using static BuildItemSO;


public class CameraCtrl : MonoBehaviour
{

    public CinemachineCameraOffset Offset;
    public float step;
    public float maxDistance;
    public float minDistance;
    public float maxAngel;
    public float minAngel;
    public Transform cameraTarget;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                cameraTarget.rotation = Quaternion.Lerp(cameraTarget.rotation, Quaternion.Euler(minAngel, 0, 0), Time.deltaTime * 15f);
                return;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                cameraTarget.rotation = Quaternion.Lerp(cameraTarget.rotation, Quaternion.Euler(maxAngel, 0, 0), Time.deltaTime * 15f);
                return;
            }


        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {

            if (Offset.m_Offset.z < minDistance)
            {
                Offset.m_Offset = Vector3.Lerp(Offset.m_Offset, Offset.m_Offset + Vector3.forward * step, Time.deltaTime * 15f);
            }

        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {

            if (Offset.m_Offset.z > maxDistance)
            {
                Offset.m_Offset = Vector3.Lerp(Offset.m_Offset, Offset.m_Offset - Vector3.forward * step, Time.deltaTime * 15f);
            }

        }


    }
}
