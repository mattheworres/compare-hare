import React from 'react';
import {connect} from 'react-redux';
import {Link} from 'react-router-dom';
import {
  Modal,
  withStyles,
  Grid,
  Typography,
  CircularProgress,
  Button,
  Paper,
} from '@material-ui/core';
import autobind from 'class-autobind';
import {
  updateProgressOpenSelector,
  updatingSelector,
  matchingOffersCountSelector,
  alertIdSelector,
  updateErrorSelector,
} from '../selectors/editAlert';
import {closeEditAlert} from '../actions/editAlert';
import {loadAlerts} from '../../dashboard/actions/alertsTable';

const styles = theme => ({
  modal: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center'
  },
  paper: {
    top: '30%',
    margin: 'auto',
    transform: 'translate(0 30%)',
    width: theme.spacing.unit * 51,
    backgroundColor: theme.palette.background.paper,
    boxShadow: theme.shadows[5],
    padding: theme.spacing.unit * 4,
    outline: 'none',
  },
  updatingTitle: {
    marginBottom: '20px',
  },
  loadingIndicator: {
    alignSelf: 'center',
    textAlign: 'center',
  }
});

class SaveAlertProgressModal extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  onClose() {
    const {closeEditAlert, loadAlerts} = this.props;

    loadAlerts();
    closeEditAlert();
  }

  renderUpdating() {
    const {classes} = this.props;
    return (
      <Grid container>
        <Grid item xs={12} className={classes.updatingTitle}>
          <Typography variant="h5">Updating Alert</Typography>
        </Grid>
        <Grid item xs={3} className={classes.loadingIndicator}>
          <CircularProgress size={50} />
        </Grid>
        <Grid item xs={9}>
          <Typography variant="subtitle1">
            Our bunnies will make those changes you wanted, then will check to see if there are any matching offers, one sec...
          </Typography>
        </Grid>
      </Grid>
    )
  }

  renderResultText() {
    const {matchingOffers, alertId, classes} = this.props;

    const hasOffers = matchingOffers > 0;

    return (
      <Grid container>
        <Grid item>
          <Typography variant="h5" className={classes.updatingTitle}>
            Awesome! Alert Updated!
          </Typography>
          {hasOffers && <Typography variant="subtitle1">
            Woo! Our bunnies found you <strong>{matchingOffers}</strong> new offers! <Link to={`/alerts/${alertId}/details`}>Take a look!</Link>
          </Typography>}
          {!hasOffers && <Typography variant="subtitle1">
            Our bunnies searched the latest offers, but didn&apos;t find anything &ndash; <em>yet</em>. We&apos;ll let you know when they find something, though!
            <br /><br /><Button variant="contained" color="primary" onClick={this.onClose}>Okay!</Button>
          </Typography>}
        </Grid>
      </Grid>
    );
  }

  render() {
    const {classes, open, updating} = this.props;

    return (
      <Modal open={open} onClose={this.onClose} className={classes.modal}>
        <Paper className={classes.paper}>
          {updating && this.renderUpdating()}
          {!updating && this.renderResultText()}
        </Paper>
      </Modal>
    );
  }
}

function mapStateToProps(state) {
  return {
    open: updateProgressOpenSelector(state),
    updating: updatingSelector(state),
    matchingOffers: matchingOffersCountSelector(state),
    alertId: alertIdSelector(state),
    updateError: updateErrorSelector(state),
  };
}

const mapDispatchToProps = {
  closeEditAlert,
  loadAlerts,
};

export default withStyles(styles)(connect(mapStateToProps, mapDispatchToProps)(SaveAlertProgressModal));
