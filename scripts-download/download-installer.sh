echo ""
echo "[GSInstaller | download-installer.sh] Downloading GrowSense installer then installing..."

BRANCH=$1
GROWSENSE_BASE_DIR=$2

if [ ! "$BRANCH" ]; then
  echo "  [GSInstaller | download-installer.sh] Branch not provided as argument. Getting branch from local git repo..."
  BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
fi

if [ ! "$BRANCH" ]; then
  echo "  [GSInstaller | download-installer.sh] Branch not provided as argument. Using 'dev' default..."
  BRANCH=dev
fi

if [ ! "$GROWSENSE_BASE_DIR" ]; then
  echo "  [GSInstaller | download-installer.sh] GrowSense base directory not provided as argument. Using '/usr/local/GrowSense/' default..."
  BRANCH="/usr/local/GrowSense/"
fi

INSTALLER_DIR="$GROWSENSE_BASE_DIR/Installer"

echo "[GSInstaller | download-installer.sh]  Branch: $BRANCH"
echo "[GSInstaller | download-installer.sh]  GrowSense Base Directory: $GROWSENSE_BASE_DIR"
echo "[GSInstaller | download-installer.sh]  Installer Directory: $INSTALLER_DIR"

echo "[GSInstaller | download-installer.sh]  Checking for local installer zip file..."
echo "[GSInstaller | download-installer.sh]    $INSTALLER_DIR/GrowSenseInstaller.zip"

if [ -f "$INSTALLER_DIR/GrowSenseInstaller.zip" ]; then

  echo "[GSInstaller | download-installer.sh]  Found installer zip on filesystem..."
  echo "[GSInstaller | download-installer.sh]    $INSTALLER_DIR/GrowSenseInstaller.zip"
  
  echo "  Unzipping.."

  unzip -o $INSTALLER_DIR/GrowSenseInstaller.zip -d $INSTALLER_DIR/ || exit 1

else

  FULL_VERSION_URL="https://raw.githubusercontent.com/GrowSense/Installer/$BRANCH/full-version.txt?R=$(date +%s)"

  echo "[GSInstaller | download-installer.sh]   Full version URL: $FULL_VERSION_URL"

  FULL_VERSION=$(curl -sL "$FULL_VERSION_URL" -H "Cache-Control: no-cache, no-store, must-revalidate" -H "Pragma: no-cache" -H "Expires: 0")
  #FULL_VERSION=$(wget --no-cache -O - "$FULL_VERSION_URL")


  echo "[GSInstaller | download-installer.sh]   Full version: $FULL_VERSION"

  RELEASE_URL="https://github.com/GrowSense/Installer/releases/download/v$FULL_VERSION-dev/GrowSense-Installer.$FULL_VERSION-$BRANCH.zip"

  echo "[GSInstaller | download-installer.sh]   Release URL: $RELEASE_URL"

  DOWNLOADED_FILE=$PWD/GrowSenseInstaller.zip

  echo ""
  echo "[GSInstaller | download-installer.sh]  Downloading release file..."

  #curl -L $RELEASE_URL -O $DOWNLOADED_FILE
  wget $RELEASE_URL -O $DOWNLOADED_FILE || exit 1

  mkdir -p $INSTALLER_DIR || exit 1

  echo ""
  echo "[GSInstaller | download-installer.sh]  Unzipping release file..."
  unzip -o $DOWNLOADED_FILE -d $INSTALLER_DIR/ || exit 1

fi

echo ""
echo "[GSInstaller | download-installer.sh]  Launching installer..."
sudo mono $INSTALLER_DIR/GSInstaller.exe --branch=$BRANCH --install-to=$GROWSENSE_BASE_DIR --allow-skip-download=true || exit 1

echo "[GSInstaller | download-installer.sh] Finished downloading installer."
