import sequelize from '../sequelize';
import User from './User';
import UserLogin from './UserLogin';
import UserClaim from './UserClaim';
import UserProfile from './UserProfile';

import Alert from './Alert';
import AlertCriteria from './AlertCriteria';
import DistributorRate from './DistributorRate';
import DistributorRateUpdate from './DistributorRateUpdate';
import PendingAlertNotification from './PendingAlertNotification';
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
  onUpdate: 'cascade',
  onDelete: 'cascade',
});

User.hasMany(Alert, {
  foreignKey: 'userId',
  as: 'alerts',
  onUdpate: 'cascade',
  onDelete: 'cascade',
});

User.hasMany(PendingAlertNotification, {
  foreignKey: 'userId',
  as: 'pendingAlertNotifications',
  onUpdate: 'cascade',
  onDelete: 'cascade',
});

UtilityPrice.hasOne(UtilityPriceHistory, {
  onUpdate: 'cascade',
  onDelete: 'set null',
});

Alert.belongsTo(UtilityPriceHistory, {
  foreignKey: 'utilityPriceHistoryId',
  onDelete: 'set null',
});

AlertCriteria.hasOne(DistributorRate, {
  foreignKey: 'distributorRateId',
  onUpdate: 'cascade',
  onDelete: 'cascade',
});

DistributorRate.hasMany(AlertCriteria, {
  foreignKey: 'distributorRateId',
  onUpdate: 'cascade',
  onDelete: 'cascade',
});

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
  DistributorRate,
  DistributorRateUpdate,
  PendingAlertNotification,
  UtilityPrice,
  UtilityPriceHistory,
};
