'use strict';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.createTable('users',
      {
        _id: {
          type: Sequelize.INTEGER,
          primaryKey: true,
          autoIncrement: true
        },

        name: {
          type: Sequelize.STRING,
          allowNull: false
        },

        email: {
          type: Sequelize.STRING,
          allowNull: false
        },

        role: {
          type: Sequelize.STRING,
          allowNull: false
        },

        password: {
          type: Sequelize.STRING,
          allowNull: false
        },

        provider: {
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
        github: Sequelize.TEXT
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
