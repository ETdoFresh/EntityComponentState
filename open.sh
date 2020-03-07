#!/bin/sh

cd "$(dirname "$0")"
/Applications/Unity/Hub/Editor/2019.1.14f1/Unity.app/Contents/MacOS/Unity -projectPath . &
open https://github.com/ETdoFresh/EntityComponentState/projects/1
for f in *.sln; do open $f; done
