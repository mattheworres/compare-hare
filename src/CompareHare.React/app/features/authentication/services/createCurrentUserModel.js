import {Set} from 'immutable';
import {CurrentUserModel} from '../models';

export default function createCurrentUserModel(payload) {
  const {data} = payload;
  const userIdentityModel = data.userIdentityModel || data;

  userIdentityModel.roles = Set(userIdentityModel.roles);

  const model = new CurrentUserModel(userIdentityModel);

  return model.set('authenticated', true);
}
