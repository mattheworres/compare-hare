
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
  priceSelector: {
    width: '50%',
    marginTop: '20px',
    marginBottom: '20px'
  },
  otherText: {
    paddingTop: '20px',
  },
  scrapeUrl: {
    width: '100%',
    marginBottom: '20px',
    marginTop: '10px'
  }
});

class AddProduct3rdForm extends React.PureComponent {
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
      productName,
      newProductRetailer,
    } = this.props;

    const {
      isOtherRetailer,
      productRetailerDisplayName
    } = newProductRetailer.toJS();

    // TODO: DRY up step creation
    // TODO: DRY up common form handlers, renderButton methods across multiple components & features

    return (
      <form className={classes.paper}>
        <Typography variant="h5" className={classes.variableTitle}>
          {productRetailerDisplayName} Details
        </Typography>
        <Stepper className={classes.stepper}>
          <Step completed>
            <StepLabel>Product Name</StepLabel>
          </Step>
          <Step completed>
            <StepLabel>Pick New Retailer</StepLabel>
          </Step>
          <Step active>
            <StepLabel>Retailer Details</StepLabel>
          </Step>
          <Step>
            <StepLabel>Review</StepLabel>
          </Step>
        </Stepper>
        <Typography className={classes.subText}>
          So our bunnies don&apos;t get confused, you need to provide the web URL where the price of <em>{productName}</em> is listed at {productRetailerDisplayName}:
        </Typography>
        
        <Grid container>
          <TextField
            id="scrapeUrl"
            label="Product Price URL"
            className={classes.scrapeUrl}
            onChange={handleChange('scrapeUrl')}
            onBlur={handleBlur('scrapeUrl')}
            margin="normal"
            placeholder="https://billsappliancebarn.com/product-price-page"
            autoComplete="off"
            error={this.hasError('scrapeUrl')}
            helperText={this.getErrorText('scrapeUrl')} />
          {isOtherRetailer ? <>
            <Typography className={classes.otherText}>
              Since {productRetailerDisplayName} isn&apos;t a retailer our bunnies are trained to hop to, we need you to tell us where on the page to get the product price, with a <a href="https://developer.mozilla.org/en-US/docs/Learn/CSS/Building_blocks/Selectors" target="_blank">CSS Selector</a>:
            </Typography>
            <TextField
              id="priceSelector"
              label="Price CSS Selector"
              className={classes.priceSelector}
              onChange={handleChange('priceSelector')}
              onBlur={handleBlur('priceSelector')}
              margin="normal"
              placeholder="div.price-box div.priceView-hero-price.priceView-customer-price"
              autoComplete="off"
              error={this.hasError('priceSelector')}
              helperText={this.getErrorText('priceSelector')} />
              </> : null}
        </Grid>
        {this.renderButtons()}
      </form>
    )
  }
}

AddProduct3rdForm.propTypes = {
  productName: PropTypes.string.isRequired,
  newProductRetailer: PropTypes.object.isRequired,
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

export default withStyles(styles)(AddProduct3rdForm);
