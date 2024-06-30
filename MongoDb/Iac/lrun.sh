#!/bin/sh
docker build -t mongodbkey -f KeyGenDockerfile .
[ -d ./Output/ ] || mkdir -p ./Output/
cd ./Output
volume=$(pwd)
cd ..
docker run -it -v $volume:/data mongodbkey /app/gen.sh