import React from 'react';
import {connect} from 'react-redux';
import {Link} from 'react-router-dom';
import {
  Modal,
  withStyles,
  Grid,
  Stepper,
  Step,
  StepLabel,
  StepContent,
  Typography,
  CircularProgress,
  Button,
} from '@material-ui/core';
import {zipLookup} from '../services';
import autobind from 'class-autobind';
import {
  saveAlertOpenSelector,
  creatingAlertSelector,
  loadingOffersSelector,
  offersComparedSelector,
  matchingOffersCountSelector,
  alertIdSelector,
  saveErrorSelector,
} from '../selectors/addAlert';
import {closeAddAlert} from '../actions/addAlert';
import {CheckCircle} from '@material-ui/icons';

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
  stepIconProgress: {

  },
  checkIcon: {
    fontSize: '28px',
  }
});

class SaveAlertProgressModal extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  handleFieldChange(event) {
    const {name, value} = event.target;

    this.setState({[name]: value, touched: true});
  }

  handleZipChange(event) {
    const {value} = event.target;

    this.setState({zip: value});

    if (value && value.length === 5) {
      const zipValue = zipLookup(value);

      this.setState({state: zipValue.state, stateCode: zipValue.stateCode, touched: true});
    } else {
      this.setState({state: null, stateCode: null, touched: false});
    }
  }

  onClose() {
    this.props.closeAddAlert();
  }

  renderStepIcon(active) {
    const {classes} = this.props;
    return active ? <CircularProgress size={28} className={classes.stepIconProgress} /> : <CheckCircle color='primary' className={classes.checkIcon} />;
  }

  renderStepper() {
    const {creatingAlert, loadingOffers, offersCompared, saveError} = this.props;
    const savingAlertActive = !creatingAlert;
    const loadingOffersActive = creatingAlert && !loadingOffers;
    const comparingOffersActive = creatingAlert && !offersCompared;

    return (
      <Stepper orientation="vertical">
        <Step active={savingAlertActive} completed={creatingAlert}>
          {!creatingAlert && <StepLabel icon={this.renderStepIcon(savingAlertActive)} error={savingAlertActive && saveError}>
            Saving your alert
          </StepLabel>}
          {creatingAlert && <StepLabel>Saving your alert</StepLabel>}
          <StepContent>
            <Typography variant="body1">
              We&apos;re taking everything you told us and writing it down so our hard working bunnies know what to look for later.
            </Typography>
          </StepContent>
        </Step>
        <Step active={loadingOffersActive} completed={loadingOffers}>
          {!loadingOffers && loadingOffersActive && <StepLabel icon={this.renderStepIcon(loadingOffersActive)} error={loadingOffersActive && saveError}>
            Loading offers for your area
          </StepLabel>}
          {loadingOffers && <StepLabel>Loaded offers for your area</StepLabel>}
          {savingAlertActive && <StepLabel>Loading offers for your area</StepLabel>}
          <StepContent>
            <Typography variant="body1">
              Depending on if we&apos;ve ever gathered info for your specific area before, we may need to go get it first before doin&apos; our thing.
            </Typography>
          </StepContent>
        </Step>
        <Step active={comparingOffersActive} completed={offersCompared}>
          {!offersCompared && comparingOffersActive && <StepLabel icon={this.renderStepIcon(comparingOffersActive)} error={comparingOffersActive && saveError}>
            Comparing offers that match your alert
          </StepLabel>}
          {offersCompared && <StepLabel>Compared offers that match your alert</StepLabel>}
          {savingAlertActive && <StepLabel>Comparing offers that match your alert</StepLabel>}
          <StepContent>
            <Typography variant="body1">
              Using our trained and adorable bunnies, we&apos;re scouring the offers available to you and figure out if any of them are what you&apos;re looking for!
            </Typography>
          </StepContent>
        </Step>
      </Stepper>
    );
  }

  renderResultText() {
    const {matchingOffers, alertId} = this.props;

    const hasOffers = matchingOffers > 0;

    return (
      <Grid container>
        <Grid item>
          <Typography variant="h5">
            Super! Alert Saved!
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
    const {classes, open, creatingAlert, loadingOffers, offersCompared} = this.props;
    const showText = creatingAlert && loadingOffers && offersCompared;

    return (
      <Modal open={open} onClose={this.onClose} className={classes.modal}>
        <div className={classes.paper}>
          {this.renderStepper()}
          {showText && this.renderResultText()}
        </div>
      </Modal>
    );
  }
}

function mapStateToProps(state) {
  return {
    open: saveAlertOpenSelector(state),
    creatingAlert: creatingAlertSelector(state),
    loadingOffers: loadingOffersSelector(state),
    offersCompared: offersComparedSelector(state),
    matchingOffers: matchingOffersCountSelector(state),
    alertId: alertIdSelector(state),
    saveError: saveErrorSelector(state),
  };
}

const mapDispatchToProps = {
  closeAddAlert,
};

export default withStyles(styles)(connect(mapStateToProps, mapDispatchToProps)(SaveAlertProgressModal));
