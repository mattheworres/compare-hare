import Sequelize, { Op } from 'sequelize';
import config from '../config';

const sequelize = new Sequelize(
  config.databaseName,
  config.databaseUser,
  config.databasePassword,
  {
    host: config.databaseHost,
    port: config.databasePort,
    dialect: 'mysql',
    operatorsAliases: Op,
    define: {
      freezeTableName: true,
    },
  },
);

export default sequelize;
