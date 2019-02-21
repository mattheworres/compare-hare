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
  uniqueIdentifier: {
    type: DataType.STRING(25),
    allowNull: false,
  },
  utilityState: {
    type: DataType.STRING(2),
    allowNull: false,
  },
  utilityType: {
    type: DataType.STRING(255),
    allowNull: false,
  },
  pricePerUnit: {
    type: DataType.FLOAT,
    allowNull: true,
  },
  priceUnit: {
    type: DataType.STRING(50),
    allowNull: true,
  },
  flatRate: {
    type: DataType.STRING(120),
    allowNull: true,
  },
  priceStructure: {
    type: DataType.STRING(255),
    allowNull: false,
  },
  hasCancellationFee: {
    type: DataType.BOOLEAN,
    allowNull: false,
    defaultValue: false,
  },
  cancellationFee: DataType.STRING(255),
  hasMonthlyFee: {
    type: DataType.BOOLEAN,
    allowNull: false,
    defaultValue: false,
  },
  monthlyFee: DataType.STRING(255),
  hasEnrollmentFee: {
    type: DataType.BOOLEAN,
    allowNull: false,
    defaultValue: false,
  },
  enrollmentFee: DataType.STRING(255),
  hasNetMetering: {
    type: DataType.BOOLEAN,
    allowNull: false,
    defaultValue: false,
  },
  requiresDeposit: {
    type: DataType.BOOLEAN,
    allowNull: false,
    defaultValue: false,
  },
  hasBulkDiscounts: {
    type: DataType.BOOLEAN,
    allowNull: false,
    defaultValue: false,
  },
  isIntroductoryPrice: {
    type: DataType.BOOLEAN,
    allowNull: false,
    defaultValue: false,
  },
  hasRenewable: {
    type: DataType.BOOLEAN,
    allowNull: false,
    defaultValue: false,
  },
  renewablePercentage: DataType.FLOAT,
  termMonthLength: DataType.INTEGER,
  termEndDate: DataType.DATE,
  offerUrl: DataType.STRING(256),
  supplierAddress1: DataType.STRING,
  supplierAddress2: DataType.STRING,
  supplierCity: DataType.STRING,
  supplierState: DataType.STRING,
  supplierZip: DataType.STRING,
  supplierPhone: DataType.STRING,
  comments: DataType.STRING,
  createdAt: {
    type: DataType.DATE,
    allowNull: false,
    defaultValue: DataType.NOW,
  },
});

export default UtilityPriceHistory;
