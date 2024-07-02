#!/bin/sh

if ! command -v dotnet ef
then
  dotnet tool install --global dotnet ef
fi

cd ScadaCore
dotnet ef database update --context UserContext
dotnet ef database update --context ValueAndAlarmContext
