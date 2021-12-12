#!/bin/bash

BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

if [ "$BRANCH" = "dev" ];  then
  git add buildnumber.txt && \
  git add full-version.txt && \
  git commit -am "Updated build number" && \
  git pull -X theirs origin $BRANCH --quiet && \
  git push origin $BRANCH --quiet
else
  echo "Skipping push version. Only pushed for 'dev' branch not '$BRANCH'"
fi
