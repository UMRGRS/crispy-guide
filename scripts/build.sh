#!/bin/bash
set -e  # stop on error

UNITY_PATH="/opt/unity/Editor/Unity"
PROJECT_PATH=$(pwd)
BUILD_PATH="$PROJECT_PATH/dist"

echo "Preparing environment..."
mkdir -p $BUILD_PATH

echo "Building project..."

$UNITY_PATH \
  -batchmode \
  -quit \
  -projectPath $PROJECT_PATH \
  -buildTarget Android \
  -executeMethod BuildScript.PerformBuild

echo "Build finished successfully."