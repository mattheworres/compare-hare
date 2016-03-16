'use strict';

export default function(sequelize, DataTypes) {
  return sequelize.define('DistributorUpdate', {
    id: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true,
      autoIncrement: true
    },
    distributorId: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    distributorRateType: {
      type: DataTypes.STRING,
      allowNull: false
    },
    priceDataHash: {
      type: DataTypes.STRING,
      allowNull: false
    },
    lastUpdated: {
      type: DataTypes.DATE,
      allowNull: false,
      defaultValue: DataTypes.NOW
    }
  });
}
