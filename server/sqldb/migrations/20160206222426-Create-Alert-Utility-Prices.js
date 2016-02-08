'use strict';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.createTable('alertUtlityPrices',
      {
        alert_id: {
          type: Sequelize.INTEGER,
          allowNull: false
        },

        utlity_price_id: {
          type: Sequelize.INTEGER,
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
    return queryInterface.dropTable('alertUtlityPrices');
  }
};
