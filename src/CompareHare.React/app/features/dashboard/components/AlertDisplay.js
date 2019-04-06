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
  priceParamDisabled: {
    opacity: '0.25',
  }
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

    const hasFlatRate = price.flatRate !== null;
    const hasCancellationFee = price.hasCancellationFee !== null;
    const hasMonthlyFee = price.hasMonthlyFee !== null;
    const hasEnrollmentFee = price.hasEnrollmentFee !== null;
    const hasNetMetering = price.hasNetMetering !== null;
    const requiresDeposit = price.requiresDeposit !== null;
    const hasBulkDiscounts = price.hasBulkDiscounts !== null;
    const isIntroductoryPrice = price.isIntroductoryPrice !== null;
    const hasRenewable = price.hasRenewable !== null;
    const hasTermMonthLength = price.termMonthLength !== null;
    const hasTermEndDate = price.hasTermEndDate !== null && price.hasTermEndDate;

    return (
      <Card key={price.id} className={classes.price} style={{flex: `0 1 calc(${width} - 1em)`}}>
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
            <Grid item xs={6} className={hasFlatRate ? null : classes.priceParamDisabled}>
              <Typography variant="body1">
                <strong>Flat Rate: </strong>
                &nbsp;{this.renderNullableParameter(hasFlatRate, hasFlatRate, `${price.flatRate}`)}
              </Typography>
            </Grid>
            <Grid item xs={12} className={hasCancellationFee ? null : classes.priceParamDisabled}>
              <Typography variant="body1">
                <strong>Cancellation Fee: </strong>
                &nbsp;{this.renderNullableParameter(hasCancellationFee, price.hasCancellationFee, price.cancellationFee)}
              </Typography>
            </Grid>
            <Grid item xs={6} className={hasCancellationFee ? null : classes.priceParamDisabled}>
              <Typography variant="body1">
                <strong>Monthly Fee: </strong>
                &nbsp;{this.renderNullableParameter(hasMonthlyFee, price.hasMonthlyFee, price.monthlyFee)}
              </Typography>
            </Grid>
            <Grid item xs={6} className={hasEnrollmentFee ? null : classes.priceParamDisabled}>
              <Typography variant="body1">
                <strong>Enrollment Fee: </strong>
                &nbsp;{this.renderNullableParameter(hasEnrollmentFee, price.hasEnrollmentFee, price.enrollmentFee)}
              </Typography>
            </Grid>
            <Grid item xs={6} className={hasNetMetering ? null : classes.priceParamDisabled}>
              <Typography variant="body1">
                <strong>Net Metering: </strong>
                &nbsp;{this.renderNullableParameter(hasNetMetering, price.hasNetMetering, price.netMetering)}
              </Typography>
            </Grid>
            <Grid item xs={6} className={requiresDeposit ? null : classes.priceParamDisabled}>
              <Typography variant="body1">
                <strong>Requires Deposit: </strong>
                &nbsp;{this.renderNullableParameter(requiresDeposit, price.requiresDeposit, 'Yes')}
              </Typography>
            </Grid>
            <Grid item xs={6} className={hasBulkDiscounts ? null : classes.priceParamDisabled}>
              <Typography variant="body1">
                <strong>Bulk Discounts: </strong>
                &nbsp;{this.renderNullableParameter(hasBulkDiscounts, price.hasBulkDiscounts, 'Yes')}
              </Typography>
            </Grid>
            <Grid item xs={6} className={isIntroductoryPrice ? null : classes.priceParamDisabled}>
              <Typography variant="body1">
                <strong>Is Introductory: </strong>
                &nbsp;{this.renderNullableParameter(isIntroductoryPrice, price.isIntroductoryPrice, 'Yes')}
              </Typography>
            </Grid>
            <Grid item xs={6} className={hasRenewable ? null : classes.priceParamDisabled}>
              <Typography variant="body1">
                <strong>Renewable Energy: </strong>
                &nbsp;{this.renderNullableParameter(hasRenewable, price.hasRenewable, `${price.renewablePercentage}%`)}
              </Typography>
            </Grid>
            <Grid item xs={6} className={hasTermMonthLength ? null : classes.priceParamDisabled}>
              <Typography variant="body1">
                <strong>Term Length: </strong>
                &nbsp;{this.renderNullableParameter(hasTermMonthLength, price.termMonthLength, `${price.termMonthLength} mos.`)}
              </Typography>
            </Grid>
            <Grid item xs={6} className={hasTermEndDate ? null : classes.priceParamDisabled}>
              <Typography variant="body1">
                <strong>Term End Date: </strong>
                &nbsp;{this.renderNullableParameter(hasTermEndDate, price.hasTermEndDate, moment(price.termEndDate || '12/12/1900').format('MMMM Do YYYY'))}
              </Typography>
            </Grid>
          </Grid>
        </CardContent>
        <CardActions>
          <Button href={`tel:${price.supplierPhone}`} target="_blank">Call</Button>
          <Button href={price.offerUrl} target="_blank">Visit</Button>
        </CardActions>
      </Card>
    );
  }

  renderNullableParameter(hasField, parameter, evaluatedTrueFieldValue) {
    if (!hasField) return <>&ndash;</>;

    return parameter
      ? evaluatedTrueFieldValue
      : 'No';
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
