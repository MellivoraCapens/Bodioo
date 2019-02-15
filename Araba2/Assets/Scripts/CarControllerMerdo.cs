using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerMerdo : MonoBehaviour
{
    private float m_horizantalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public WheelCollider frontLeftW, frontRightW;
    public WheelCollider backLeftW, backRightW;
    public Transform frontLeftT, frontRightT;
    public Transform backLeftT, backRightT;

    public float maxSteerAngle = 30;
    public float motorForce = 50;

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPose();
    }

    public void GetInput()
    {
        m_horizantalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");

    }
    public void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizantalInput;
        frontLeftW.steerAngle = m_steeringAngle;
        frontRightW.steerAngle = m_steeringAngle;

    }
    public void Accelerate()
    {
        backLeftW.motorTorque = m_verticalInput * -motorForce;
        frontLeftW.motorTorque = m_verticalInput * -motorForce;
        backRightW.motorTorque = m_verticalInput * -motorForce;
        frontRightW.motorTorque = m_verticalInput * -motorForce;

    }
    public void UpdateWheelPose()
    {
        UpdateWheelPose(frontLeftW, frontLeftT);
        UpdateWheelPose(frontRightW, frontRightT);
        UpdateWheelPose(backLeftW, backLeftT);
        UpdateWheelPose(backRightW, backRightT);

    }
    public void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;
        _collider.GetWorldPose(out _pos, out _quat);
      
        _transform.rotation = _quat;
    }
    public void Brake()
    {

    }

}

