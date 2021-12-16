echo "Testing download and install script..."

DIR=$PWD

bash pull-release-from-local.sh

cd scripts-download/

TMP_DIR=".tmp"

mkdir -p $TMP_DIR

echo "  Launching download-installer.sh"
bash download-installer.sh dev .tmp
#OUTPUT=$(bash download-installer.sh)
#echo $OUTPUT

rm $TMP_DIR -R
cd $PWD

echo "Finished testing script"
