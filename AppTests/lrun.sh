#!/bin/sh
rm -Rf coveragereport
docker build -t apptests .
cd ..
volume=$(pwd)
docker run -it -v $volume:/app apptests /app/AppTests/druntests.sh