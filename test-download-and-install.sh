echo "[test-script.sh] Testing download and install script..."

DIR=$PWD

sudo rm .tmp -R

echo "[test-script.sh]   Creating release zip..."

bash build.sh || exit 1
bash create-release-zip.sh || exit 1

RELEASE_FILE=""

echo "[test-script.sh]   Detecting release file..."

cd releases

FILES="*"
for f in $FILES
do
  echo "    $f"
  RELEASE_FILE=$f
done

cd $DIR

echo "[test-script.sh]   Copying release file to tmp dir..."
mkdir -p $DIR/.tmp/GrowSense/Installer

cp -v releases/$RELEASE_FILE $DIR/.tmp/GrowSense/Installer/GrowSenseInstaller.zip

bash pull-release-from-local.sh

#rm GSInstaller.zip

TMP_DIR="$PWD/.tmp/GrowSense"

mkdir -p $TMP_DIR

echo "[test-script.sh]   Launching download-installer.sh"
bash scripts-download/download-installer.sh dev $TMP_DIR || exit 1
#OUTPUT=$(bash download-installer.sh)
#echo $OUTPUT

#rm $TMP_DIR -R
cd $PWD

echo "[test-script.sh] Finished testing script"
