import socket
import random
import struct
import sys
import os

player_index: int = random.randint(-200000000, -1)
listen_telemetry_port: int = 9001

targets_list_string_ipv4_port = []

# get file path
current_directory = os.path.dirname(os.path.abspath(__file__))
string_path_ips_file = os.path.join(current_directory, 'broadcast.txt')

def import_ips_from_file(file_path):
    # create file if it does not exist
    if not os.path.exists(file_path):
        with open(file_path, 'w') as f:
            f.write("127.0.0.1:9002\n")

    string_file_ips_content = ''
    if os.path.exists(file_path):
        with open(file_path, 'r') as f:
            string_file_ips_content = f.read().strip()

    string_lines = string_file_ips_content.split('\n')
    for string_line in string_lines:
        if string_line:
            try:
                target_ip, redirection_player_index_port = string_line.split(':')
                if len(target_ip.split('.')) != 4:
                    print(f"Invalid IP format in {file_path}. Expected 'ip:port'. Using defaults.")
                    continue
                if not redirection_player_index_port.isdigit():
                    print(f"Invalid port format in {file_path}. Expected 'ip:port'. Using defaults.")
                    continue
                redirection_player_index_port = int(redirection_player_index_port)
                targets_list_string_ipv4_port.append((target_ip, redirection_player_index_port))
                print(f"Loaded target: {target_ip}:{redirection_player_index_port}")
            except ValueError:
                print(f"Invalid format in {file_path}. Expected 'ip:port'. Using defaults.")

import_ips_from_file(string_path_ips_file)

# Check for command-line arguments
if len(sys.argv) > 1:
    try:
        player_index = int(sys.argv[1])
    except ValueError:
        print("Invalid arguments. Usage: python LiftOffPlayerIndexTelemetryRelay.py [player_index] [target_ip] [redirection_port]")
        sys.exit(1)

print(f"Player Index: {player_index}")
print(f"Listening Telemetry Port: {listen_telemetry_port}")

def listen_to_udp(port):
    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as udp_socket:
        udp_socket.bind(('0.0.0.0', port))
        print(f"Listening for UDP packets on port {port}...")

        try:
            while True:
                data, client_address = udp_socket.recvfrom(1024)
                bytes_telemetry = bytearray(data)
                # prepend player index as little endian int
                int_lenght = len(bytes_telemetry)
                #print(f"Received telemetry data of length {int_lenght} from {client_address}")
                bytes_telemetry = struct.pack('<i', player_index) + bytes_telemetry

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
