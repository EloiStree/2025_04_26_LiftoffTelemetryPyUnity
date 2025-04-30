import math
import socket
import struct
from scipy.spatial.transform import Rotation as R

def listen_to_udp(port):
    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as udp_socket:
        udp_socket.bind(('0.0.0.0', port))
        print(f"Listening for UDP packets on port {port}...")
        
        while True:
            data, client_address = udp_socket.recvfrom(1024)
            bytes_telemetry = bytearray(data)   

            timestamp = bytes_telemetry[0:4]
            position_x = bytes_telemetry[4:8]
            position_y = bytes_telemetry[8:12]
            position_z = bytes_telemetry[12:16]
            quaternion_x = bytes_telemetry[16:20]
            quaternion_y = bytes_telemetry[20:24]
            quaternion_z = bytes_telemetry[24:28]
            quaternion_w = bytes_telemetry[28:32]

            drone_position_x = struct.unpack('f', position_x[:4])[0]
            drone_position_y = struct.unpack('f', position_y[:4])[0]
            drone_position_z = struct.unpack('f', position_z[:4])[0]
            drone_quaternion_x = struct.unpack('f', quaternion_x[:4])[0]
            drone_quaternion_y = struct.unpack('f', quaternion_y[:4])[0]
            drone_quaternion_z = struct.unpack('f', quaternion_z[:4])[0]
            drone_quaternion_w = struct.unpack('f', quaternion_w[:4])[0]


            # Convert quaternion to Euler angles
            quat = [drone_quaternion_x, drone_quaternion_y, drone_quaternion_z, drone_quaternion_w]
            r = R.from_quat(quat)
            euler_rad = r.as_euler('yxz', degrees=False)
            drone_euler_x, drone_euler_y, drone_euler_z = [math.degrees(angle) for angle in euler_rad]

            #inverse y and x
            temp_value = drone_euler_x
            drone_euler_x = drone_euler_y
            drone_euler_y = temp_value

      

            print(f"Timestamp: {struct.unpack('f', timestamp[:4])[0]:.2f}, Position: ({drone_position_x:.2f}, {drone_position_y:.2f}, {drone_position_z:.2f}), Euler Angles: ({drone_euler_x:.2f}, {drone_euler_y:.2f}, {drone_euler_z:.2f})")
if __name__ == "__main__":
    listen_to_udp(9001)




# """
# https://steamcommunity.com/sharedfiles/filedetails/?id=3160488434
# C:\Users\Shadow\AppData\LocalLow\LuGus Studios\Liftoff  TelemetryConfiguration.json
# {
#     "EndPoint": "127.0.0.1:9001",
#     "StreamFormat": [
#       "Timestamp",
#       "Position",
#       "Attitude",
#       "Velocity",
#       "Gyro",
#       "Input",
#       "Battery",
#       "MotorRPM"
#     ]
#   }


# Timestamp (1 float) - current timestamp of the drone's flight. The unit scale is in seconds. This value is reset to zero when the drone is reset.
# Position (3 floats) - the drone's world position as a 3D coordinate. The unit scale is in meters. Each position component can be addressed individually as PositionX, PositionY, or PositionZ.
# Attitude (4 floats) - the drone's world attitude as a quaternion. Each quaternion component can be addressed individually as AttitudeX, AttitudeY, AttitudeZ and AttitudeW.
# Velocity (3 floats) - the drone's linear velocity as a 3D vector in world-space. The unit scale is in meters/second. Each component can be addressed individually as SpeedX, SpeedY, or SpeedZ. Note: to get the velocity in local-space, transform it[math.stackexchange.com] using the values in the Attitude data stream.
# Gyro (3 floats) - the drone's angular velocity rates, represented with three components in the order: pitch, roll and yaw. The unit scale is in degrees/second. Each component can also be addressed individually as GyroPitch, GyroRoll and GyroYaw.
# Input (4 floats) - the drone's input at that time, represented with four components in the following order: throttle, yaw, pitch and roll. Each input can be addressed individually as InputThrottle, InputYaw, InputPitch and InputRoll.
# Battery (2 floats) - the drone's current battery state, represented by the remaining voltage, and the charge percentage. Each of these two can be addressed individually with the BatteryPercentage and BatteryVoltage keys. Note - these values will only make sense when battery simulation is enabled in the game's options.
# MotorRPM (1 byte + (1 float * number of motors)) - the rotations per minute for each motor. The byte at the front of this piece of data defines the amount of motors on the drone, and thus how many floats you can expect to find next. The sequence of motors for a quadcopter in Liftoff is as follows: left front, right front, left back, right back.

# """

