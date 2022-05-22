#!/bin/bash

BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

if [ "$BRANCH" = "dev" ];  then
  git commit buildnumber.txt full-version.txt -m "Updated version [ci-skip]" && \
  bash push.sh
else
  echo "Skipping push version. Only pushed for 'dev' branch not '$BRANCH'"
fi
