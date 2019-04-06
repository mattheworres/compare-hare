import React from 'react';
import {Page} from '../../layout/components';
import {connect} from 'react-redux';
import {AlertsTable} from './index';
import {Grid} from '@material-ui/core';
import {AddAlertModal, SaveAlertProgressModal} from '../../alerts/components';
import {
  AddPaPowerAlert1stModal,
  AddPaPowerAlert2ndModal,
  AddPaPowerAlert3rdModal,
} from '../../alerts/components/paPower';

class Dashboard extends React.Component {
  render() {
    return (
      <Page>
        <Grid container spacing={16}>
          <Grid item xs={false} sm={false} md={false} lg={1} />
          <Grid item xs={12} sm={12} md={12} lg={10}>
            <AlertsTable />
          </Grid>
          <Grid item xs={false} sm={false} md={false} lg={1}/>
        </Grid>
        <AddAlertModal />
        <AddPaPowerAlert1stModal />
        <AddPaPowerAlert2ndModal />
        <AddPaPowerAlert3rdModal />
        <SaveAlertProgressModal />
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
