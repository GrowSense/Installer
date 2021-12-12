echo "Testing download and install script..."

DIR=$PWD
cd scripts-download/
OUTPUT=$(bash download-and-install.sh)
[[ "$OUTPUT" == *"Hello World"* ]] || exit 1
cd $PWD

echo "Finished testing script"
