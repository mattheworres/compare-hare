'use strict';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.createTable('pendingAlertNotificationsUtilityPriceHistories',
      {
        pendingAlertNotificationId: {
          type: Sequelize.INTEGER,
          allowNull: false,
          references: {
            model: "pendingAlertNotifications",
            key: "id"
          }
        },

        utilityPriceHistoryId: {
          type: Sequelize.INTEGER,
          allowNull: false,
          references: {
            model: "utilityPriceHistories",
            key: "id"
          }
        }
      },
      {
        engine: 'InnoDB', // default: 'InnoDB'
        charset: 'latin1' // default: null
      }
    );
  },

  down: function (queryInterface, Sequelize) {
    return queryInterface.dropTable('pendingAlertNotificationsUtilityPriceHistories');
  }
};
