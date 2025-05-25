import math
import socket
import random
import struct
import sys

player_index:int= random.randint( -200000000,-1)
listen_telemetry_port:int=9001
redirection_player_index_port:int=9002
target_ip:str='127.0.0.1'

targets_list_string_ipv4_port = []

# get file path
import os
current_directory = os.path.dirname(os.path.abspath(__file__))
string_path_ips_file = current_directory + os.sep + 'broadcast.txt'
def import_ips_from_file(file_path):
    # create file if it does not exist
    if not os.path.exists(string_path_ips_file):
        with open(string_path_ips_file, 'w') as f:
            f.write(f"{target_ip}:{redirection_player_index_port}\n")

    string_file_ips_content = ''

    if os.path.exists(string_path_ips_file):
        with open(string_path_ips_file, 'r') as f:
            string_file_ips_content = f.read().strip()

    string_lines = string_file_ips_content.split('\n')
    for string_line in string_lines:
        if string_line:
            try:
                target_ip, redirection_player_index_port = string_line.split(':')
                if len(target_ip.split('.')) != 4:
                    print(f"Invalid IP format in {string_path_ips_file}. Expected 'ip:port'. Using defaults.")
                    continue
                if not redirection_player_index_port.isdigit():
                    print(f"Invalid port format in {string_path_ips_file}. Expected 'ip:port'. Using defaults.")
                    continue
                redirection_player_index_port = int(redirection_player_index_port)
                targets_list_string_ipv4_port.append((target_ip,redirection_player_index_port))
                print (f"Loaded target: {target_ip}:{redirection_player_index_port}")
            except ValueError:
                print(f"Invalid format in {string_path_ips_file}. Expected 'ip:port'. Using defaults.")

import_ips_from_file(string_path_ips_file)

# params optional
# player index 
# target ip
# redirection port

# Check for command-line arguments
if len(sys.argv) > 1:
    try:
        # Parse player index if provided
        if len(sys.argv) > 1:
            player_index = int(sys.argv[1])
        # Parse target IP if provided
        if len(sys.argv) > 2:
            target_ip = sys.argv[2]
        # Parse redirection port if provided
        if len(sys.argv) > 3:
            redirection_player_index_port = int(sys.argv[3])
    except ValueError:
        print("Invalid arguments. Usage: python LiftOffPlayerIndexTelemetryRelay.py [player_index] [target_ip] [redirection_port]")
        sys.exit(1)



print (f"Player Index: {player_index}")
print (f"Target IP: {target_ip}")
print (f"Redirection Port: {redirection_player_index_port}")
print (f"Listening Telemetry Port: {listen_telemetry_port}")



def listen_to_udp(port):
    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as udp_socket:
        udp_socket.bind(('0.0.0.0', port))
        print(f"Listening for UDP packets on port {port}...")
        
        try:
            while True:
                data, client_address = udp_socket.recvfrom(1024)
                bytes_telemetry = bytearray(data)   
                # copy byte telemetry with int little endian index in front of the byte array
                bytes_telemetry = struct.pack('<i', player_index) + bytes_telemetry

                # send the byte array to the redirection player index port
                with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as udp_socket_send:
                    udp_socket_send.sendto(bytes_telemetry, (target_ip, redirection_player_index_port))
                for target_ip, redirection_port in targets_list_string_ipv4_port:
                    try:
                        with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as udp_socket_send:
                            udp_socket_send.sendto(bytes_telemetry, (target_ip, redirection_port))
                    except Exception as e:
                        print(f"Failed to send data to {target_ip}:{redirection_port} - {e}")
                   
        except Exception as e:
            print(f"An error occurred: {e}")

if __name__ == "__main__":
    listen_to_udp(listen_telemetry_port)




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

