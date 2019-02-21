import DataType from 'sequelize';
import Model from '../sequelize';

const StateUtilityIndex = Model.define('StateUtilityIndex', {
  id: {
    type: DataType.INTEGER,
    allowNull: false,
    primaryKey: true,
    autoIncrement: true,
  },
  loaderDataIdentifier: {
    type: DataType.STRING(256),
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
  active: {
    type: DataType.BOOLEAN,
    allowNull: false,
    defaultValue: true,
  },
  lastUpdatedHash: {
    type: DataType.STRING(40),
    allowNull: true,
  },
  createdAt: {
    type: DataType.DATE,
    allowNull: false,
    defaultValue: DataType.NOW,
  },
});

export default StateUtilityIndex;
