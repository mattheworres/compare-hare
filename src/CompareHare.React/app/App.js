import '@babel/polyfill';

import React from 'react';
import {withRouter} from 'react-router';

import {SnackbarProvider} from 'material-ui-snackbar-provider';
import {Routes} from './features/shared/components';

const App = () => (
  <SnackbarProvider SnackbarProps={{autohideDuration: 4000}}>
    <Routes />
  </SnackbarProvider>
);

export default withRouter(App);
