import React from 'react';
import {
  withStyles,
  Paper,
  Grid,
  CircularProgress,
  Typography,
  Chip,
  CardContent,
  CardActions,
  Button,
  Card,
} from '@material-ui/core';
import {connect} from 'react-redux';
import PropTypes from 'prop-types';
import {loadAlert} from '../actions/alertDisplay';
import autobind from 'class-autobind';
import {Page} from '../../layout/components';
import {pricesSelector, loadingSelector, hasErrorSelector, alertSelector} from '../selectors/alertDisplay';
import parametersSelector from '../selectors/alertDisplay/parametersSelector';
import moment from 'moment';

const styles = () => ({
  header: {
    padding: '15px',
  },
  parameterContainer: {
    marginTop: '10px',
  },
  parameter: {
    marginRight: '5px',
  },
  content: {
    padding: '15px',
  },
  priceContainer: {
    display: 'flex',
    flexDirection: 'row',
    justifyContent: 'space-around',
    flexWrap: 'wrap',
  },
  price: {
    marginBottom: '20px',
  },
});

class AlertDisplay extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  componentDidMount() {
    const {match, loadAlert} = this.props;
    const {alertId} = match.params;

    loadAlert(alertId);
  }

  renderLoading() {
    return (
      <Paper className={this.props.classes.content}>
        <CircularProgress />
        <Typography variant="h5">Loading...</Typography>
      </Paper>
    );
  }

  renderError() {
    return (
      <Paper className={this.props.classes.content}>
        <Typography variant="h5">Whoops, we can&apos;t load your alert...</Typography>
      </Paper>
    )
  }

  renderPrice(price) {
    const {classes} = this.props;

    let width = '33%';
    const screenWidth = window.innerWidth;

    if (screenWidth < 1200 && screenWidth > 768) {
      width = '50%';
    } else if (screenWidth < 768) {
      width = '100%';
    }

    let priceStructureColor = 'primary';

    if (price.priceStructure === 'Variable') priceStructureColor = 'default';
    if (price.priceStructure === 'Unlimited') priceStructureColor = 'secondary';

    return (
      <Card className={classes.price} style={{flex: `0 1 calc(${width} - 1em)`}}>
        <CardContent>
          <Grid container spacing={8}>
            <Grid item xs={8}>
              <Typography variant="h6">{price.name}</Typography>
            </Grid>
            <Grid item xs={4} style={{textAlign: 'right'}}>
              <Chip
                color={priceStructureColor}
                variant="outlined"
                label={`${price.priceStructure} price`}
              />
            </Grid>
            <Grid item xs={6}>
              <Typography variant="body1">
                <strong>Price:</strong>
                &nbsp;${price.pricePerUnit}/{price.priceUnit}
              </Typography>
            </Grid>
            {price.flatRate !== null && (<Grid item xs={6}>
              <Typography variant="body1">
                <strong>Flat Rate: </strong>
                &nbsp;${price.fatRate}
              </Typography>
            </Grid>)}
            {price.hasCancellationFee !== null && price.hasCancellationFee && (<Grid item xs={6}>
              <Typography variant="body1">
                <strong>Cancellation Fee: </strong>
                &nbsp;{price.cancellationFee}
              </Typography>
            </Grid>)}
            {price.hasMonthlyFee !== null && price.hasMonthlyFee && (<Grid item xs={6}>
              <Typography variant="body1">
                <strong>Monthly Fee: </strong>
                &nbsp;{price.monthlyFee}
              </Typography>
            </Grid>)}
            {price.hasEnrollmentFee !== null && price.hasEnrollmentFee && (<Grid item xs={6}>
              <Typography variant="body1">
                <strong>Enrollment Fee: </strong>
                &nbsp;{price.enrollmentFee}
              </Typography>
            </Grid>)}
            {price.hasNetMetering !== null && price.hasNetMetering && (<Grid item xs={6}>
              <Typography variant="body1">
                <strong>Net Metering: </strong>
                &nbsp;{price.netMetering}
              </Typography>
            </Grid>)}
            {price.requiresDeposit !== null && price.requiresDeposit && (<Grid item xs={6}>
              <Typography variant="body1">
                <strong>Requires Deposit: </strong> Yes
              </Typography>
            </Grid>)}
            {price.hasBulkDiscounts !== null && price.hasBulkDiscounts && (<Grid item xs={6}>
              <Typography variant="body1">
                <strong>Bulk Discounts: </strong> Yes
              </Typography>
            </Grid>)}
            {price.isIntroductoryPrice !== null && price.isIntroductoryPrice && (<Grid item xs={6}>
              <Typography variant="body1">
                <strong>Is Introductory: </strong> Yes
              </Typography>
            </Grid>)}
            {price.hasRenewable !== null && price.hasRenewable && (<Grid item xs={6}>
              <Typography variant="body1">
                <strong>Renewable Energy: </strong>
                &nbsp;{price.renewablePercentage}%
              </Typography>
            </Grid>)}
            {price.termMonthLength !== null && (<Grid item xs={6}>
              <Typography variant="body1">
                <strong>Term Length: </strong>
                &nbsp;{price.termMonthLength} mos.
              </Typography>
            </Grid>)}
            {price.hasTermEndDate !== null && price.hasTermEndDate && (<Grid item xs={6}>
              <Typography variant="body1">
                <strong>Term End Date: </strong>
                &nbsp;{moment(price.termEndDate).format('MMMM Do YYYY')}
              </Typography>
            </Grid>)}
          </Grid>
        </CardContent>
        <CardActions>
          <Button href={`tel:${price.supplierPhone}`} target="_blank">Call</Button>
          <Button href={price.offerUrl} target="_blank">Visit</Button>
        </CardActions>
      </Card>
    );
  }

  renderPrices() {
    const {prices, classes} = this.props;

    if (!prices || prices.length == 0) {
      return (
        <Paper className={classes.content}>
          <Typography variant="h5">Doesn&apos;t look like your alert has any matches, yet...</Typography>
        </Paper>
      )
    }

    return prices.map(this.renderPrice);
  }

  renderParameter(param) {
    return (
      <Chip className={this.props.classes.parameter}
        key={param.name}
        color="primary"
        label={`${param.name}: ${param.value}`}
      />
    );
  }

  renderHeader() {
    const {alert, parameters, classes} = this.props;

    return (
      <Paper className={classes.header}>
        <Grid container>
          <Grid item xs={6}>
            <Typography variant="h5">{alert.name}</Typography>
          </Grid>
          <Grid item xs={4}>
            <Typography variant="h6" align="center">{alert.utilityState} {alert.utilityType}</Typography>
          </Grid>
          <Grid item xs={2}>
            <Typography variant="h6">{alert.loaderIdentifier}</Typography>
          </Grid>
          <Grid item xs={12} className={classes.parameterContainer}>
            {parameters.map(this.renderParameter)}
          </Grid>
        </Grid>
      </Paper>
    );
  }

  render() {
    const {loading, hasError, classes} = this.props;

    if (loading) return this.renderLoading();

    if (hasError) return this.renderError();

    return (
      <Page>
        <Grid container spacing={16}>
          <Grid item xs={false} lg={1} />
          <Grid item xs={12} sm={12} md={12} lg={10}>
            {this.renderHeader()}
          </Grid>
          <Grid item xs={false} lg={1} />
          <Grid item xs={12} sm={12} md={12} lg={12} justifyContent="space-evenly" alignContent="space-evenly" className={classes.priceContainer}>
            {this.renderPrices()}
          </Grid>
        </Grid>
      </Page>
    );
  }
}

AlertDisplay.propTypes = {
  classes: PropTypes.object,
};

function mapStateToProps(state) {
  return {
    loading: loadingSelector(state),
    hasError: hasErrorSelector(state),
    alert: alertSelector(state),
    prices: pricesSelector(state),
    parameters: parametersSelector(state),
  };
}

const mapDispatchToProps = {
  loadAlert,
}

export default connect(mapStateToProps, mapDispatchToProps)(withStyles(styles)(AlertDisplay));
