echo ""
echo "[download-and-install.sh] Downloading GrowSense installer then installing..."

BRANCH=$1

if [ ! "$BRANCH" ]; then
  BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
fi

if [ ! "$BRANCH" ]; then
  BRANCH=dev
fi

FULL_VERSION_URL="https://raw.githubusercontent.com/GrowSense/Installer/$BRANCH/fullversion.txt?$(date +%s)"

echo "[download-and-install.sh]   Full version URL: $FULL_VERSION_URL"

FULL_VERSION=$(curl -s -H 'Cache-Control: no-cache' "$FULL_VERSION_URL")

echo "[download-and-install.sh]   Full version: $FULL_VERSION"

RELEASE_URL="https://github.com/GrowSense/Installer/releases/download/v$FULL_VERSION-dev/GrowSense-Installer.$FULL_VERSION-$BRANCH.zip"

echo "[download-and-install.sh]   Release URL: $RELEASE_URL"

DOWNLOADED_FILE=$PWD/GrowSenseInstaller.zip

echo ""
echo "[download-and-install.sh]  Downloading release file..."

curl -L $RELEASE_URL -O $DOWNLOADED_FILE

mkdir -p tmp

echo ""
echo "[download-and-install.sh]  Unzipping release file..."
unzip $DOWNLOADED_FILE -d tmp

mono tmp/bin/Release/GSInstaller.exe
#$SUDO wget -nv --no-cache -O - $RELEASE_URL
