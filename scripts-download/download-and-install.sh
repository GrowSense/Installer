echo ""
echo "Downloading GrowSense installer then installing..."

BRANCH=$1

if [ ! "$BRANCH" ]; then
  BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
fi

if [ ! "$BRANCH" ]; then
  BRANCH=dev
fi

FULL_VERSION=$(curl -s -H 'Cache-Control: no-cache' "https://raw.githubusercontent.com/GrowSense/Installer/$BRANCH/fullversion.txt")

echo "  Full version: $FULL_VERSION"

RELEASE_URL="https://github.com/GrowSense/Installer/releases/download/v$FULL_VERSION-dev/GrowSense-Installer.$FULL_VERSION-$BRANCH.zip"

echo "  Release URL: $RELEASE_URL"

$SUDO wget -nv --no-cache -O - $RELEASE_URL
