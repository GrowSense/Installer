#!/bin/bash

BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

if [ "$BRANCH" = "dev" ];  then
  git add buildnumber.txt && \
  git commit buildnumber.txt -m "Updated build number" && \
  git push origin $BRANCH && \
  git add full-version.txt && \
  git commit full-version.txt -m "Updated full version" && \
  git push origin $BRANCH
else
  echo "Skipping push version. Only pushed for 'dev' branch not '$BRANCH'"
fi
