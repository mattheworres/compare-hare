import DataType from 'sequelize';
import Model from '../sequelize';

const AlertUtilityPriceHistory = Model.define('AlertUtilityPriceHistory', {
  id: {
    type: DataType.INTEGER,
    primaryKey: true,
    autoIncrement: true,
  },
  alertId: {
    type: DataType.INTEGER,
    allowNull: false,
  },
});

export default AlertUtilityPriceHistory;
