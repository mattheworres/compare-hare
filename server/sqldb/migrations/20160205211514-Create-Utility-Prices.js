'use strict';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.createTable('utilityPrices',
      {
        id: {
          type: Sequelize.INTEGER,
          primaryKey: true,
          autoIncrement: true
        },

        priceType: {
          type: Sequelize.INTEGER,
          allowNull: false
        },

        supplierId: {
          type: Sequelize.STRING,
          allowNull: false
        },
        offerId: {
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

        secondaryPricePerUnit: Sequelize.DECIMAL(8,4),
        secondaryPriceType: Sequelize.INTEGER,

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

        termMonthLength: Sequelize.INTEGER,
        termEndDate: Sequelize.DATE,

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
    return queryInterface.dropTable('utilityPrices')
  }
};
