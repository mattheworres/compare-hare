
import React from 'react';
import PropTypes from 'prop-types';
import {
  Typography,
  Button,
  TextField,
  withStyles,
  Grid,
  InputAdornment,
} from '@material-ui/core';
import {NavigateNext, Check, Clear} from '@material-ui/icons';
import autobind from 'class-autobind';

const styles = theme => ({
  paper: {
    margin: 'auto',
    transform: 'translate(0 30%)',
    width: theme.spacing.unit * 71,
    backgroundColor: theme.palette.background.paper,
    boxShadow: theme.shadows[5],
    padding: theme.spacing.unit * 4,
    outline: 'none',
    overflowY: 'scroll'
  },
  goodDate: {
    color: 'green'
  },
  priceDate: {
    width: '50%',
    paddingBottom: '20px'
  },
  price: {
    width: '50%',
    paddingBottom: '20px'
  }
})

class AddManualPriceForm extends React.PureComponent {
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

  handleDateChange(e) {
    const { handleChange, checkManualDate, trackedProductRetailerId } = this.props;
    const { value } = e.target;
    handleChange(e);
    checkManualDate({trackedProductRetailerId, priceDate: value});
  }

  renderError() {
    const {saveError} = this.props;

    const errorDisplay = Object.keys(saveError).map((errorKey, index) => {
      return (<Typography key={index} color="error"><strong>{errorKey}</strong>: {saveError[errorKey][0]}</Typography>);
    });

    return (
      <Typography color="error">
        <strong>Whoops, looks like theres an issue:</strong>
        {errorDisplay}
      </Typography>
    )
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
              Add
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
      touched,
      dateCheck,
      saveError
    } = this.props;

    const GOOD_DATE = <Check className={classes.goodDate} />
    const BAD_DATE = <Clear color="error" />

    const dateTouched = Boolean(touched['priceDate']);
    const dateIcon = dateCheck ? GOOD_DATE : BAD_DATE;

    // TODO: DRY up step creation
    // TODO: DRY up common form handlers, renderButton methods across multiple components & features

    return (
      <form className={classes.paper}>
        <Typography variant="h5">
          Add Manual Price
        </Typography>
        <Typography>
          Know what price this item was on a certain date? Enter it now!
        </Typography>
        <Grid container>
          <Grid item xs={12}>
            <TextField
              id="priceDate"
              label="Date"
              className={classes.priceDate}
              type="date"
              autoComplete="off"
              value={values.priceDate}
              onChange={handleChange('priceDate')}
              onBlur={handleBlur('priceDate')}
              margin="normal"
              error={this.hasError('priceDate')}
              helperText={this.getErrorText('priceDate')}
              InputProps={{
                onChange: this.handleDateChange,
                endAdornment: dateTouched ? <InputAdornment position="end">{dateIcon}</InputAdornment> : null
              }} />
          </Grid>
          <Grid item xs={12}>
            <TextField
                id="price"
                label="Price"
                value={values.price}
                className={classes.price}
                onChange={handleChange('price')}
                onBlur={handleBlur('price')}
                error={this.hasError('price')}
                helperText={this.getErrorText('price')}
                InputProps={{
                  startAdornment: <InputAdornment position="start">$</InputAdornment>
                }}
                margin="normal" />
          </Grid>
        </Grid>
        {saveError && this.renderError()}
        {this.renderButtons()}
      </form>
    )
  }
}

AddManualPriceForm.propTypes = {
  dateCheck: PropTypes.bool,
  checkManualDate: PropTypes.func.isRequired,
  saveError: PropTypes.object.isRequired,
  trackedProductRetailerId: PropTypes.string.isRequired,
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

export default withStyles(styles)(AddManualPriceForm);
