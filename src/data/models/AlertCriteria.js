import DataType from 'sequelize';
import Model from '../sequelize';

const AlertCriteria = Model.define('AlertCriteria', {
  id: {
    type: DataType.INTEGER,
    allowNull: false,
    primaryKey: true,
    autoIncrement: true,
  },
  name: DataType.STRING,
  utilityState: {
    type: DataType.STRING(2),
    allowNull: false,
  },
  utilityType: {
    type: DataType.STRING(255),
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
  comments: DataType.STRING(255),
  stateUtilityIndexHash: {
    type: DataType.STRING(40),
    allowNull: true,
  },
  createdAt: {
    type: DataType.DATE,
    allowNull: false,
    defaultValue: DataType.NOW,
  },
});

export default AlertCriteria;
