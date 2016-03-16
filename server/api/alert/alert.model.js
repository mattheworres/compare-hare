'use strict';

export default function(sequelize, DataTypes) {
  return sequelize.define('Alert', {
    id: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true,
      autoIncrement: true
    },
    alertCriteriaId: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    utilityPriceId: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    utilityPriceHistoryId: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    createdAt: {
      type: DataTypes.DATE,
      allowNull: false,
      defaultValue: DataTypes.NOW
    },
    lastUpdatedAt: {
      type: DataTypes.DATE,
      allowNull: false,
      defaultValue: DataTypes.NOW
    }
  });
}
