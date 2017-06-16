#! /etc/bash

mcs -r:/Applications/Unity/Unity.app/Contents/Frameworks/Managed/UnityEngine.dll -target:library ../Scripts/Logger.cs -out:Logger.dll