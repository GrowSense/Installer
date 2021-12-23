echo ""
echo "[GSInstaller | download-installer.sh] Downloading GrowSense installer then installing..."


for ARGUMENT in "$@"
do

    KEY=$(echo $ARGUMENT | cut -f1 -d=)
    VALUE=$(echo $ARGUMENT | cut -f2 -d=)   

    case "$KEY" in
            --branch)              branch=${VALUE} ;;
            --to)    install_to=${VALUE} ;;     
            --version)    version=${VALUE} ;;     
            *)   
    esac    


done

echo "[GSInstaller | download-installer.sh]   Branch: $branch"
echo "[GSInstaller | download-installer.sh]   Version: $version"
echo "[GSInstaller | download-installer.sh]   Install to: $install_to"

#BRANCH=$1
#install_to=$2

#if [ ! "$branch" ]; then
#  echo "  [GSInstaller | download-installer.sh] Branch not provided as argument. Getting branch from local git repo..."
#  BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
#fi

if [ ! "$branch" ]; then
  echo "[GSInstaller | download-installer.sh]   Branch not provided as argument. Using 'dev' default..."
  branch=dev
fi

if [ ! "$install_to" ]; then
  echo "[GSInstaller | download-installer.sh]   GrowSense base directory not provided as argument. Using '/usr/local/GrowSense/' default..."
  install_to="/usr/local/"
fi


if [[ "$install_to" == *"Index" ]] || [[ "$install_to" == *"Index/" ]]; then
  echo "[GSInstaller | download-installer.sh]   Install to contains /Index/. Stripping back to parent directory..."
  echo "[GSInstaller | download-installer.sh]     Before: $install_to"
  install_to="$(dirname "$install_to")"
  echo "[GSInstaller | download-installer.sh]     After: $install_to"
fi

if [[ "$install_to" == *"GrowSense" ]] || [[ "$install_to" == *"GrowSense/" ]]; then
  echo "[GSInstaller | download-installer.sh]   Install to contains /GrowSense/. Stripping back to parent directory..."
  echo "[GSInstaller | download-installer.sh]     Before: $install_to"
  install_to="$(dirname "$install_to")"
  echo "[GSInstaller | download-installer.sh]     After: $install_to"
fi

installer_dir="$install_to/GrowSense/Installer"
growsense_dir="$install_to/GrowSense"

echo "[GSInstaller | download-installer.sh]  Branch: $branch"
echo "[GSInstaller | download-installer.sh]  GrowSense Base Directory: $growsense_dir"
echo "[GSInstaller | download-installer.sh]  Installer Directory: $installer_dir"


echo "Installing mono..."

if ! type "xbuild" &>/dev/null; then
    echo "  Installing mono"
    sudo apt-get update -qq && sudo apt-get install -y tzdata mono-runtime mono-xsp4 ca-certificates-mono || exit 1  
else
  echo "  Mono is already installed. Skipping install."
fi

if ! type "xsp4" &>/dev/null; then
    echo "  Installing mono xsp4"
    sudo apt-get install -y tzdata mono-xsp4 || exit 1
else
  echo "  Mono xsp4 is already installed. Skipping install."
fi


echo ""
echo "  Checking mono version..."
mono --version

echo "Finished installing mono."


echo "[GSInstaller | download-installer.sh]  Checking for local installer zip file..."
echo "[GSInstaller | download-installer.sh]    $installer_dir/GrowSenseInstaller.zip"

if [ -f "$installer_dir/GrowSenseInstaller.zip" ]; then

  echo "[GSInstaller | download-installer.sh]  Found installer zip on filesystem..."
  echo "[GSInstaller | download-installer.sh]    $installer_dir/GrowSenseInstaller.zip"
  
  echo "  Unzipping.."

  unzip -o $installer_dir/GrowSenseInstaller.zip -d $installer_dir/ || exit 1

else

  full_version_url="https://raw.githubusercontent.com/GrowSense/Installer/$branch/full-version.txt?R=$(date +%s)"

  echo "[GSInstaller | download-installer.sh]   Full version URL: $full_version_url"

  full_version=$(curl -sL "$full_version_url" -H "Cache-Control: no-cache, no-store, must-revalidate" -H "Pragma: no-cache" -H "Expires: 0")
  #full_version=$(wget --no-cache -O - "$full_version_url")


  echo "[GSInstaller | download-installer.sh]   Full version: $full_version"

  release_url="https://github.com/GrowSense/Installer/releases/download/v$full_version-$branch/GrowSense-Installer.$full_version-$branch.zip"

  echo "[GSInstaller | download-installer.sh]   Release URL: $release_url"

  downloaded_file=$PWD/GrowSenseInstaller.zip

  echo ""
  echo "[GSInstaller | download-installer.sh]  Downloading release file..."

  #curl -L $release_url -O $downloaded_file
  wget $release_url -O $downloaded_file || exit 1

  mkdir -p $installer_dir || exit 1

  echo ""
  echo "[GSInstaller | download-installer.sh]  Unzipping release file..."
  unzip -o $downloaded_file -d $installer_dir/ || exit 1

fi

echo ""
echo "[GSInstaller | download-installer.sh]  Launching installer..."
sudo mono $installer_dir/GSInstaller.exe install --branch=$branch --install-to=$install_to --version=$version $4 $5 $6 $7 $8 $9 || exit 1

echo "[GSInstaller | download-installer.sh] Finished downloading installer."
