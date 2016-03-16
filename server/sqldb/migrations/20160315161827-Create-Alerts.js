'use strict';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.createTable('alerts',
      {
        id: {
          type: Sequelize.INTEGER,
          primaryKey: true,
          autoIncrement: true
        },

        alertCriteriaId: {
          type: Sequelize.INTEGER,
          allowNull: false,
          references: {
            model: "alertCriteria",
            key: "id"
          }
        },

        utilityPriceId: {
          type: Sequelize.INTEGER,
          references: {
            model: "utilityPrices",
            key: "id"
          }
        },

        utlityPriceHistoryId: {
          type: Sequelize.INTEGER,
          allowNull: false,
          references: {
            model: "utilityPriceHistories",
            key: "id"
          }
        },

        createdAt: {
          type: Sequelize.DATE,
          allowNull: false,
          defaultValue: Sequelize.literal("CURRENT_TIMESTAMP")
        },

        lastUpdatedAt: {
          type: Sequelize.DATE,
          allowNull: false,
          defaultValue: Sequelize.literal("CURRENT_TIMESTAMP")
        }
      },
      {
        engine: 'InnoDB', // default: 'InnoDB'
        charset: 'latin1' // default: null
      }
    );
  },

  down: function (queryInterface, Sequelize) {
    return queryInterface.dropTable('alerts');
  }
};
