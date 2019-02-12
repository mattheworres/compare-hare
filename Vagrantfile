# -*- mode: ruby -*-
# vi: set ft=ruby :

Vagrant.configure("2") do |config|
  config.vm.hostname = 'comparehare-dev'
  config.vm.box = "ubuntu/xenial64"
  config.vm.synced_folder "./", "/vagrant",
    id: "app",
    owner: "vagrant",
    group: "vagrant",
    mount_options: ["dmode=775,fmode=664"]

  config.vm.provider "virtualbox" do |vb|
    vb.name = "CompareHare Dev"
    vb.cpus = 1
    vb.memory = 1024
    # Disable the generation of the console log file
    # https://groups.google.com/forum/#!topic/vagrant-up/eZljy-bddoI
    vb.customize [ "modifyvm", :id, "--uartmode1", "disconnected" ]
    vb.customize ["setextradata", :id, "VBoxInternal2/SharedFoldersEnableSymlinksCreate/vagrant", "1"]
  end

  config.vm.provision :shell, path: 'vagrant-config/bootstrap.sh', keep_color: true

  config.vm.network "private_network", type: "dhcp"
  config.vm.network :forwarded_port, guest: 80, host: 8000, hostIp: '127.0.0.1'
  config.vm.network :forwarded_port, guest: 3306, host: 3307, hostIp: '127.0.0.1'
end
