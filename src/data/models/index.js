import sequelize from '../sequelize';
import User from './User';
import UserLogin from './UserLogin';
import UserClaim from './UserClaim';
import UserProfile from './UserProfile';

import Alert from './Alert';
import AlertCriteria from './AlertCriteria';
import AlertUtilityPriceHistory from './AlertUtilityPriceHistory';
import PendingAlertNotification from './PendingAlertNotification';
import StateUtilityIndex from './StateUtilityIndex';
import UtilityPrice from './UtilityPrice';
import UtilityPriceHistory from './UtilityPriceHistory';

User.hasMany(UserLogin, {
  foreignKey: 'userId',
  as: 'logins',
  onUpdate: 'cascade',
  onDelete: 'cascade',
});

User.hasMany(UserClaim, {
  foreignKey: 'userId',
  as: 'claims',
  onUpdate: 'cascade',
  onDelete: 'cascade',
});

User.hasOne(UserProfile, {
  foreignKey: 'userId',
  as: 'profile',
  onUpdate: 'cascade',
  onDelete: 'cascade',
});

User.hasMany(AlertCriteria, {
  foreignKey: 'userId',
  as: 'alertCriteria',
});

AlertCriteria.belongsTo(User, {
  foreignKey: 'userId',
});

StateUtilityIndex.hasMany(AlertCriteria, {
  foreignKey: 'stateUtilityIndexId',
});

AlertCriteria.belongsTo(StateUtilityIndex, {
  foreignKey: 'stateUtilityIndexId',
});

Alert.belongsToMany(UtilityPriceHistory, {
  through: {
    model: 'AlertUtilityPriceHistory',
    unique: false,
  },
  as: 'utilityPriceHistories',
  foreignKey: 'alertId',
});

// UtilityPriceHistory.belongsToMany(Alert, {
//   through: {
//     model: Alert,
//     unique: false,
//   },
//   as: 'alerts',
//   foreignKey: 'utilityPriceHistoryId',
// });

function sync(...args) {
  return sequelize.sync(...args);
}

export default { sync };
export {
  User,
  UserLogin,
  UserClaim,
  UserProfile,
  Alert,
  AlertCriteria,
  AlertUtilityPriceHistory,
  StateUtilityIndex,
  PendingAlertNotification,
  UtilityPrice,
  UtilityPriceHistory,
};
