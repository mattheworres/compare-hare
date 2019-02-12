import DataType from 'sequelize';
import Model from '../sequelize';

const UtilityPriceHistory = Model.define('UtilityPriceHistory', {
  id: {
    type: DataType.INTEGER,
    allowNull: false,
    primaryKey: true,
    autoIncrement: true,
  },
  name: {
    type: DataType.STRING,
    allowNull: false,
  },
  priceType: {
    type: DataType.STRING,
    allowNull: false,
  },
  distributorState: {
    type: DataType.STRING(2),
    allowNull: false,
  },
  distributorRateId: {
    type: DataType.INTEGER,
    allowNull: false,
  },
  pricePerUnit: {
    type: DataType.DECIMAL,
    allowNull: false,
  },
  priceUnit: {
    type: DataType.STRING,
    allowNull: false,
  },
  priceFuture: DataType.DECIMAL,
  priceFutureStart: DataType.DATE,
  priceFutureEnd: DataType.DATE,
  secondaryPricePerUnit: DataType.DECIMAL,
  secondaryPriceType: DataType.INTEGER,
  hasCancellationFee: {
    type: DataType.BOOLEAN,
    allowNull: false,
  },
  cancellationFee: DataType.DECIMAL,
  hasMonthlyFee: {
    type: DataType.BOOLEAN,
    allowNull: false,
  },
  monthlyFee: DataType.DECIMAL,
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
  offerUrl: DataType.STRING,
  termMonthLength: DataType.INTEGER,
  termEndDate: DataType.DATE,
  supplierAddress1: DataType.STRING,
  supplierAddress2: DataType.STRING,
  supplierCity: DataType.STRING,
  supplierState: DataType.STRING,
  supplierZip: DataType.STRING,
  supplierPhone1: DataType.STRING,
  supplierPhone2: DataType.STRING,
  supplierUrl: DataType.STRING,
  comments: DataType.STRING,
  createdAt: {
    type: DataType.DATE,
    allowNull: false,
    defaultValue: DataType.NOW,
  },
});

export default UtilityPriceHistory;
