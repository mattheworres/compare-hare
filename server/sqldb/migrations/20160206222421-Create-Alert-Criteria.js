'use strict';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.createTable('alertCriteria',
      {
        id: {
          type: Sequelize.INTEGER,
          primaryKey: true,
          autoIncrement: true
        },

        user_id: {
          type: Sequelize.INTEGER,
          allowNull: false
        },

        distributor_id: {
          type: Sequelize.INTEGER,
          allowNull: false
        },

        distributorKey: {
          type: Sequelize.STRING,
          allowNull: false
        },

        distributorState: {
          type: Sequelize.STRING(2),
          allowNull: false
        },

        priceType: {
          type: Sequelize.INTEGER,
          allowNull: false
        },

        minimumPrice: Sequelize.DECIMAL(8,4),
        maximumPrice: Sequelize.DECIMAL(8,4),

        minimumRenewablePercent: Sequelize.DECIMAL(8,4),
        maximumRenewablePercent: Sequelize.DECIMAL(8,4),

        minimumMonthLength: Sequelize.INTEGER,
        maximumMonthLength: Sequelize.INTEGER,

        hasCancellationFee: {
          type: Sequelize.BOOLEAN,
          allowNull: false
        },

        hasMonthlyFee: {
          type: Sequelize.BOOLEAN,
          allowNull: false
        },

        hasNetMetereing: {
          type: Sequelize.BOOLEAN,
          allowNull: false
        },

        requiresDeposit: {
          type: Sequelize.BOOLEAN,
          allowNull: false
        },

        hasBulkDiscounts: {
          type: Sequelize.BOOLEAN,
          allowNull: false
        },

        comments: Sequelize.STRING,

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
    /*
      Add reverting commands here.
      Return a promise to correctly handle asynchronicity.

      Example:
      return queryInterface.dropTable('users');
    */
  }
};
