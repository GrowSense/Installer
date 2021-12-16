echo "Testing download and install script..."

DIR=$PWD

bash pull-release-from-local.sh

rm GSInstaller.zip

TMP_DIR="$PWD/.tmp/GrowSense"

mkdir -p $TMP_DIR

echo "  Launching download-installer.sh"
bash scripts-download/download-installer.sh dev $TMP_DIR
#OUTPUT=$(bash download-installer.sh)
#echo $OUTPUT

rm $TMP_DIR -R
cd $PWD

echo "Finished testing script"
