import DataType from 'sequelize';
import Model from '../sequelize';

const DistributorRateUpdate = Model.define('DistributorRateUpdate', {
  id: {
    type: DataType.INTEGER,
    allowNull: false,
    primaryKey: true,
    autoIncrement: true,
  },
  distributorRateId: {
    type: DataType.INTEGER,
    allowNull: false,
  },
  distributorRateType: {
    type: DataType.STRING,
    allowNull: false,
  },
  distributorState: {
    type: DataType.STRING(2),
    allowNull: false,
  },
  priceDataHash: {
    type: DataType.STRING,
    allowNull: false,
  },
  lastUpdated: {
    type: DataType.DATE,
    allowNull: false,
    defaultValue: DataType.NOW,
  },
});

export default DistributorRateUpdate;
