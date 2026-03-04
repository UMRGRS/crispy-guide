#!/bin/bash
set -e

UNITY_PATH="/opt/unity/Editor/Unity"
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