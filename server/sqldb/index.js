/**
 * Sequelize initialization module
 */

'use strict';

import path from 'path';
import config from '../config/environment';
import Sequelize from 'sequelize';

var db = {
  Sequelize,
  sequelize: new Sequelize(config.sequelize.uri, config.sequelize.options)
};

// Insert models below
db.Thing = db.sequelize.import('../api/thing/thing.model');
db.Distributor = db.sequelize.import('../api/distributor/distributor.model');
db.User = db.sequelize.import('../api/user/user.model');
db.UtilityPrice = db.sequelize.import('../api/utilityPrice/utilityPrice.model');
db.UtilityPriceHistory = db.sequelize.import('../api/utilityPriceHistory/utilityPriceHistory.model');
db.AlertCriteria = db.sequelize.import('../api/alertCriteria/alertCritieria.model');
db.Alert = db.sequelize.import('../api/alert/alert.model');
db.PendingAlertNotification = db.sequelize.import('../api/pendingAlertNotification/pendingAlertNotification.model');
db.DistributorUpdate = db.sequelize.import('../api/distributorUpdate/distributorUpdate.model');

// Define relationships below
db.User.hasOne(db.Distributor, {
  as: 'defaultDistributor'
  foreignKey: 'defaultDistributorId'
});

db.Distributor.hasMany(db.UtilityPrice, {
  foreignKey: 'distributorId',
  as: 'utilityPrices'
});

db.Distributor.hasMany(db.UtilityPriceHistory, {
  foreignKey: 'distributorId',
  as: 'utilityPriceHistories'
});

db.AlertCriteria.belongsTo(db.User);

db.AlertCriteria.hasOne(db.Distributor, {
  foreignKey: 'distributorId'
});

db.AlertCriteria.hasMany(db.Alert, {
  foreignKey: 'alertCriteriaId',
  as: 'alerts'
});

db.Alert.belongsTo(db.AlertCriteria);

db.Alert.hasOne(db.UtilityPrice, {
  foreignKey: 'utilityPriceId'
});

db.Alert.hasOne(db.UtilityPriceHistory, {
  foreignKey: 'utilityPriceHistoryId'
});

db.PendingAlertNotification.belongsTo(db.User);
db.PendingAlertNotification.hasOne(db.Distributor);
db.PendingAlertNotification.belongsTo(db.AlertCriteria);
//TODO: PendingALertNotification many to many w/price histories
db.DistributorUpdate.belongsTo(db.Distributor);

export default db;
