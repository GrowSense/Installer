echo "Running NUnit tests..."

DIR=$PWD

#if [ -z "$CATEGORY" ]; then
#    CATEGORY="Unit"
#fi

#CATEGORY_INCLUDE=" --include=$CATEGORY"

mono lib/NUnit.Runners/tools/nunit-console.exe bin/Release/*Tests.dll || exit 1 #--include="$CATEGORY"

echo "" && \
echo "Testing complete"

