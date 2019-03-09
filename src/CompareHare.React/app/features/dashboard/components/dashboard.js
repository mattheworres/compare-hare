import React from 'react';
import {Page} from '../../layout/components';
import {connect} from 'react-redux';
import {AlertsCard} from './index';
import {Grid} from '@material-ui/core';
import {AddAlertModal} from '../../alerts/components';
import {
  AddPaPowerAlert1stModal,
  AddPaPowerAlert2ndModal,
} from '../../alerts/components/paPower';

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
        <AddPaPowerAlert1stModal />
        <AddPaPowerAlert2ndModal />
      </Page>
    );
  }
}

function mapStateToProps() {
  return {

  };
}

const mapDispatchToProps = {

}

export default connect(mapStateToProps, mapDispatchToProps)(Dashboard);
