import DataType from 'sequelize';
import Model from '../sequelize';

const AlertCriteria = Model.define('AlertCriteria', {
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
  name: DataType.STRING,
  distributorRateId: {
    type: DataType.INTEGER,
    allowNull: false,
  },
  distributorState: {
    type: DataType.STRING(2),
    allowNull: false,
  },
  priceType: {
    type: DataType.INTEGER,
    allowNull: false,
  },
  minimumPrice: DataType.DECIMAL,
  maximumPrice: DataType.DECIMAL,
  minimumRenewablePercent: DataType.DECIMAL,
  maximumRenewablePercent: DataType.DECIMAL,
  minimumMonthLength: DataType.INTEGER,
  maximumMonthLength: DataType.INTEGER,
  hasCancellationFee: {
    type: DataType.BOOLEAN,
    allowNull: false,
  },
  hasMonthlyFee: {
    type: DataType.BOOLEAN,
    allowNull: false,
  },
  hasNetMetering: {
    type: DataType.BOOLEAN,
    allowNull: false,
  },
  requiresDeposit: {
    type: DataType.BOOLEAN,
    allowNull: false,
  },
  hasBulkDiscounts: {
    type: DataType.BOOLEAN,
    allowNull: false,
  },
  comments: DataType.STRING,
  createdAt: {
    type: DataType.DATE,
    allowNull: false,
    defaultValue: DataType.NOW,
  },
});

export default AlertCriteria;
