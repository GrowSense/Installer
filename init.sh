echo "Initializing nunit tests for GrowSense installer"

DIR=$PWD

cd lib && \
sh get-libs.sh && \
cd $DIR 
