using System;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine.Events;


namespace Eloi.LiftoffWrapper
{

    using UnityEngine;



    [System.Serializable]
    public struct STRUCT_Telemetry
    {
        public int m_playerIndex;
        public float m_timestamp;
        public float m_positionX;
        public float m_positionY;
        public float m_positionZ;
        public float m_rotationX;
        public float m_rotationY;
        public float m_rotationZ;
        public float m_rotationW;
        public float m_velocitySpeedX;
        public float m_velocitySpeedY;
        public float m_velocitySpeedZ;
        public float m_gyroPitch;
        public float m_gyroRoll;
        public float m_gyroYaw;
        public float m_inputThrottle;
        public float m_inputYaw;
        public float m_inputRoll;
        public float m_inputPitch;
        public float m_batteryPercent;
        public float m_batteryVoltage;

        public void GetVelocity(out Vector3 velocity)
        {
            velocity.x = m_velocitySpeedX;
            velocity.y = m_velocitySpeedY;
            velocity.z = m_velocitySpeedZ;
        }

        public void GetPosition(out Vector3 position)
        {
            position.x = m_positionX;
            position.y = m_positionY;
            position.z = m_positionZ;

        }

        public void GetRotation(out Quaternion rotation)
        {
            rotation.x = m_rotationX;
            rotation.y = m_rotationY;
            rotation.z = m_rotationZ;
            rotation.w = m_rotationW;
        }

        public void GetGyro(out Vector3 gyro)
        {
            gyro.x = m_gyroPitch;
            gyro.y = m_gyroRoll;
            gyro.z = m_gyroYaw;
        }

        public void GetPlayerIndex(out int index)
        {
            index = m_playerIndex;
        }
    }

    public class LiftoffMono_BytesToStructTelemetry : MonoBehaviour
    {

        public STRUCT_Telemetry m_lastParsedTelemetry = new STRUCT_Telemetry();
        public UnityEvent<STRUCT_Telemetry> m_onTelemetryParsed = new UnityEvent<STRUCT_Telemetry>();

        public void PushInBytes(byte[] bytesToParse)
        {

            Liftoff_BytesToStructTelemetry.ParseBytesToFullTelemetryWithPlqyerIndex(bytesToParse, out m_lastParsedTelemetry);
            m_onTelemetryParsed.Invoke(m_lastParsedTelemetry);

        }
    }
    public class Liftoff_BytesToStructTelemetry
    {
        public static void ParseBytesToFullTelemetryWithPlqyerIndex(in byte[] bytes, out STRUCT_Telemetry telemetry, int playerDefaultIndex = -42)
        {
            telemetry = new STRUCT_Telemetry();
            telemetry.m_playerIndex = System.BitConverter.ToInt32(bytes, 0);
            telemetry.m_timestamp = System.BitConverter.ToSingle(bytes, 4); // Corrected offset for timestamp
            telemetry.m_positionX = System.BitConverter.ToSingle(bytes, 8);
            telemetry.m_positionY = System.BitConverter.ToSingle(bytes, 12);
            telemetry.m_positionZ = System.BitConverter.ToSingle(bytes, 16);
            telemetry.m_rotationX = System.BitConverter.ToSingle(bytes, 20);
            telemetry.m_rotationY = System.BitConverter.ToSingle(bytes, 24);
            telemetry.m_rotationZ = System.BitConverter.ToSingle(bytes, 28);
            telemetry.m_rotationW = System.BitConverter.ToSingle(bytes, 32);
            telemetry.m_velocitySpeedX = System.BitConverter.ToSingle(bytes, 36);
            telemetry.m_velocitySpeedY = System.BitConverter.ToSingle(bytes, 40);
            telemetry.m_velocitySpeedZ = System.BitConverter.ToSingle(bytes, 44);
            telemetry.m_gyroPitch = System.BitConverter.ToSingle(bytes, 48);
            telemetry.m_gyroRoll = System.BitConverter.ToSingle(bytes, 52);
            telemetry.m_gyroYaw = System.BitConverter.ToSingle(bytes, 56);
            telemetry.m_inputThrottle = System.BitConverter.ToSingle(bytes, 60);
            telemetry.m_inputYaw = System.BitConverter.ToSingle(bytes, 64);
            telemetry.m_inputPitch = System.BitConverter.ToSingle(bytes, 68);
            telemetry.m_inputRoll = System.BitConverter.ToSingle(bytes, 72);
            telemetry.m_batteryPercent = System.BitConverter.ToSingle(bytes, 76);
            telemetry.m_batteryVoltage = System.BitConverter.ToSingle(bytes, 80);

            /**
             Timestamp (1 float) - current timestamp of the drone's flight. The unit scale is in seconds. This value is reset to zero when the drone is reset.
    Position (3 floats) - the drone's world position as a 3D coordinate. The unit scale is in meters. Each position component can be addressed individually as PositionX, PositionY, or PositionZ.
    Attitude (4 floats) - the drone's world attitude as a quaternion. Each quaternion component can be addressed individually as AttitudeX, AttitudeY, AttitudeZ and AttitudeW.
    Velocity (3 floats) - the drone's linear velocity as a 3D vector in world-space. The unit scale is in meters/second. Each component can be addressed individually as SpeedX, SpeedY, or SpeedZ. Note: to get the velocity in local-space, transform it[math.stackexchange.com] using the values in the Attitude data stream.
    Gyro (3 floats) - the drone's angular velocity rates, represented with three components in the order: pitch, roll and yaw. The unit scale is in degrees/second. Each component can also be addressed individually as GyroPitch, GyroRoll and GyroYaw.
    Input (4 floats) - the drone's input at that time, represented with four components in the following order: throttle, yaw, pitch and roll. Each input can be addressed individually as InputThrottle, InputYaw, InputPitch and InputRoll.
    Battery (2 floats) - the drone's current battery state, represented by the remaining voltage, and the charge percentage. Each of these two can be addressed individually with the BatteryPercentage and BatteryVoltage keys. Note - these values will only make sense when battery simulation is enabled in the game's options.
    MotorRPM (1 byte + (1 float * number of motors)) - the rotations per minute for each motor. The byte at the front of this piece of data defines the amount of motors on the drone, and thus how many floats you can expect to find next. The sequence of motors for a quadcopter in Liftoff is as follows: left front, right front, left back, right back.
             */

        }

        public static void ParseBytesToFullTelemetry(in byte[] bytes, out STRUCT_Telemetry telemetry)
        {
            telemetry = new STRUCT_Telemetry();
            telemetry.m_playerIndex = 0;
            telemetry.m_timestamp = System.BitConverter.ToSingle(bytes, 0); // Corrected offset for timestamp
            telemetry.m_positionX = System.BitConverter.ToSingle(bytes, 4);
            telemetry.m_positionY = System.BitConverter.ToSingle(bytes, 8); // Corrected offset for PositionY
            telemetry.m_positionZ = System.BitConverter.ToSingle(bytes, 12); // Corrected offset for PositionZ
            telemetry.m_rotationX = System.BitConverter.ToSingle(bytes, 16); // Corrected offset for RotationX
            telemetry.m_rotationY = System.BitConverter.ToSingle(bytes, 20); // Corrected offset for RotationY
            telemetry.m_rotationZ = System.BitConverter.ToSingle(bytes, 24); // Corrected offset for RotationZ
            telemetry.m_rotationW = System.BitConverter.ToSingle(bytes, 28); // Corrected offset for RotationW
            telemetry.m_velocitySpeedX = System.BitConverter.ToSingle(bytes, 32); // Corrected offset for VelocitySpeedX
            telemetry.m_velocitySpeedY = System.BitConverter.ToSingle(bytes, 36); // Corrected offset for VelocitySpeedY
            telemetry.m_velocitySpeedZ = System.BitConverter.ToSingle(bytes, 40); // Corrected offset for VelocitySpeedZ
            telemetry.m_gyroPitch = System.BitConverter.ToSingle(bytes, 44); // Corrected offset for GyroPitch
            telemetry.m_gyroRoll = System.BitConverter.ToSingle(bytes, 48); // Corrected offset for GyroRoll
            telemetry.m_gyroYaw = System.BitConverter.ToSingle(bytes, 52); // Corrected offset for GyroYaw
            telemetry.m_inputThrottle = System.BitConverter.ToSingle(bytes, 56); // Corrected offset for InputThrottle
            telemetry.m_inputYaw = System.BitConverter.ToSingle(bytes, 60); // Corrected offset for InputYaw
            telemetry.m_inputPitch = System.BitConverter.ToSingle(bytes, 64); // Corrected offset for InputPitch
            telemetry.m_inputRoll = System.BitConverter.ToSingle(bytes, 68); // Corrected offset for InputRoll
            telemetry.m_batteryPercent = System.BitConverter.ToSingle(bytes, 72); // Corrected offset for BatteryPercent
            telemetry.m_batteryVoltage = System.BitConverter.ToSingle(bytes, 76); // Corrected offset for BatteryVoltage



            /**
             Timestamp (1 float) - current timestamp of the drone's flight. The unit scale is in seconds. This value is reset to zero when the drone is reset.
    Position (3 floats) - the drone's world position as a 3D coordinate. The unit scale is in meters. Each position component can be addressed individually as PositionX, PositionY, or PositionZ.
    Attitude (4 floats) - the drone's world attitude as a quaternion. Each quaternion component can be addressed individually as AttitudeX, AttitudeY, AttitudeZ and AttitudeW.
    Velocity (3 floats) - the drone's linear velocity as a 3D vector in world-space. The unit scale is in meters/second. Each component can be addressed individually as SpeedX, SpeedY, or SpeedZ. Note: to get the velocity in local-space, transform it[math.stackexchange.com] using the values in the Attitude data stream.
    Gyro (3 floats) - the drone's angular velocity rates, represented with three components in the order: pitch, roll and yaw. The unit scale is in degrees/second. Each component can also be addressed individually as GyroPitch, GyroRoll and GyroYaw.
    Input (4 floats) - the drone's input at that time, represented with four components in the following order: throttle, yaw, pitch and roll. Each input can be addressed individually as InputThrottle, InputYaw, InputPitch and InputRoll.
    Battery (2 floats) - the drone's current battery state, represented by the remaining voltage, and the charge percentage. Each of these two can be addressed individually with the BatteryPercentage and BatteryVoltage keys. Note - these values will only make sense when battery simulation is enabled in the game's options.
    MotorRPM (1 byte + (1 float * number of motors)) - the rotations per minute for each motor. The byte at the front of this piece of data defines the amount of motors on the drone, and thus how many floats you can expect to find next. The sequence of motors for a quadcopter in Liftoff is as follows: left front, right front, left back, right back.
             */

        }
    }
}
