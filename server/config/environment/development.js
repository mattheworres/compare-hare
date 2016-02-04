'use strict';

// Development specific configuration
// ==================================
module.exports = {

  // Sequelize connection opions
  sequelize: {
    options: {
      username: 'root',
      password: 'passw0rd',
      logging: false,
      define: {
        timestamps: false
      }
    }
  },

  // Seed database on startup
  seedDB: false

};
