echo ""
echo "Downloading GrowSense installer then installing..."

BRANCH=$1

if [ ! "$BRANCH"]; then
  BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
fi

if [ ! "$BRANCH"]; then
  BRANCH=dev
fi

RELEASE_URL="https://github.com/GrowSense/Installer/releases/download/v$FULL_VERSION-dev/GrowSense-Installer.$FULL_VERSION-$BRANCH.zip

FULL_VERSION=$(curl -s -H 'Cache-Control: no-cache' "https://raw.githubusercontent.com/GrowSense/Index/$BRANCH/fullversion.txt")

$SUDO wget -nv --no-cache -O - https://raw.githubusercontent.com/GrowSense/Installer/$BRANCH/scripts-download/download-and-install.sh
