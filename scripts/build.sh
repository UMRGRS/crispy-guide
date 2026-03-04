#!/bin/bash
set -e  # stop on error

UNITY_PATH="/home/umrgrs/Unity/Hub/Editor/2022.3.62f3/"
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