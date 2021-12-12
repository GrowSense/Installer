#!/bin/bash

BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

if [ "$BRANCH" = "dev" ];  then
  git commit buildnumber.txt full-version.txt -m "Updated build number" && \
  git pull -X theirs origin $BRANCH --quiet && \
  git push origin $BRANCH --quiet
else
  echo "Skipping push version. Only pushed for 'dev' branch not '$BRANCH'"
fi
