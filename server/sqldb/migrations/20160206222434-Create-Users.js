'use strict';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.createTable('users',
      {
        id: {
          type: Sequelize.INTEGER,
          allowNull: false
        },

        name: Sequelize.STRING,

        email: {
          type: Sequelize.STRING,
          allowNull: false
        },

        role: Sequelize.STRING,
        password: {
          type: Sequelize.STRING,
          allowNull: false
        },
        salt: {
          type: Sequelize.STRING,
          allowNull: false
        },
        facebook: Sequelize.TEXT,
        twitter: Sequelize.TEXT,
        google: Sequelize.TEXT,
        github: Sequelize.TEXT,

        default_distributor_id: Sequelize.INTEGER
        default_distributor_ratetype: Sequelize.STRING
      },
      {
        engine: 'InnoDB', // default: 'InnoDB'
        charset: 'latin1' // default: null
      }
    );
  },

  down: function (queryInterface, Sequelize) {
    return queryInterface.dropTable('users');
  }
};
