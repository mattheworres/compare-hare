'use strict';

export default function(sequelize, DataTypes) {
  return sequelize.define('Distributor', {
    id: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true,
      autoIncrement: true
    },
    name: {
      type: DataTypes.STRING,
      allowNull: false
    },
    rateType: {
      type: DataTypes.STRING,
      allowNull: false
    },
    pricePerUnit: {
      type: DataTypes.DECIMAL,
      allowNull: false
    },
    priceUnit: {
      type: DataTypes.STRING,
      allowNull: false
    },
    priceFuture: DataTypes.DECIMAL,
    priceFutureStart: DataTypes.DATE,
    priceFutureEnd: DataTypes.DATE,
    hasCancellationFee: {
      type: DataTypes.BOOLEAN,
      allowNull: false
    },
    cancellationFee: DataTypes.DECIMAL,
    hasMonthlyFee: {
      type: DataTypes.BOOLEAN,
      allowNull: false
    },
    monthlyFee: DataTypes.DECIMAL,
    hasNetMetering: {
      type: DataTypes.BOOLEAN,
      allowNull: false
    },
    requiresDeposit: {
      type: DataTypes.BOOLEAN,
      allowNull: false
    },
    hasBulkDiscounts: {
      type: DataTypes.BOOLEAN,
      allowNull: false
    },
    offerUrl: DataTypes.STRING,
    supplierAddress1: DataTypes.STRING,
    supplierAddress2: DataTypes.STRING,
    supplierCity: DataTypes.STRING,
    supplierState: DataTypes.STRING,
    supplierZip: DataTypes.STRING,
    supplierPhone1: DataTypes.STRING,
    supplierPhone2: DataTypes.STRING,
    supplierUrl: DataTypes.STRING,
    comments: DataTypes.STRING,
    createdAt: {
      type: DataTypes.DATE,
      allowNull: false,
      defaultValue: DataTypes.NOW
    }
  });
}
