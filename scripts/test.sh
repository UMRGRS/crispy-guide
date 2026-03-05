#!/bin/bash
set -e

UNITY_PATH="/home/umrgrs/Unity/Hub/Editor/2022.3.62f3/Editor/Unity"
PROJECT_PATH=$(pwd)

echo "Running unit tests..."

$UNITY_PATH \
  -batchmode \
  -quit \
  -projectPath $PROJECT_PATH \
  -runTests \
  -testPlatform editmode \
  -logFile test.log

echo "Tests completed successfully."