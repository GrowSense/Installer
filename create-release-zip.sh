echo "Packaging release zip file..."

DIR=$PWD

#TMP_RELEASE_FOLDER=".tmp-release/"
#BIN_RELEASE_FOLDER="bin/Release"
RELEASES_FOLDER="releases"

rm releases/ -R

BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

VERSION_POSTFIX=""

if [ "$BRANCH" != "lts" ]; then
  VERSION_POSTFIX="-$BRANCH"
fi

sh clean.sh

VERSION="$(cat full-version.txt)"

#mkdir -p $TMP_RELEASE_FOLDER

#cp $BIN_RELEASE_FOLDER/BridgeArduinoSerialToMqttSplitCsv.exe $TMP_RELEASE_FOLDER/
#cp $BIN_RELEASE_FOLDER/BridgeArduinoSerialToMqttSplitCsv.exe.config $TMP_RELEASE_FOLDER/
#cp $BIN_RELEASE_FOLDER/BridgeArduinoSerialToMqttSplitCsv.exe.mdb $TMP_RELEASE_FOLDER/
#cp $BIN_RELEASE_FOLDER/BridgeArduinoSerialToMqttSplitCsv.Tests.dll $TMP_RELEASE_FOLDER/
#cp $BIN_RELEASE_FOLDER/BridgeArduinoSerialToMqttSplitCsv.Tests.dll.mdb $TMP_RELEASE_FOLDER/
#cp $BIN_RELEASE_FOLDER/BridgeArduinoSerialToMqttSplitCsv.Tests.Integration.dll $TMP_RELEASE_FOLDER/
#cp $BIN_RELEASE_FOLDER/BridgeArduinoSerialToMqttSplitCsv.Tests.Integration.dll.mdb $TMP_RELEASE_FOLDER/
#cp $BIN_RELEASE_FOLDER/ArduinoSerialControllerClient.dll $TMP_RELEASE_FOLDER/
#cp $BIN_RELEASE_FOLDER/duinocom.core.dll $TMP_RELEASE_FOLDER/
#cp $BIN_RELEASE_FOLDER/nunit.framework.dll $TMP_RELEASE_FOLDER/
#cp $BIN_RELEASE_FOLDER/M2Mqtt.Net.dll $TMP_RELEASE_FOLDER/

#echo "$VERSION" > $TMP_RELEASE_FOLDER/full-version.txt

mkdir -p $RELEASES_FOLDER

#cd .tmp/BridgeArduinoSerialToMqttSplitCsv

DIR=$PWD

echo ""
echo "  Zipping release..."
cd bin/Release/
echo "$VERSION" > full-version.txt
zip -q -r $DIR/releases/GrowSense-Installer.$VERSION$VERSION_POSTFIX.zip * -x nunit.framework* -x *.Tests.dll
cd $DIR

#cd $DIR/releases

#echo ""
#echo "  Unzipping release..."
#unzip BridgeArduinoSerialToMqttSplitCsv.$VERSION$VERSION_POSTFIX.zip -d BridgeArduinoSerialToMqttSplitCsv.$VERSION$VERSION_POSTFIX

#echo ""
#echo "  Listing release contents..."
#ls BridgeArduinoSerialToMqttSplitCsv.$VERSION$VERSION_POSTFIX -R

#cd $DIR

#rm .tmp -r


echo ""
echo "Finished packaging release zip file."
