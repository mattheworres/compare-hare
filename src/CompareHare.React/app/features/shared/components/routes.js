import React from 'react';
import {Switch, Route} from 'react-router';

import {Dashboard} from '../../dashboard/components';
import {NotFound, AuthenticatedRoute} from './index';
import {
  SigninPage,
  LogoutPage,
  LoadIdentityPage,
} from '../../authentication/components';
import LandingPage from '../../landing/components/LandingPage';

export default () => (
  <Switch>
    <Route exact path="/" component={LandingPage} />

    <Route path="/load-identity" component={LoadIdentityPage} />
    <Route path="/signin" component={SigninPage} />
    <Route path="/logout" component={LogoutPage} />

    {/* Authenticated routes start below here */}
    <AuthenticatedRoute path="/dashboard" component={Dashboard} />

    <Route component={NotFound} />
  </Switch>
);
