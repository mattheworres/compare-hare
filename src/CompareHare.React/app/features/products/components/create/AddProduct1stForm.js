
import React from 'react';
import PropTypes from 'prop-types';
import {
  Typography,
  Button,
  TextField,
  withStyles,
  Grid,
  Stepper,
  Step,
  StepLabel,
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
    overflowY: 'scroll'
  },
  stepper: {
    paddingTop: '20px',
    paddingBottom: 0,
  },
})

class AddProduct1stForm extends React.Component {
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
    } = this.props;

    // TODO: DRY up step creation
    // TODO: DRY up common form handlers, renderButton methods across multiple components & features

    return (
      <form className={classes.paper}>
        <Typography variant="h5">
          Add Product
        </Typography>
        <Stepper className={classes.stepper}>
          <Step active>
            <StepLabel>Product Name</StepLabel>
          </Step>
          <Step>
            <StepLabel>Pick New Retailer</StepLabel>
          </Step>
          <Step>
            <StepLabel>Retailer Details</StepLabel>
          </Step>
          <Step>
            <StepLabel>Review</StepLabel>
          </Step>
        </Stepper>
        <Typography variant="h6" className={classes.subText}>
          What is this product called?
        </Typography>
        <Typography>
           This is mostly for you, so you remember what the prices are for (Names? Our bunnies don&apos;t need stinkin&apos; names)
        </Typography>
        <Typography>
          ex.: <em>LG Wash Tower WKE100HWA</em>
        </Typography>
        <Grid container>
          <TextField
            id="name"
            label="Name"
            className={classes.name}
            fullWidth
            value={values.name}
            onChange={handleChange('name')}
            onBlur={handleBlur('name')}
            margin="normal"
            placeholder="LG Wash Tower WKE100HWA"
            autoFocus
            error={this.hasError('name')}
            helperText={this.getErrorText('name')} />
        </Grid>
        {this.renderButtons()}
      </form>
    )
  }
}

AddProduct1stForm.propTypes = {
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

export default withStyles(styles)(AddProduct1stForm);
