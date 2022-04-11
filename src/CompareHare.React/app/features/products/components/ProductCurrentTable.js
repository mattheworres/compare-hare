import React from 'react';
import autobind from 'class-autobind';
import {Button, Card, CardActions, CardContent, CircularProgress, Fab, ListItemIcon, ListItemText, Menu, MenuItem, Paper, Table, TableBody, TableCell, TableFooter, TableHead, TableRow, Typography, withStyles} from '@material-ui/core';
import {connect} from 'react-redux';
// import {loadProductCurrent} from '../actions/productDisplay';
import {
  loadingSelector,
  productSelector,
  deletingSelector,
  hasErrorSelector
} from '../selectors/productDisplay';
import {Delete, Edit, Add, AddCircle, CheckCircle, Close, ToggleOff, ToggleOn, ArrowDropUp, ArrowDropDown, MoreVert} from '@material-ui/icons';
import PropTypes from 'prop-types';
import moment from 'moment';
import {retrieveAttributeValue} from '../../shared/services/displayHelpers';

const ENABLED_ICON = <CheckCircle color="primary" />
const DISABLED_ICON = <Close color="disabled" />
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
      productId: null,
      menuAnchor: null,
      menuRetailerId: null
    };
  }

  openRetailerMenu(event) {
    const retailerId = retrieveAttributeValue(event, 'data-tracked-product-retailer-id');
    this.setState({
      menuAnchor: event.currentTarget,
      menuRetailerId: retailerId
    })
  }

  closeRetailerMenu() {
    this.setState({
      menuAnchor: null,
      menuRetailerId: null
    });
  }

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

  renderMenu() {
    const {menuAnchor, menuRetailerId} = this.state;
    const open = Boolean(menuAnchor && menuRetailerId);
    let retailer;
    let retailerEnabled = false;

    if (open) {
      const {productRetailers} = this.props.product || {};
      retailer = productRetailers.filter(r => r.id === menuRetailerId)[0];
      retailerEnabled = retailer && retailer.enabled;
    }

    return (
      <Menu
        id="retailer-menu"
        anchorEl={menuAnchor}
        open={open}
        onClose={this.closeRetailerMenu}
        PaperProps={{
          style: {
            maxHeight: 216,
            width: 225
          }
        }}
        >
          <MenuItem>
            <ListItemIcon><Add /></ListItemIcon>
            <ListItemText inset primary="Add Data Manually" />
          </MenuItem>
          <MenuItem disabled>
            <ListItemIcon>{retailerEnabled ? <ToggleOff /> : <ToggleOn />}</ListItemIcon>
            <ListItemText inset primary={retailerEnabled ? 'Disable' : 'Enable'} />
          </MenuItem>
          <MenuItem disabled>
            <ListItemIcon><Edit /></ListItemIcon>
            <ListItemText inset primary="Edit" />
          </MenuItem>
          <MenuItem disabled>
            <ListItemIcon><Delete /></ListItemIcon>
            <ListItemText inset primary="Delete" />
          </MenuItem>
      </Menu>
    )
  }

  renderRetailer(retailer) {
    const { classes } = this.props;
    const UP_ICON = <ArrowDropUp className={classes.negative} />
    const DOWN_ICON = <ArrowDropDown className={classes.positive} />
    const emptyDisplay = <>&mdash;</>;
    const enabledIcon = retailer.enabled ? ENABLED_ICON : DISABLED_ICON;
    let tableData;

    if (retailer.lastUpdated !== null) {
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

      tableData = <>
        <TableCell>{moment(retailer.lastUpdated || '12/12/1900').format('MMMM Do YYYY')}</TableCell>
        <TableCell align="right">{priceDisplay}</TableCell>
        <TableCell align="right">{amountChangeDisplay}</TableCell>
        <TableCell align="right">{percentChangeDisplay}</TableCell>
      </>;
    } else {
      tableData = <>
        <TableCell colSpan={4}>
          <Typography color="textSecondary">
            Oopy daisy, our bunnies haven&apos;t hopped out to check {retailer.retailerName} yet. Hang tight!
          </Typography>
        </TableCell>
      </>
    }

    return (
      <TableRow key={retailer.id}>
        <TableCell>{enabledIcon}</TableCell>
        <TableCell>{retailer.retailerName}</TableCell>
        {tableData}
        <TableCell align="right">
          <Fab size="small" color="default" data-tracked-product-retailer-id={retailer.trackedProductRetailerId} onClick={this.openRetailerMenu}><MoreVert /></Fab>
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
              <Button size="small" color="primary"><AddCircle />&nbsp;Add a new retailer</Button>
            </TableCell>
          </TableRow>
        </TableFooter>
        </Table>
        {this.renderMenu()}
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
