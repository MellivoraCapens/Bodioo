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
    public WheelFrictionCurve friction;
    public float maxSteerAngle = 30;
    public float motorForce = 50;
    public Vector3 centerOfMass;
    public int gear;
    public float brakeForce;
    Rigidbody rb;
    private bool handbrake=false;
    private bool nitro=false;
    public ParticleSystem nos1, nos2;
    public ParticleSystem temp;
    private bool brake=false;
    
   
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass; 
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
       
        HandBrake();
        Nitro();
        print(backLeftW.rpm);
     
        
      
    }
    private void Update()
    {
        WheelPose();
    }

    public void GetInput()
    {
        if (Input.GetAxis("Vertical") == 1)
            m_verticalInput = 1;
        else if (Input.GetAxis("Vertical") == -1)
            m_verticalInput = -1;
        else
        {
            m_verticalInput = 0;
        }

        m_horizantalInput = Input.GetAxis("Horizontal");

        print(m_verticalInput);
    }
    public void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizantalInput;
        frontLeftW.steerAngle = m_steeringAngle;
        frontRightW.steerAngle = m_steeringAngle;


    }
    public void Accelerate()
    {
        if (gear == 0  )
        {
            if (m_verticalInput > 0)
                gear = 1;
            if (m_verticalInput < 0)
                gear = -1;
        }
        if (gear == -1 && m_verticalInput == 1 )
        {
            backLeftW.brakeTorque = brakeForce;
            frontLeftW.brakeTorque = brakeForce;
            backRightW.brakeTorque = brakeForce;
            frontRightW.brakeTorque = brakeForce;
            
            brake = true;
            if (rb.velocity.magnitude < 0.5f)
                gear++;
        }
        if (gear == -1 && m_verticalInput <=0)
        {
            if(brake)
            {
                backLeftW.brakeTorque = 0;
                frontLeftW.brakeTorque = 0;
                backRightW.brakeTorque = 0;
                frontRightW.brakeTorque = 0;
                brake = false;
            }
            backLeftW.motorTorque = m_verticalInput * -motorForce;
            frontLeftW.motorTorque = m_verticalInput * -motorForce;
            backRightW.motorTorque = m_verticalInput * -motorForce;
            frontRightW.motorTorque = m_verticalInput * -motorForce;
        }
        if (gear == 1 && m_verticalInput == -1 )
        {
            backLeftW.brakeTorque = brakeForce;
            frontLeftW.brakeTorque = brakeForce;
            backRightW.brakeTorque = brakeForce;
            frontRightW.brakeTorque = brakeForce;
            brake = true;
            if (rb.velocity.magnitude < 0.5f)
                gear--;
        }
        if (gear == 1&& m_verticalInput >= 0)
        {
            if (brake)
            {
                backLeftW.brakeTorque = 0;
                frontLeftW.brakeTorque = 0;
                backRightW.brakeTorque = 0;
                frontRightW.brakeTorque = 0;
                brake = false;
            }
            backLeftW.motorTorque = m_verticalInput * -motorForce;
            frontLeftW.motorTorque = m_verticalInput * -motorForce;
            backRightW.motorTorque = m_verticalInput * -motorForce;
            frontRightW.motorTorque = m_verticalInput * -motorForce;
        }


      


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
    public void HandBrake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            handbrake = true;
          backRightW.brakeTorque = 10000.0f;
            backLeftW.brakeTorque = 1000.0f;

            friction = backLeftW.sidewaysFriction;
            friction.stiffness = 0.5f;
            backLeftW.sidewaysFriction = friction;
            backRightW.sidewaysFriction = friction;
            friction.stiffness = 0.8f;
            frontLeftW.sidewaysFriction = friction;
            frontRightW.sidewaysFriction = friction;
           
            backRightW.brakeTorque = motorForce; 
            backLeftW.brakeTorque = motorForce;
            backLeftW.motorTorque = 0;
            backRightW.motorTorque = 0;



        }
        else if(handbrake)
        {
            friction = backLeftW.sidewaysFriction;
            friction.stiffness = 2;
            backLeftW.sidewaysFriction = friction;
            backRightW.sidewaysFriction = friction;

            backRightW.brakeTorque = 0;
            backLeftW.brakeTorque = 0;
            proccesTask();
            frontLeftW.sidewaysFriction = friction;
            frontRightW.sidewaysFriction = friction;
            
            backRightW.brakeTorque = 0;
            backLeftW.brakeTorque = 0;
            handbrake = false;


        }
    }
    IEnumerator proccesTask()
    {
        yield return new WaitForSeconds(1);
    }
    public void Nitro()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            nitro = true;
            friction = backRightW.forwardFriction;
            friction.stiffness = 3;
            backLeftW.forwardFriction = friction;
            backRightW.forwardFriction = friction;
            frontLeftW.forwardFriction = friction;
            frontRightW.forwardFriction = friction;
            temp=nos1;
            temp.emissionRate = 10;
            nos1.emissionRate = 100;
            nos2.emissionRate = 100;

        }
        else if(nitro)
        {

            friction = backRightW.forwardFriction;
            friction.stiffness = 1;
            backLeftW.forwardFriction = friction;
            backRightW.forwardFriction = friction;
            frontLeftW.forwardFriction = friction;
            frontRightW.forwardFriction = friction;
            nos1.emissionRate = 0;
            nos2.emissionRate = 0;
            nitro = false;
        }
    }
    public void CalculateStiffnes()
    {
        if (rb.velocity.magnitude > 40)
        {
            maxSteerAngle = 20;


        }
    }
    public void WheelPose()
    {

        backLeftT.Rotate(backLeftW.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        backRightT.Rotate(backRightW.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        frontRightT.Rotate(frontRightW.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        frontLeftT.Rotate(frontLeftW.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        frontLeftT.localEulerAngles = new Vector3(frontLeftT.localEulerAngles.x, frontLeftW.steerAngle - frontLeftT.localEulerAngles.z, frontLeftT.localEulerAngles.z);
        frontRightT.localEulerAngles = new Vector3(frontRightT.localEulerAngles.x, 180 + (frontRightW.steerAngle - frontRightT.localEulerAngles.z), frontRightT.localEulerAngles.z);

    }
}

