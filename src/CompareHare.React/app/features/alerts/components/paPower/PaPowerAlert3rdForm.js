
import React from 'react';
import PropTypes from 'prop-types';
import {
  Typography,
  Button,
  TextField,
  withStyles,
  Grid,
  ListItemText,
} from '@material-ui/core';
import autobind from 'class-autobind';
import PaPowerSteps from './PaPowerSteps';

const styles = theme => ({
  paper: {
    margin: 'auto',
    transform: 'translate(0 30%)',
    width: theme.spacing.unit * 51,
    backgroundColor: theme.palette.background.paper,
    boxShadow: theme.shadows[5],
    padding: theme.spacing.unit * 4,
    outline: 'none',
    overflowY: 'scroll'
  },
  stepper: {
    paddingTop: '20px',
    paddingBottom: '10px',
  },
  mainGridContainer: {
    marginBottom: '15px',
  },
  subText: {
    marginBottom: '15px',
  },
  conditionalWrapper: {
    padding: '8px',
  },
  rightAligned: {
    textAlign: 'right',
  },
})

class PaPowerAlert3rdForm extends React.Component {
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

  renderButtons() {
    const {handleSubmit, onClose, errors, classes} = this.props;
    const isValid = Object.keys(errors).filter(key => errors[key] !== undefined).length === 0;

    return (
      <Grid container>
        <Grid item xs={7}>
          <Button
            type="button"
            variant="outlined"
            onClick={onClose}>
            Cancel
          </Button>
        </Grid>
        <Grid item xs={5} className={classes.rightAligned}>
          <Button
            type="button"
            variant="contained"
            color="primary"
            onClick={handleSubmit}
            disabled={!isValid}>
              Save Alert
          </Button>
        </Grid>
      </Grid>
    );
  }

  renderTextField(field, label, width) {
    const {alertModel} = this.props;
    const value = alertModel[field];

    return (
      <Grid item xs={width}>
        <ListItemText primary={value} secondary={label} />
      </Grid>
    );
  }

  renderPriceFields() {
    const {
      hasMinimumPrice,
      hasMaximumPrice,
      minimumPrice,
      maximumPrice,
    } = this.props.alertModel;

    if (!hasMinimumPrice && !hasMaximumPrice) return null;

    let text = '';

    if (hasMinimumPrice && !hasMaximumPrice) {
      text = `Prices above $${minimumPrice}/kWh`;
    } else if (hasMinimumPrice && hasMaximumPrice) {
      text = `Prices between $${minimumPrice} and $${maximumPrice}/kWh`;
    } else if (!hasMinimumPrice && hasMaximumPrice) {
      text = `Prices below $${maximumPrice}/kWh`;
    }

    return (
      <Grid item xs={12}>
        <ListItemText primary={text} secondary='Filter by Price' />
      </Grid>
    );
  }

  renderMonthFields() {
    const {
      hasMinimumMonthLength,
      hasMaximumMonthLength,
      minimumMonthLength,
      maximumMonthLength,
    } = this.props.alertModel;

    if (!hasMinimumMonthLength && !hasMaximumMonthLength) return null;

    let text = '';

    if (hasMinimumMonthLength && !hasMaximumMonthLength) {
      text = `At least ${minimumMonthLength} month long contracts`;
    } else if (hasMinimumMonthLength && hasMaximumMonthLength) {
      text = `Contracts between ${minimumMonthLength} and ${maximumMonthLength} months long`;
    } else if (!hasMinimumMonthLength && hasMaximumMonthLength) {
      text = `Contracts no longer than ${maximumMonthLength} months long`;
    }

    return (
      <Grid item xs={12}>
        <ListItemText primary={text} secondary='Filter by Contract Length' />
      </Grid>
    );
  }

  renderRenewableFields() {
    const {
      filterRenewable,
      hasRenewable,
      minimumRenewablePercent,
      maximumRenewablePercent,
    } = this.props.alertModel;

    if (!filterRenewable) return null;

    let text = '';

    if (!hasRenewable) {
      text = 'No offers with any renewable energy sources';
    } else if (minimumRenewablePercent != 0 || maximumRenewablePercent != 100) {
      text = `Only offers with ${minimumRenewablePercent}% - ${maximumRenewablePercent}% of renewable energy sources`;
    } else {
      text = `Only offers with any amount of renewable energy sources`;
    }

    return (
      <Grid item xs={12}>
        <ListItemText primary={text} secondary='Filter by Renewable Energy' />
      </Grid>
    );
  }

  renderConditionalFields() {
    const {
      filterCancellationFee,
      hasCancellationFee,
      filterMonthlyFee,
      hasMonthlyFee,
      filterEnrollmentFee,
      hasEnrollmentFee,
      filterRequiresDeposit,
      requiresDeposit,
      filterBulkDiscounts,
      hasBulkDiscounts,
    } = this.props.alertModel;

    if (!filterCancellationFee && !filterMonthlyFee && !filterEnrollmentFee && !filterRequiresDeposit && !filterBulkDiscounts) return null;

    return (
      <Grid container className={this.props.classes.conditionalWrapper} spacing={16}>
      {filterCancellationFee && (<Grid item xs={12}>
        <ListItemText
            primary={`Only offers with ${hasCancellationFee ? null : 'NO '} cancellation fees`}
            secondary="Cancellation Fees" />
      </Grid>)}
      {filterMonthlyFee && (<Grid item xs={12}>
        <ListItemText
          primary={`Only offers with ${hasMonthlyFee ? null : 'NO '} monthly fees`}
          secondary="Monthly Fees" />
      </Grid>)}
      {filterEnrollmentFee && (<Grid item xs={12}>
        <ListItemText
          primary={`Only offers with ${hasEnrollmentFee ? null : 'NO '} monthly fees`}
          secondary="Enrollment Fees" />
      </Grid>)}
      {filterRequiresDeposit && (<Grid item xs={12}>
        <ListItemText
          primary={`Only offers with ${requiresDeposit ? null : 'NO '} deposits required`}
          secondary="Required Deposits" />
      </Grid>)}
      {filterBulkDiscounts && (<Grid item xs={12}>
        <ListItemText
          primary={`Only offers with ${hasBulkDiscounts ? null : 'NO '} bulk discounts`}
          secondary="Bulk Discounts" />
      </Grid>)}
    </Grid>
    );
  }

  render() {
    const {
      classes,
      isNew,
      handleChange,
      handleBlur,
      values,
    } = this.props;

    return (
      <form className={classes.paper}>
        <Typography variant="h5">
          {isNew ? 'New' : 'Edit'} Alert: PA Power
        </Typography>
        <PaPowerSteps
          stepperClass={classes.stepper}
          activeStepIndex={3} />
        <Typography variant="h6" className={classes.subText}>
          Take a moment and review your alert before saving it:
        </Typography>
        <Grid container spacing={16} className={classes.mainGridContainer}>
          {this.renderTextField('name', 'Alert Name', 6)}
          {this.renderTextField('zip', 'Service Zip', 6)}
          <Grid item xs={6}>
            <ListItemText primary='Pennsylvania' secondary='State' />
          </Grid>
          <Grid item xs={6}>
            <ListItemText primary='Power' secondary='Utility' />
          </Grid>
          {this.renderPriceFields()}
          {this.renderMonthFields()}
          {this.renderRenewableFields()}
          {this.renderConditionalFields()}
          <Grid item xs={12}>
            <Typography variant="h6">
              Feel free to jot down personal notes here, help future you remember what you were thinking:
            </Typography>
            <TextField
              className={classes.dynamicSecondaryElement}
              id="comments"
              label="Comments"
              value={values.comments}
              placeholder="I want to make sure I dont miss any great promotional rates on renewable energy..."
              onChange={handleChange('comments')}
              onBlur={handleBlur('comments')}
              error={this.hasError('comments')}
              helperText={this.getErrorText('comments')}
              multiline
              fullWidth
              rows={4}
              margin="normal" />
          </Grid>
        </Grid>
        {this.renderButtons()}
      </form>
    )
  }
}

PaPowerAlert3rdForm.propTypes = {
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
  alertModel: PropTypes.object.isRequired,
};

export default withStyles(styles)(PaPowerAlert3rdForm);
