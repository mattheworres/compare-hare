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
    references: {
      model: 'AlertCriteria',
      key: 'id',
    },
  },
  userId: {
    type: DataType.UUID,
    allowNull: false,
    references: {
      model: 'User',
      key: 'id',
    },
  },
  utilityState: {
    type: DataType.STRING(2),
    allowNull: false,
  },
  utilityType: {
    type: DataType.STRING(255),
    allowNull: false,
  },
  alertOfferHash: {
    type: DataType.STRING(40),
    allowNull: false,
  },
  createdAt: {
    type: DataType.DATE,
    allowNull: false,
    defaultValue: DataType.NOW,
  },
});

export default Alert;
