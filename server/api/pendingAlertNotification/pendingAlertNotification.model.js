'use strict';

export default function(sequelize, DataTypes) {
  return sequelize.define('PendingAlertNotification', {
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
    alertCriteriaId: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    createdAt: {
      type: DataTypes.DATE,
      allowNull: false,
      defaultValue: DataTypes.NOW
    }
  });
}
