import React from 'react';
import autobind from 'class-autobind';
import {Button, Card, CardActions, CardContent, CircularProgress, Fab, Paper, Table, TableBody, TableCell, TableFooter, TableHead, TableRow, Typography, withStyles} from '@material-ui/core';
import {connect} from 'react-redux';
// import {loadProductCurrent} from '../actions/productDisplay';
import {
  loadingSelector,
  productSelector,
  deletingSelector,
  hasErrorSelector
} from '../selectors/productDisplay';
import {Delete, Edit, Add, ToggleOn, ToggleOffOutlined, ArrowDropUp, ArrowDropDown} from '@material-ui/icons';
import PropTypes from 'prop-types';
import moment from 'moment';

const ENABLED_ICON = <ToggleOn color="primary" />
const DISABLED_ICON = <ToggleOffOutlined color="disabled" />
const printMoney = amount => amount.toLocaleString('en-US', { style: 'currency', currency: 'USD'});

const styles = theme => ({
  root: {
    ...theme.mixins.gutters(),
    width: '100%',
    marginTop: theme.spacing.unit * 3,
    overflowX: 'auto'
  },
  title: {
    margin: '20px'
  },
  product: {
    ...theme.mixins.gutters()
  },
  positive: {
    color: '#4caf50'
  },
  negative: {
    color: '#f44336'
  },

});

class ProductCurrentTable extends React.PureComponent {
  constructor(props) {
    super(props);

    autobind(this);

    this.state = {
      productId: null
    };
  }

  // componentDidMount() {
  //   const {match, loadProductCurrent} = this.props;
  //   const {trackedProductId} = match.params;

  //   loadProductCurrent(trackedProductId);
  // }

  renderLoading() {
    return (
      <Paper>
        <CircularProgress />
        <Typography variant="h5" className={this.props.paddedTypography}>Loading...</Typography>
      </Paper>
    );
  }

  renderEmptyContent() {
    const {classes} = this.props;
    return (
      <Card className={classes.card}>
        <CardContent>
          <Typography variant="h5" component="h2" className={this.props.paddedTypography}>
            You have no retailers for this tracked product!
          </Typography>
          <Typography component="p">Get started by clicking below!</Typography>
        </CardContent>
        <CardActions>
          <Button size="small" >Add one now!</Button>
        </CardActions>
      </Card>
    );
  }

  renderError() {
    return (
      <Paper>
        <Typography variant="h5" className={this.props.paddedTypography}>Whoops we can&apos;t load your product...</Typography>
      </Paper>
    );
  }

  renderRetailer(retailer) {
    const { classes } = this.props;
    const UP_ICON = <ArrowDropUp className={classes.negative} />
    const DOWN_ICON = <ArrowDropDown className={classes.positive} />
    const emptyDisplay = '&mdash;';
    
    const enabledIcon = retailer.enabled ? ENABLED_ICON : DISABLED_ICON;
    const priceDisplay = retailer.price ? <Typography variant="h6" color="primary">{printMoney(retailer.price)}</Typography> : emptyDisplay;
    const amountChangePositive = retailer.amountChange && retailer.amountChange > 0;
    const amountChangeDisplay = retailer.amountChange
      ? <Typography variant="h6" className={amountChangePositive ? classes.negative : classes.positive}>
          {amountChangePositive ? UP_ICON : DOWN_ICON}
          {printMoney(retailer.amountChange)}
        </Typography>
      : emptyDisplay;
    const percentChangePositive = retailer.percentChange && retailer.percentChange > 0;
    const percentChangeDisplay = retailer.percentChange
      ? <Typography variant="h6" className={percentChangePositive ? classes.negative : classes.positive}>
          {percentChangePositive ? UP_ICON : DOWN_ICON}
          {(retailer.percentChange * 100).toFixed(2)}%
        </Typography>
     : emptyDisplay;

    return (
      <TableRow key={retailer.id}>
        <TableCell>{enabledIcon}</TableCell>
        <TableCell>{retailer.retailerName}</TableCell>
        <TableCell>{moment(retailer.lastUpdated || '12/12/1900').format('MMMM Do YYYY')}</TableCell>
        <TableCell align="right">{priceDisplay}</TableCell>
        <TableCell align="right">{amountChangeDisplay}</TableCell>
        <TableCell align="right">{percentChangeDisplay}</TableCell>
        <TableCell align="right">
          <Fab size="medium" color="primary"  data-tracked-product-retailer-id={retailer.trackedProductRetailerId}><Edit /></Fab>&nbsp;
          <Fab size="medium" color="secondary"  data-tracked-product-retailer-id={retailer.trackedProductRetailerId}><Delete /></Fab>
        </TableCell>
      </TableRow>
    );
  }

  render() {
    const { classes, product, loading, /*loadingEdit, deleting,*/ hasError } = this.props;

    if (loading) return this.renderLoading();

    if (hasError) return this.renderError();

    if (!product || !product.productRetailers || product.productRetailers.length === 0) return this.renderEmptyContent();

    const enabledIcon = product.enabled ? ENABLED_ICON : DISABLED_ICON;

    return (
      <Paper className={classes.root}>
        <Typography variant="h4" className={classes.title}>
          {enabledIcon}&nbsp;&nbsp;{product.productName}
        </Typography>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>&nbsp;</TableCell>
              <TableCell>Retailer</TableCell>
              <TableCell>Last Updated</TableCell>
              <TableCell align="right">Price</TableCell>
              <TableCell align="right">Last change</TableCell>
              <TableCell align="right">Last change (%)</TableCell>
              <TableCell>&nbsp;</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>{product.productRetailers.map(this.renderRetailer)}</TableBody>
        <TableFooter>
          <TableRow>
            <TableCell colSpan={4}>
              <Button size="small" color="primary"><Add />&nbsp;Add a new product</Button>
            </TableCell>
          </TableRow>
        </TableFooter>
        </Table>
      </Paper>
    );
  }
}

ProductCurrentTable.propTypes = {
  classes: PropTypes.object
};

function mapStateToProps(state) {
  return {
    loading: loadingSelector(state),
    deleting: deletingSelector(state),
    product: productSelector(state),
    hasError: hasErrorSelector(state),
  };
}

const mapDispatchToProps = {
  // loadProductCurrent,
};

export default connect(mapStateToProps, mapDispatchToProps)(withStyles(styles)(ProductCurrentTable));
