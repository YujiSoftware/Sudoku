$script = <<-SCRIPT
sudo apt-get install -y git

# Rust
curl -LOsSf https://sh.rustup.rs
./rustup-init.sh -y

# Go
curl -LOsSf https://golang.org/dl/go1.15.linux-amd64.tar.gz
tar -C /usr/local -xzf go1.15.linux-amd64.tar.gz
echo 'export PATH=$PATH:/usr/local/go/bin' > /etc/profile.d/go.sh

# Java
curl -LOsSf https://github.com/AdoptOpenJDK/openjdk14-binaries/releases/download/jdk-14.0.2%2B12/OpenJDK14U-jdk_x64_linux_hotspot_14.0.2_12.tar.gz
mkdir /usr/local/java
tar -x -C /usr/local/java -f OpenJDK14U-jdk_x64_linux_hotspot_14.0.2_12.tar.gz
echo 'export PATH=$PATH:/usr/local/java/jdk-14.0.2+12/bin/' > /etc/profile.d/java.sh

# .NET SDK 3.1
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-3.1

# Ruby
apt install -y rbenv
CONFIGURE_OPTS='--disable-install-rdoc' rbenv install 2.7.1
rbenv global 2.7.1

SCRIPT

Vagrant.configure("2") do |config|
  config.vm.box = "ubuntu/bionic64"  
  config.vm.provision "shell", inline: $script

  config.vm.synced_folder "./data", "/Vagrant"
  
  config.vm.provider "virtualbox" do |vm|
    vm.memory = 4096
  end
end