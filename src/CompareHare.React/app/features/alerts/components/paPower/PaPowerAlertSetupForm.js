import React from 'react';
import PropTypes from 'prop-types';
import {
  Typography,
  Button,
  withStyles,
  Grid,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
} from '@material-ui/core';
import PaPowerSteps from './PaPowerSteps';
import {NavigateNext} from '@material-ui/icons';
import autobind from 'class-autobind';

const styles = theme => ({
  paper: {
    margin: 'auto',
    transform: 'translate(0 30%)',
    width: theme.spacing.unit * 51,
    backgroundColor: theme.palette.background.paper,
    boxShadow: theme.shadows[5],
    padding: theme.spacing.unit * 4,
    outline: 'none',
  },
  stepper: {
    paddingTop: '20px',
    paddingBottom: '10px',
  },
  dynamicGridCell: {
    height: '72px',
  },

})

class PaPowerAlertSetupForm extends React.PureComponent {
  constructor(props) {
    super(props);

    autobind(this);
  }

  onCheckPress(field, currentValue) {
    this.props.setFieldValue(field, !currentValue);
  }

  hasError(field) {
    const {touched, errors} = this.props;

    return Boolean(touched[field]) && Boolean(errors[field]);
  }

  getErrorText(field) {
    const {touched, errors} = this.props;

    return touched[field] ? errors[field] : null;
  }

  renderButtons() {
    const {handleSubmit, onClose, isValid} = this.props;

    return (
      <Grid container>
        <Grid item xs={9}>
          <Button
            type="button"
            variant="outlined"
            onClick={onClose}>
            Cancel
          </Button>
        </Grid>
        <Grid item xs={2}>
          <Button
            type="submit"
            variant="contained"
            color="primary"
            onClick={handleSubmit}
            disabled={!isValid}>
              Next
              <NavigateNext />
          </Button>
        </Grid>
      </Grid>
    );
  }

  renderDistributorOptions() {
    return this.props.distributors.map(option => {
      return <MenuItem key={option.id} value={option.id}>{option.name}</MenuItem>
    });
  }

  renderDistributorRateOptions() {
    return this.props.distributorRates.map(option => {
      return <MenuItem key={option.rateSchedule} value={option.rateSchedule}>{option.rateSchedule}</MenuItem>
    })
  }

  shouldRenderDistributorRates() {
    return this.props.distributorRates && this.props.distributorRates.length > 0;
  }

  getDefaultDistributor() {
    const { values, distributors, setFieldValue, onDistributorChange } = this.props;
    const { distributorId } = values;

    if (distributorId || (!distributors || distributors.length === 0)) {
      return values.distributorId;
    }

    setFieldValue('distributorId', distributors[0].id);
    onDistributorChange(distributors[0].id);

    return distributors[0].id;
  }

  getDefaultDistributorRate() {
    const { values, distributorRates, setFieldValue } = this.props;
    const { distributorRate } = values;

    if (distributorRate && distributorRate.length > 2 || (!distributorRates || distributorRates.length ===0)) {
      return values.distributorRate;
    }

    setFieldValue('distributorRate', distributorRates[0].rateSchedule);

    return distributorRates[0].rateSchedule;
  }

  handleDistributorChange(element) {
    const {
      setFieldValue,
      onDistributorChange
    } = this.props;

    // Need to inform reducer dist ID has changed, those need placed into the tree so we can grab them

    onDistributorChange(element.target.value);
    setFieldValue('distributorId', element.target.value);
  }

  render() {
    const {
      classes,
      handleChange,
      handleBlur,
      isNew,
    } = this.props;

    return (
      <form className={classes.paper}>
        <Typography variant="h5">
          {isNew ? 'New' : 'Edit'} Alert: PA Power
        </Typography>

        <PaPowerSteps
          stepperClass={classes.stepper}
          activeStepIndex={0} />

        <Typography>
          To start, select your distributor and rate schedule below:
        </Typography>

        <Grid container>
          <Grid item xs={12} className={classes.dynamicGridCell}>
            <FormControl margin="normal" required fullWidth>
              <InputLabel htmlFor="distributorId">Select your distributor</InputLabel>
              <Select
                inputProps={{
                  name: 'distributorId',
                  id: 'distributorId',
                }}
                value={this.getDefaultDistributor()}
                onChange={this.handleDistributorChange}
                onBlur={handleBlur('distributorId')}
              >
                {this.renderDistributorOptions()}
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12} className={classes.dynamicGridCell}>
            <FormControl margin="normal" required fullWidth>
              <InputLabel htmlFor="distributorRate">and rate schedule</InputLabel>
              <Select
                inputProps={{
                  name: 'distributorRate',
                  id: 'distributorRate',
                }}
                value={this.getDefaultDistributorRate()}
                onChange={handleChange('distributorRate')}
                onBlur={handleBlur('distributorRate')}
              >
                {this.renderDistributorRateOptions()}
              </Select>
            </FormControl>
          </Grid>
        </Grid>
        {this.renderButtons()}
      </form>
    )
  }
}

PaPowerAlertSetupForm.propTypes = {
  isNew: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  handleSubmit: PropTypes.func.isRequired,
  handleBlur: PropTypes.func.isRequired,
  handleChange: PropTypes.func.isRequired,
  setFieldValue: PropTypes.func.isRequired,
  values: PropTypes.object.isRequired,
  errors: PropTypes.object.isRequired,
  touched: PropTypes.object.isRequired,
  isSubmitting: PropTypes.bool.isRequired,
  isValid: PropTypes.bool.isRequired,
  classes: PropTypes.object.isRequired,
  distributors: PropTypes.array,
  onDistributorChange: PropTypes.func.isRequired
};

export default withStyles(styles)(PaPowerAlertSetupForm);
