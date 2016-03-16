'use strict';

export default function(sequelize, DataTypes) {
  return sequelize.define('AlertCriteria', {
    id: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true,
      autoIncrement: true
    },
    userId: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    distributorId: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    name: DataTypes.STRING,
    distributorRateType: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    distributorState: {
      type: DataTypes.STRING,
      allowNull: false
    },
    priceType: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    minimumPrice: DataTypes.DECIMAL,
    maximumPrice: DataTypes.DECIMAL,
    minimumRenewablePercent: DataTypes.DECIMAL,
    maximumRenewablePercent: DataTypes.DECIMAL,
    minimumMonthLength: DataTypes.INTEGER,
    maximumMonthLength: DataTypes.INTEGER,
    hasCancellationFee: {
      type: DataTypes.BOOLEAN,
      allowNull: false
    },
    hasMonthlyFee: {
      type: DataTypes.BOOLEAN,
      allowNull: false
    },
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
    comments: DataTypes.STRING,
    createdAt: {
      type: DataTypes.DATE,
      allowNull: false,
      defaultValue: DataTypes.NOW
    }
  });
}
