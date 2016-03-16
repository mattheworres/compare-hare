'use strict';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.createTable('distributorsUtilityPrices',
      {
        distributorId: {
          type: Sequelize.INTEGER,
          allowNull: false,
          references: {
            model: "distributors",
            key: "id"
          }
        },

        utilityPriceId: {
          type: Sequelize.INTEGER,
          allowNull: false,
          references: {
            model: "utilityPrices",
            key: "id"
          }
        }
      },
      {
        engine: 'InnoDB', // default: 'InnoDB'
        charset: 'latin1' // default: null
      }
    );
  },

  down: function (queryInterface, Sequelize) {
    return queryInterface.dropTable('distributorsUtilityPrices');
  }
};
