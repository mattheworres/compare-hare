
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
  MenuItem,
  Select,
} from '@material-ui/core';
import {NavigateNext} from '@material-ui/icons';
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
  variableTitle: {
    overflowX: 'hidden'
  },
  stepper: {
    paddingTop: '20px',
    paddingBottom: '20px',
  },
  productRetailer: {
    width: '50%',
    marginTop: '20px',
    marginBottom: '20px'
  },
  standardRetailer: {
    paddingTop: '20px',
    paddingBottom: '20px',
    height: '110px'
  },
  otherRetailerDisplayName: {
    width: '50%',
    marginBottom: '20px',
    marginTop: '10px'
  }
});

const OTHER_RETAILER_ID = 1001;

class AddProduct2ndForm extends React.PureComponent {
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

  renderProductRetailerOptions() {  
    const list = this.props.productRetailerOptions.toJS();
    return list.map(option =>
      <MenuItem key={option.value} value={option.value}>{option.label}</MenuItem>
    );
  }

  renderRetailerOtherName() {
    const {
      values,
      handleChange,
      handleBlur,
      productRetailerOptions,
      classes
    } = this.props;
    const {productRetailer} = values;
    const list = productRetailerOptions.toJS();

    const retailerIsOther = productRetailer === OTHER_RETAILER_ID;
    const retailerName = !retailerIsOther
      ? list.filter(r => parseInt(r.value, 10) === productRetailer)[0].label
      : 'Other';

    return retailerIsOther
      ? <>
        <Typography>
          No problem! In order to keep things straight (for you, again - the bunnies can&apos;t read!) <strong><br />
          Go ahead and provide a name for this retailer</strong> so you remember where the prices are coming from.
        </Typography>
        <TextField
          id="otherRetailerDisplayName"
          label="Retailers Name"
          className={classes.otherRetailerDisplayName}
          onChange={handleChange('otherRetailerDisplayName')}
          onBlur={handleBlur('otherRetailerDisplayName')}
          margin="normal"
          placeholder="Bill's Appliance Barn"
          autoComplete="off"
          error={this.hasError('otherRetailerDisplayName')}
          helperText={this.getErrorText('otherRetailerDisplayName')} />
      </>
      : <Typography className={classes.standardRetailer}>
        Good to go! We&apos;ll ask you for the web address of the product page on <strong>{retailerName}&apos;s</strong> site next.
      </Typography>
  }

  renderButtons() {
    const {handleSubmit, isValid} = this.props;

    return ( //TODO: actually wire up back instead of cancel
      <Grid container>
        <Grid item xs={9}>
          {/* <Button
            type="button"
            variant="outlined"
            >
            Back
          </Button> */}
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
      productName
    } = this.props;

    // TODO: DRY up step creation
    // TODO: DRY up common form handlers, renderButton methods across multiple components & features

    return (
      <form className={classes.paper}>
        <Typography variant="h5" className={classes.variableTitle}>
          Adding <em>{productName}</em>
        </Typography>
        <Stepper className={classes.stepper}>
          <Step completed>
            <StepLabel>Product Name</StepLabel>
          </Step>
          <Step active>
            <StepLabel>Pick New Retailer</StepLabel>
          </Step>
          <Step>
            <StepLabel>Retailer Details</StepLabel>
          </Step>
          <Step>
            <StepLabel>Review</StepLabel>
          </Step>
        </Stepper>
        <Typography className={classes.subText}>
          Let&apos;s add a retailer that our bunnies will check the price of {productName} at:
        </Typography>
        
        <Grid container>
          <Select
              className={classes.productRetailer}
              value={values.productRetailer}
              onChange={handleChange}
              onBlur={handleBlur}
              inputProps={{
                name: 'productRetailer',
                id: 'productRetailer',
              }}>
              {this.renderProductRetailerOptions()}
          </Select>
          {values.productRetailer ? this.renderRetailerOtherName() : null}
        </Grid>
        {this.renderButtons()}
      </form>
    )
  }
}

AddProduct2ndForm.propTypes = {
  productName: PropTypes.string.isRequired,
  productRetailerOptions: PropTypes.arrayOf(PropTypes.object).isRequired,
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

export default withStyles(styles)(AddProduct2ndForm);
