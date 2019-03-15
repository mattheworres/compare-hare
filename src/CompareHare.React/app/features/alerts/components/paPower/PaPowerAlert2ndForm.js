import React from 'react';
import PropTypes from 'prop-types';
import {
  Typography,
  Button,
  TextField,
  FormControlLabel,
  withStyles,
  Switch,
  Grid,
  Stepper,
  Step,
  StepLabel,
  Select,
  MenuItem,
  InputAdornment,
} from '@material-ui/core';
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
  dynamicSecondaryCell: {
    paddingTop: 0,
  },
  dynamicSecondaryElement: {
    marginTop: 0,
  }
})

class PaPowerAlert2ndForm extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  hasError(field) {
    const {touched, errors} = this.props;

    return Boolean(touched[field]) && Boolean(errors[field]);
  }

  getErrorText(field) {
    const {touched, errors} = this.props;

    return touched[field] ? errors[field] : null;
  }

  onCheckChange(field, currentValue) {
    this.props.setFieldValue(field, !currentValue);
  }

  renderButtons() {
    const {handleSubmit, onClose, errors} = this.props;
    const isValid = Object.keys(errors).filter(key => errors[key] !== undefined).length === 0;

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

  renderYesNoOptions() {
    const yesNoOptions = [
      {value: true, text: 'Yes'},
      {value: false, text: 'No'}
    ];

    return yesNoOptions.map((option, index) => <MenuItem key={index} value={option.value}>{option.text}</MenuItem>);
  }

  render() {
    const {
      classes,
      handleChange,
      handleBlur,
      values,
      isNew,
    } = this.props;

    return (
      <form className={classes.paper}>
        <Typography variant="h5">
          {isNew ? 'New' : 'Edit'} Alert: PA Power
        </Typography>
        <Stepper className={classes.stepper}>
          <Step completed>
            <StepLabel>Name &amp; Price</StepLabel>
          </Step>
          <Step active>
            <StepLabel>Flags</StepLabel>
          </Step>
          <Step>
            <StepLabel>Review</StepLabel>
          </Step>
        </Stepper>
        <Grid container spacing={16}>
          <Grid item xs={6}>
            <FormControlLabel
              control={
                <Switch
                  checked={values.filterRenewable}
                  onChange={handleChange('filterRenewable')}
                  value={values.filterRenewable}
                  color="primary"
                />
              }
              label="Renewable"
            />
          </Grid>
          <Grid item xs={6}>
            {values.filterRenewable && <Select
              value={values.hasRenewable}
              onChange={() => this.onCheckChange('hasRenewable', values.hasRenewable)}
              inputProps={{
                name: 'hasRenewable',
                id: 'hasRenewable',
              }}>
              {this.renderYesNoOptions()}
            </Select>}
          </Grid>
          {values.filterRenewable && values.hasRenewable && <Grid item xs={6}>
            <TextField
              className={classes.dynamicSecondaryElement}
              id="minimumRenewablePercent"
              label="Min Renewable"
              value={values.minimumRenewablePercent}
              onChange={handleChange('minimumRenewablePercent')}
              onBlur={handleBlur('minimumRenewablePercent')}
              error={this.hasError('minimumRenewablePercent')}
              helperText={this.getErrorText('minimumRenewablePercent')}
              InputProps={{
                endAdornment:<InputAdornment position="end" >%</InputAdornment>,
              }}
          margin="normal" />
          </Grid>}
          {values.filterRenewable && values.hasRenewable && <Grid item xs={6}>
            <TextField
              className={classes.dynamicSecondaryElement}
              id="maximumRenewablePercent"
              label="Max Renewable"
              value={values.maximumRenewablePercent}
              onChange={handleChange('maximumRenewablePercent')}
              onBlur={handleBlur('maximumRenewablePercent')}
              error={this.hasError('maximumRenewablePercent')}
              helperText={this.getErrorText('maximumRenewablePercent')}
              InputProps={{
                endAdornment: <InputAdornment position="end" >%</InputAdornment>,
              }}
              margin="normal" />
          </Grid>}
          <Grid item xs={6}>
            <FormControlLabel
              control={
                <Switch
                  checked={values.filterCancellationFee}
                  onChange={handleChange('filterCancellationFee')}
                  value={values.filterCancellationFee}
                  color="primary"
                />
              }
              label="Cancellation Fees"
            />
          </Grid>
          <Grid item xs={6}>
            {values.filterCancellationFee && <Select
              value={values.hasCancellationFee}
              onChange={() => this.onCheckChange('hasCancellationFee', values.hasCancellationFee)}
              inputProps={{
                name: 'hasCancellationFee',
                id: 'hasCancellationFee',
              }}>
              {this.renderYesNoOptions()}
            </Select>}
          </Grid>
          <Grid item xs={6}>
            <FormControlLabel
              control={
                <Switch
                  checked={values.filterMonthlyFee}
                  onChange={handleChange('filterMonthlyFee')}
                  value={values.filterMonthlyFee}
                  color="primary"
                />
              }
              label="Monthly Fees"
            />
          </Grid>
          <Grid item xs={6}>
            {values.filterMonthlyFee && <Select
              value={values.hasMonthlyFee}
              onChange={() => this.onCheckChange('hasMonthlyFee', values.hasMonthlyFee)}
              inputProps={{
                name: 'hasMonthlyFee',
                id: 'hasMonthlyFee',
              }}>
              {this.renderYesNoOptions()}
            </Select>}
          </Grid>
          <Grid item xs={6}>
            <FormControlLabel
              control={
                <Switch
                  checked={values.filterEnrollmentFee}
                  onChange={handleChange('filterEnrollmentFee')}
                  value={values.filterEnrollmentFee}
                  color="primary"
                />
              }
              label="Enrollment Fees"
            />
          </Grid>
          <Grid item xs={6}>
            {values.filterEnrollmentFee && <Select
              value={values.hasEnrollmentFee}
              onChange={() => this.onCheckChange('hasEnrollmentFee', values.hasEnrollmentFee)}
              inputProps={{
                name: 'hasEnrollmentFee',
                id: 'hasEnrollmentFee',
              }}>
              {this.renderYesNoOptions()}
            </Select>}
          </Grid>
          <Grid item xs={6}>
            <FormControlLabel
              control={
                <Switch
                  checked={values.filterRequiresDeposit}
                  onChange={handleChange('filterRequiresDeposit')}
                  value={values.filterRequiresDeposit}
                  color="primary"
                />
              }
              label="Requires Deposit"
            />
          </Grid>
          <Grid item xs={6}>
            {values.filterRequiresDeposit && <Select
              value={values.requiresDeposit}
              onChange={() => this.onCheckChange('requiresDeposit', values.requiresDeposit)}
              inputProps={{
                name: 'requiresDeposit',
                id: 'requiresDeposit',
              }}>
              {this.renderYesNoOptions()}
            </Select>}
          </Grid>
          <Grid item xs={6}>
            <FormControlLabel
              control={
                <Switch
                  checked={values.filterBulkDiscounts}
                  onChange={handleChange('filterBulkDiscounts')}
                  value={values.filterBulkDiscounts}
                  color="primary"
                />
              }
              label="Bulk Discounts"
            />
          </Grid>
          <Grid item xs={6}>
            {values.filterBulkDiscounts && <Select
              value={values.hasBulkDiscounts}
              onChange={() => this.onCheckChange('hasBulkDiscounts', values.hasBulkDiscounts)}
              inputProps={{
                name: 'hasBulkDiscounts',
                id: 'hasBulkDiscounts',
              }}>
              {this.renderYesNoOptions()}
            </Select>}
          </Grid>
        </Grid>
        {this.renderButtons()}
      </form>
    )
  }
}

PaPowerAlert2ndForm.propTypes = {
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
};

export default withStyles(styles)(PaPowerAlert2ndForm);
