echo ""
echo "[GSInstaller | download-and-install.sh] Downloading GrowSense installer then installing..."

BRANCH=$1
GROWSENSE_BASE_DIR=$2

if [ ! "$BRANCH" ]; then
  echo "  [GSInstaller | download-and-install.sh] Branch not provided as argument. Getting branch from local git repo..."
  BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
fi

if [ ! "$BRANCH" ]; then
  echo "  [GSInstaller | download-and-install.sh] Branch not provided as argument. Using 'dev' default..."
  BRANCH=dev
fi

if [ ! "$GROWSENSE_BASE_DIR" ]; then
  echo "  [GSInstaller | download-and-install.sh] GrowSense base directory not provided as argument. Using '/usr/local/GrowSense/' default..."
  BRANCH="/usr/local/GrowSense/"
fi

INSTALLER_DIR="$GROWSENSE_BASE_DIR/Installer"

echo "[GSInstaller | download-and-install.sh]  Branch: $BRANCH"
echo "[GSInstaller | download-and-install.sh]  GrowSense Base Directory: $GROWSENSE_BASE_DIR"
echo "[GSInstaller | download-and-install.sh]  Installer Directory: $INSTALLER_DIR"

FULL_VERSION_URL="https://raw.githubusercontent.com/GrowSense/Installer/$BRANCH/full-version.txt?R=$(date +%s)"

echo "[GSInstaller | download-and-install.sh]   Full version URL: $FULL_VERSION_URL"

FULL_VERSION=$(curl -sL "$FULL_VERSION_URL" -H "Cache-Control: no-cache, no-store, must-revalidate" -H "Pragma: no-cache" -H "Expires: 0")
#FULL_VERSION=$(wget --no-cache -O - "$FULL_VERSION_URL")


echo "[GSInstaller | download-and-install.sh]   Full version: $FULL_VERSION"

RELEASE_URL="https://github.com/GrowSense/Installer/releases/download/v$FULL_VERSION-dev/GrowSense-Installer.$FULL_VERSION-$BRANCH.zip"

echo "[GSInstaller | download-and-install.sh]   Release URL: $RELEASE_URL"

DOWNLOADED_FILE=$PWD/GrowSenseInstaller.zip

echo ""
echo "[GSInstaller | download-and-install.sh]  Downloading release file..."

#curl -L $RELEASE_URL -O $DOWNLOADED_FILE
wget $RELEASE_URL -O $DOWNLOADED_FILE

mkdir -p $INSTALLER_DIR

echo ""
echo "[GSInstaller | download-and-install.sh]  Unzipping release file..."
unzip -o $DOWNLOADED_FILE -d $INSTALLER_DIR/

mono $INSTALLER_DIR/GSInstaller.exe --branch=$BRANCH --install-to=$GROWSENSE_BASE_DIR --allow-skip-download=true

echo "[GSInstaller | download-and-install.sh] Finished downloading installer."
