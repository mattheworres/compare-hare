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
    type: DataType.UUID,
    allowNull: false,
    references: {
      model: 'User',
      key: 'id',
    },
  },
  alertId: {
    type: DataType.INTEGER,
    allowNull: false,
    references: {
      model: 'Alert',
      key: 'id',
    },
  },
  createdAt: {
    type: DataType.DATE,
    allowNull: false,
    defaultValue: DataType.NOW,
  },
});

export default PendingAlertNotification;
