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
  InputAdornment,
} from '@material-ui/core';
import {NavigateNext} from '@material-ui/icons';
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
  },
  stepper: {
    paddingTop: '20px',
    paddingBottom: 0,
  },
  dynamicGridCell: {
    height: '72px',
  }
})

class PaPowerAlert1stForm extends React.Component {
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
        <PaPowerSteps
          stepperClass={classes.stepper}
          activeStepIndex={1} />
        <TextField
          id="name"
          label="Name"
          className={classes.name}
          fullWidth
          value={values.name}
          onChange={handleChange('name')}
          onBlur={handleBlur('name')}
          margin="normal"
          placeholder="New Alert 1"
          autoFocus
          error={this.hasError('name')}
          helperText={this.getErrorText('name')} />
        <Typography>
          Now, tell us what kind of offers you want us to notify you about:
        </Typography>
        <Grid container>
          <Grid item xs={6} className={classes.dynamicGridCell}>
            <FormControlLabel
              control={
                <Switch
                  checked={values.hasMinimumPrice}
                  onChange={handleChange('hasMinimumPrice')}
                  value={values.hasMinimumPrice}
                  color="primary"
                />
              }
              label="Minimum price"
            />
          </Grid>
          <Grid item xs={6} className={classes.dynamicGridCell}>
            {values.hasMinimumPrice && <TextField
              id="minimumPrice"
              label="Minimum Price"
              value={values.minimumPrice}
              onChange={handleChange('minimumPrice')}
              onBlur={handleBlur('minimumPrice')}
              error={this.hasError('minimumPrice')}
              helperText={this.getErrorText('minimumPrice')}
              InputProps={{
                startAdornment: <InputAdornment position="start">$</InputAdornment>,
                endAdornment: <InputAdornment position="end">/kWh</InputAdornment>
              }}
              margin="normal" />}
          </Grid>
          <Grid item xs={6} className={classes.dynamicGridCell}>
            <FormControlLabel
              control={
                <Switch
                  checked={values.hasMaximumPrice}
                  onChange={handleChange('hasMaximumPrice')}
                  value={values.hasMaximumPrice}
                  color="primary"
                />
              }
              label="Maximum price"
            />
          </Grid>
          <Grid item xs={6} className={classes.dynamicGridCell}>
            {values.hasMaximumPrice && <TextField
              id="maximumPrice"
              label="Maximum Price"
              value={values.maximumPrice}
              onChange={handleChange('maximumPrice')}
              onBlur={handleBlur('maximumPrice')}
              error={this.hasError('maximumPrice')}
              helperText={this.getErrorText('maximumPrice')}
              InputProps={{
                startAdornment: <InputAdornment position="start">$</InputAdornment>,
                endAdornment: <InputAdornment position="end">/kWh</InputAdornment>
              }}
              margin="normal" />}
          </Grid>
          <Grid item xs={6} className={classes.dynamicGridCell}>
            <FormControlLabel
              control={
                <Switch
                  checked={values.hasMinimumMonthLength}
                  onChange={handleChange('hasMinimumMonthLength')}
                  value={values.hasMinimumMonthLength}
                  color="primary"
                />
              }
              label="Min month length"
            />
          </Grid>
          <Grid item xs={6} className={classes.dynamicGridCell}>
            {values.hasMinimumMonthLength && <TextField
              id="minimumMonthLength"
              label="Min Month Length"
              value={values.minimumMonthLength}
              onChange={handleChange('minimumMonthLength')}
              onBlur={handleBlur('minimumMonthLength')}
              error={this.hasError('minimumMonthLength')}
              helperText={this.getErrorText('minimumMonthLength')}
              InputProps={{
                endAdornment: <InputAdornment position="end">mos.</InputAdornment>
              }}
              margin="normal" />}
          </Grid>
          <Grid item xs={6} className={classes.dynamicGridCell}>
            <FormControlLabel
              control={
                <Switch
                  checked={values.hasMaximumMonthLength}
                  onChange={handleChange('hasMaximumMonthLength')}
                  value={values.hasMaximumMonthLength}
                  color="primary"
                />
              }
              label="Max month length"
            />
          </Grid>
          <Grid item xs={6} className={classes.dynamicGridCell}>
            {values.hasMaximumMonthLength && <TextField
              id="maximumMonthLength"
              label="Max Month Length"
              value={values.maximumMonthLength}
              onChange={handleChange('maximumMonthLength')}
              onBlur={handleBlur('maximumMonthLength')}
              error={this.hasError('maximumMonthLength')}
              helperText={this.getErrorText('maximumMonthLength')}
              InputProps={{
                endAdornment: <InputAdornment position="end">mos.</InputAdornment>
              }}
              margin="normal" />}
          </Grid>
        </Grid>
        {this.renderButtons()}
      </form>
    )
  }
}

PaPowerAlert1stForm.propTypes = {
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

export default withStyles(styles)(PaPowerAlert1stForm);
