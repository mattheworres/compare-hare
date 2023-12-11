
import React from 'react';
import PropTypes from 'prop-types';
import {
  Typography,
  Button,
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
  retailerContainer: {
    display: 'flex',
    marginTop: '20px',
    marginBottom: '20px',
    justifyContent: 'center'
  },
  retailerItem: {
    width: '45%',
    textAlign: 'center',
    fontSize: '0.9rem',
    fontWeight: '500',
    alignSelf: 'center',
    color: 'white',
    backgroundColor: theme.palette.primary.main,
    height: '28px',
    borderRadius: '14px',
    marginLeft: '5px',
    marginRight: '5px',
    marginBottom: '10px',
    paddingTop: '7px'
  },
  addRetailerButton: {
    
  },
  submitButton: {
    display: 'flex',
    marginLeft: 'auto'
  }
});

class AddProduct4thForm extends React.PureComponent {
  constructor(props) {
    super(props);

    autobind(this);
  }

  renderRetailer(retailer, idx) {
    return (
      <Typography key={idx} className={this.props.classes.retailerItem}>
        {retailer.productRetailerDisplayName}
      </Typography>
    );
  }

  renderRetailersList() {
    const { classes, productRetailers } = this.props;
    
    return (
      <Grid container className={classes.retailerContainer}>
        {productRetailers.map(this.renderRetailer)}
      </Grid>
    )
  }

  renderButtons() {
    const {handleSubmit, classes, onAddAnotherRetailer} = this.props;

    return (
      <Grid container>
        <Grid item xs={7}>
          <Button className={classes.addRetailerButton}
            type="button"
            variant="contained"
            color="secondary"
            onClick={onAddAnotherRetailer}
          >
            + Add Retailer
          </Button>
        </Grid>
        <Grid item xs={5}>
          <Button
            type="button"
            variant="contained"
            color="primary"
            className={classes.submitButton}
            onClick={handleSubmit}>
              Save Product
              <NavigateNext />
          </Button>
        </Grid>
      </Grid>
    );
  }

  render() {
    const {
      classes,
      productName,
      productRetailers
    } = this.props;

    // TODO: DRY up step creation
    // TODO: DRY up common form handlers, renderButton methods across multiple components & features

    const retailersWording = productRetailers.length === 1
      ? 'this 1 retailer'
      : `these ${productRetailers.length} retailers`;

    return (
      <form className={classes.paper}>
        <Typography variant="h5" className={classes.variableTitle}>
          Review: {productName}
        </Typography>
        <Stepper className={classes.stepper}>
          <Step completed>
            <StepLabel>Product Name</StepLabel>
          </Step>
          <Step completed>
            <StepLabel>Pick New Retailer</StepLabel>
          </Step>
          <Step completed>
            <StepLabel>Retailer Details</StepLabel>
          </Step>
          <Step active>
            <StepLabel>Review</StepLabel>
          </Step>
        </Stepper>
        <Typography className={classes.subText}>
          Splendid! Looks like you&apos;ve given our bunnies all they need to do some price hoppin&apos; on your behalf.
        </Typography>
        <Typography>
          Look over this list of {retailersWording} to make sure it looks good (or click &quot;Add Retailer&quot; below!)
        </Typography>
        {this.renderRetailersList()}
        {this.renderButtons()}
      </form>
    )
  }
}

AddProduct4thForm.propTypes = {
  productName: PropTypes.string.isRequired,
  productRetailers: PropTypes.array.isRequired,
  onClose: PropTypes.func.isRequired,
  onAddAnotherRetailer: PropTypes.func.isRequired,
  handleSubmit: PropTypes.func.isRequired,
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(AddProduct4thForm);
