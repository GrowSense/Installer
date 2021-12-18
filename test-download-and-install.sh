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

tmp_dir="$PWD/.tmp/GrowSense"

mkdir -p $tmp_dir

echo "[test-script.sh]   Launching download-installer.sh"
bash scripts-download/download-installer.sh --branch=dev --to=$tmp_dir || exit 1
#OUTPUT=$(bash download-installer.sh)
#echo $OUTPUT

#rm $tmp_dir -R
cd $PWD

echo "[test-script.sh] Finished testing script"
