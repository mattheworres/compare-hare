import DataType from 'sequelize';
import Model from '../sequelize';

const Alert = Model.define('Alert', {
  id: {
    type: DataType.INTEGER,
    allowNull: false,
    primaryKey: true,
    autoIncrement: true,
  },
  alertCriteriaId: {
    type: DataType.INTEGER,
    allowNull: false,
  },
  priceOfferId: {
    type: DataType.STRING,
    allowNull: false,
  },
  distributorState: {
    type: DataType.STRING(2),
    allowNull: false,
  },
  utilityPriceHistoryId: {
    type: DataType.INTEGER,
    allowNull: false,
  },
  createdAt: {
    type: DataType.DATE,
    allowNull: false,
    defaultValue: DataType.NOW,
  },
  lastUpdatedAt: {
    type: DataType.DATE,
    allowNull: false,
    defaultValue: DataType.NOW,
  },
});

export default Alert;
