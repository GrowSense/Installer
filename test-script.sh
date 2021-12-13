echo "Testing download and install script..."

DIR=$PWD

cd scripts-download/

TMP_DIR=".tmp"

mkdir -p $TMP_DIR

OUTPUT=$(bash download-and-install.sh $)
[[ "$OUTPUT" == *"Hello World"* ]] || exit 1

rm $TMP_DIR -R
cd $PWD

echo "Finished testing script"
