'use strict';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.createTable('distributors',
      {
        id: {
          type: Sequelize.INTEGER,
          primaryKey: true,
          autoIncrement: true
        },

        name: {
          type: Sequelize.STRING,
          allowNull: false
        },

        rateType: {
          type: Sequelize.STRING,
          allowNull: false
        },

        pricePerUnit: {
          type: Sequelize.DECIMAL(8,4),
          allowNull: false
        },
        priceUnit: {
          type: Sequelize.STRING(25),
          allowNull: false
        },

        priceFuture: Sequelize.DECIMAL(8,4),
        priceFutureStart: Sequelize.DATE,
        priceFutureEnd: Sequelize.DATE,

        hasCancellationFee: {
          type: Sequelize.BOOLEAN,
          allowNull: false
        },
        cancellationFee: Sequelize.DECIMAL(8,4),

        hasMonthlyFee: {
          type: Sequelize.BOOLEAN,
          allowNull: false
        },
        monthlyFee: Sequelize.DECIMAL(8,4),

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

        offerUrl: Sequelize.STRING,

        supplierAddress1: Sequelize.STRING,
        supplierAddress2: Sequelize.STRING,
        supplierCity: Sequelize.STRING,
        supplierState: Sequelize.STRING,
        supplierZip: Sequelize.STRING,
        supplierPhone1: Sequelize.STRING,
        supplierPhone2: Sequelize.STRING,
        supplierUrl: Sequelize.STRING,

        comments: Sequelize.STRING,

        createdAt: {
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
    return queryInterface.dropTable('distributors');
  }
};
