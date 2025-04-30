# 2025_04_26_LiftoffTelemetryPyUnity


- https://www.liftoff-game.com
- https://store.steampowered.com/app/410340/Liftoff_FPV_Drone_Racing/
- https://store.steampowered.com/app/1891780/Liftoff__DJI_FPV/
  - for University:https://www.liftoff-game.com/our-products/liftoff-core

Was going to hack the game to have position of the drone in aim to make POC...
But the team of Liftoff are angels:
[https://steamcommunity.com/sharedfiles/filedetails/?id=3160488434](https://steamcommunity.com/sharedfiles/filedetails/?id=3160488434)

Find here python script and a Unity package to recovert the drone info with LiftOff Telemetry
![image](https://github.com/user-attachments/assets/f30fdd98-0699-4f90-ac37-0dddd1624958)

``` json
{
    "EndPoint": "127.0.0.1:9001",
    "StreamFormat": [
      "Timestamp",
      "Position",
      "Attitude",
      "Velocity",
      "Gyro",
      "Input",
      "Battery",
      "MotorRPM"
    ]
  }
```



![image](https://github.com/user-attachments/assets/4ae6b6b2-34cf-4173-bdef-c2ca5020ac9f)



![image](https://github.com/user-attachments/assets/a6caaaf8-464a-4357-9c18-be94b8c91206)



