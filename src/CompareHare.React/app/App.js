import '@babel/polyfill';

import React from 'react';
import {withRouter} from 'react-router';

//import {SnackbarProvider} from 'material-ui-snackbar-provider';
import {Routes} from './features/shared/components';

// const App = () => (
//   <SnackbarProvider>
//     <Routes />
//   </SnackbarProvider>
// );

const App = () => (
  <Routes />
);

export default withRouter(App);
