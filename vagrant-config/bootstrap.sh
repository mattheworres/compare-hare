CONFIG_SRC=/vagrant/vagrant-config
MYSQL=/etc/mysql/mysql.conf.d
#NGINX=/etc/nginx

# The output of all these installation steps is noisy. With this utility
# the progress report is nice and concise.
function install {
  echo installing $1
  shift
  apt-get -y install "$@" >/dev/null 2>&1
}

function quietRun {
  "$@" > /dev/null 2>&1
}

install CUrl curl

echo Update Repositories
curl -sS https://dl.yarnpkg.com/debian/pubkey.gpg | sudo apt-key add - >/dev/null
echo "deb https://dl.yarnpkg.com/debian/ stable main" | tee /etc/apt/sources.list.d/yarn.list >/dev/null
curl -sL https://deb.nodesource.com/setup_8.x | sudo -E bash - >/dev/null
#apt-add-repository ppa:ondrej/php  >/dev/null
apt-get -y update  >/dev/null

install NodeJS nodejs
install NPM npm
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'
install Yarn yarn

#install nginx nginx-full

debconf-set-selections <<< 'mysql-server mysql-server/root_password password passw0rd'
debconf-set-selections <<< 'mysql-server mysql-server/root_password_again password passw0rd'
install MySql mysql-server
quietRun mysql -uroot -proot < $CONFIG_SRC/database-config.sql || echo "Run MySQL config"
quietRun cp $MYSQL/mysqld.cnf $MYSQL/mysqld.cnf.save || echo "Backup MySQL cnf"
quietRun cp $CONFIG_SRC/mysqld.cnf $MYSQL/mysqld.cnf || echo "Install new MySQL cnf"

# echo Updating Nginx config
# quietRun cp $CONFIG_SRC/comparehare $NGINX/sites-available/comparehare || echo "Copy Nginx config to sites-available"
# quietRun rm $NGINX/sites-enabled/* || echo "Remove existing sites-enabled from Nginx"
# quietRun ln -s $NGINX/sites-available/comparehare $NGINX/sites-enabled/comparehare || echo "Add Nginx config to sites-enabled"

#quietRun cp $CONFIG_SRC/xdebug.ini $PHP/mods-available/xdebug.ini || echo "Copy Xdebug config"

echo Restarting Services
quietRun systemctl restart mysql.service || echo "Restart MySQL"
#quietRun systemctl restart nginx.service || echo "Restart Nginx"

echo Creating CompareHare database
#runuser -l vagrant -c 'mysqladmin -u root -p passw0rd create comparehare'
quietRun mysqladmin -u root -p passw0rd create comparehare

#runuser -l vagrant -c 'sudo chown -R -H $USER: /vagrant'
#runuser -l vagrant -c 'sudo npm install -g gulp@3.9.0'
