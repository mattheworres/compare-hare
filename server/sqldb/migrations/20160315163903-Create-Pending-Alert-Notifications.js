'use strict';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.createTable('pendingAlertNotifications',
      {
        id: {
          type: Sequelize.INTEGER,
          primaryKey: true,
          autoIncrement: true
        },

        userId: {
          type: Sequelize.INTEGER,
          allowNull: false,
          references: {
            model: "users",
            key: "id"
          }
        },

        distributorId: {
          type: Sequelize.INTEGER,
          allowNull: false,
          references: {
            model:"distributors",
            key: "id"
          }
        },

        alertCriteriaId: {
          type: Sequelize.INTEGER,
          allowNull: false,
          references: {
            model: "alertCriteria",
            key: "id"
          }
        },

        createdAt: {
          type: Sequelize.DATE,
          allowNull: false
        }
      },
      {
        engine: 'InnoDB', // default: 'InnoDB'
        charset: 'latin1' // default: null
      }
    );
  },

  down: function (queryInterface, Sequelize) {
    return queryInterface.dropTable('pendingAlertNotifications');
  }
};
