import DataType from 'sequelize';
import Model from '../sequelize';

const PendingAlertNotification = Model.define('PendingAlertNotification', {
  id: {
    type: DataType.INTEGER,
    allowNull: false,
    primaryKey: true,
    autoIncrement: true,
  },
  userId: {
    type: DataType.INTEGER,
    allowNull: false,
  },
  distributorRateId: {
    type: DataType.INTEGER,
    allowNull: false,
  },
  alertCriteriaId: {
    type: DataType.INTEGER,
    allowNull: false,
  },
  createdAt: {
    type: DataType.DATE,
    allowNull: false,
    defaultValue: DataType.NOW,
  },
});

export default PendingAlertNotification;
