'use strict';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.createTable('distributorUpdates',
      {
        id: {
          type: Sequelize.INTEGER,
          primaryKey: true,
          autoIncrement: true
        },

        distributorId: {
          type: Sequelize.INTEGER,
          allowNull: false,
          references: {
            model: "distributors",
            key: "id"
          }
        },

        distributorRateType: {
          type: Sequelize.STRING,
          allowNull: false
        },

        priceDataHash: {
          type: Sequelize.STRING,
          allowNull: false
        },

        lastUpdated: {
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
    return queryInterface.dropTable('distributorUpdates');
  }
};
