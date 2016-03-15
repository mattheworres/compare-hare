'use strict';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.createTable('alerts',
      {
        id: {
          type: Sequelize.INTEGER,
          allowNull: false
        },

        alert_criteria_id: {
          type: Sequelize.INTEGER,
          allowNull: false
        },

        utility_price_id: Sequelize.INTEGER,

        utlity_price_history_id: {
          type: Sequelize.INTEGER,
          allowNull: false
        },

        createdAt: {
          type: Sequelize.DATE,
          allowNull: false
        },

        lastUpdatedAt: {
          type: Sequelize.DATE,
          allowNull: false
        }
      },
      {
        engine: 'InnoDB', // default: 'InnoDB'
        charset: 'latin1' // default: null
      }
      };
  },

  down: function (queryInterface, Sequelize) {
    return queryInterface.dropTable('alerts');
  }
};
