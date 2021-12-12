echo "Building installer..."

echo "  Dir: $PWD"

DIR=$PWD

xbuild /p:Configuration=Release /p:TargetFrameworkVersion=v4.5 src/GrowSense.Installer.sln  || exit 1
#/verbosity:quiet

echo "Finished building installer"
