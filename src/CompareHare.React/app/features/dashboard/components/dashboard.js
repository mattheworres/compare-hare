import React from 'react';
import {Page} from '../../layout/components';
import {AlertsCard} from './index';
import {Grid} from '@material-ui/core';
import {AddAlertModal} from '../../alerts/components';

class Dashboard extends React.Component {
  render() {
    return (
      <Page>
        <Grid container spacing={16}>
          <Grid item xs={2} />
          <Grid item xs={8}>
            <AlertsCard />
          </Grid>
          <Grid item xs={2} />
        </Grid>
        <AddAlertModal />
      </Page>
    );
  }
}

export default Dashboard;
