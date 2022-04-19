import React from 'react';
import autobind from 'class-autobind';
import {
  Button,
  Card,
  CardActions,
  CardContent,
  CircularProgress,
  Fab,
  ListItemIcon,
  ListItemText,
  Menu,
  MenuItem,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableFooter,
  TableHead,
  TableRow,
  Typography,
  withStyles
} from '@material-ui/core';
import {connect} from 'react-redux';
import {openAddManual} from '../actions/addManual';
import {toggleProductRetailer} from '../actions/productDisplay';
import {
  loadingSelector,
  productSelector,
  deletingSelector,
  hasErrorSelector,
  togglingSelector,
  toggleErrorSelector
} from '../selectors/productDisplay';
import {
  Delete,
  Edit,
  Add,
  AddCircle,
  CheckCircle,
  Close,
  ToggleOff,
  ToggleOn,
  MoreVert
} from '@material-ui/icons';
import PropTypes from 'prop-types';
import moment from 'moment';
import {retrieveAttributeValue, printMoney} from '../../shared/services/displayHelpers';
import AddManualPriceModal from './addManual/AddManualPriceModal';
import PriceChangeDisplay from '../../shared/components/PriceChangeDisplay';

const ENABLED_ICON = <CheckCircle color="primary" />
const DISABLED_ICON = <Close color="disabled" />

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
    color: '#4caf50',
    verticalAlign: 'bottom'
  },
  negative: {
    color: '#f44336',
    verticalAlign: 'bottom'
  },
  emptyRow: {
    textAlign: 'center'
  }
});

class ProductCurrentTable extends React.PureComponent {
  constructor(props) {
    super(props);

    autobind(this);

    this.state = {
      productId: null,
      menuAnchor: null,
      menuRetailerId: null,
      menuRetailerName: null,
      menuRetailerEnabled: false
    };
  }

  componentDidUpdate(prevProps) {
    const {toggling, toggleError, loadProductCurrent, trackedProductId} = this.props;
    if (!toggling && toggling != prevProps.toggling && !toggleError) {
      loadProductCurrent(trackedProductId);
    }
  }

  handleAddPriceClick() {
    const { product } = this.props;
    this.props.openAddManual(product.trackedProductId, this.state.menuRetailerId)
    this.closeRetailerMenu();
  }

  handleToggle() {
    const { menuRetailerId, menuRetailerEnabled } = this.state;
    this.props.toggleProductRetailer(menuRetailerId, !menuRetailerEnabled);
    this.closeRetailerMenu();
  }

  openRetailerMenu(event) {
    const retailerId = retrieveAttributeValue(event, 'data-tracked-product-retailer-id');
    const {productRetailers} = this.props.product || {};

    const retailer = productRetailers.filter(r => r.trackedProductRetailerId == retailerId)[0];

    this.setState({
      menuAnchor: event.currentTarget,
      menuRetailerId: retailerId,
      menuRetailerEnabled: retailer && retailer.enabled,
      menuRetailerName: retailer && retailer.retailerName
    })
  }

  closeRetailerMenu() {
    this.setState({
      menuAnchor: null,
      menuRetailerId: null,
      menuRetailerName: null,
      menuRetailerEnabled: false
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
    const {menuAnchor, menuRetailerId, menuRetailerEnabled} = this.state;
    const open = Boolean(menuAnchor && menuRetailerId);

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
          <MenuItem onClick={this.handleAddPriceClick}>
            <ListItemIcon><Add /></ListItemIcon>
            <ListItemText inset primary="Add Price (manual)" />
          </MenuItem>
          <MenuItem onClick={this.handleToggle}>
            <ListItemIcon>{menuRetailerEnabled ? <ToggleOff /> : <ToggleOn />}</ListItemIcon>
            <ListItemText inset primary={menuRetailerEnabled ? 'Disable' : 'Enable'} />
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
    
    const emptyDisplay = <>&mdash;</>;
    const {
      price,
      percentChange,
      amountChange,
      lastUpdated,
      retailerName,
      trackedProductRetailerId,
      scrapeUrl
    } = retailer;
    const enabledIcon = retailer.enabled ? ENABLED_ICON : DISABLED_ICON;
    let tableData;

    if (lastUpdated !== null) {
      const priceDisplay = price ? <Typography variant="h6" color="primary">{printMoney(price)}</Typography> : emptyDisplay;

      tableData = <>
        <TableCell>{moment(lastUpdated || '12/12/1900').format('MMMM Do YYYY')}</TableCell>
        <TableCell align="right">{priceDisplay}</TableCell>
        <TableCell align="right"><PriceChangeDisplay amountChange={amountChange} percentChange={percentChange} /></TableCell>
      </>;
    } else {
      tableData = <>
        <TableCell colSpan={3}>
          <Typography color="textSecondary" className={classes.emptyRow}>
            Oopy daisy, our bunnies haven&apos;t hopped out to check {retailerName} yet. Hang tight!
          </Typography>
        </TableCell>
      </>
    }

    const retailerNameAnchor = <a href={scrapeUrl} target='_blank'>{retailerName}</a>;

    return (
      <TableRow key={trackedProductRetailerId}>
        <TableCell>{enabledIcon}</TableCell>
        <TableCell>{retailerNameAnchor}</TableCell>
        {tableData}
        <TableCell align="right">
          <Fab size="small" color="default"
            data-tracked-product-retailer-id={trackedProductRetailerId}
            onClick={this.openRetailerMenu}><MoreVert /></Fab>
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
        <AddManualPriceModal />
      </Paper>
    );
  }
}

ProductCurrentTable.propTypes = {
  classes: PropTypes.object,
  loadProductCurrent: PropTypes.func.isRequired,
  trackedProductId: PropTypes.string,
};

function mapStateToProps(state) {
  return {
    loading: loadingSelector(state),
    deleting: deletingSelector(state),
    product: productSelector(state),
    hasError: hasErrorSelector(state),
    toggling: togglingSelector(state),
    toggleError: toggleErrorSelector(state)
  };
}

const mapDispatchToProps = {
  openAddManual,
  toggleProductRetailer,
};

export default connect(mapStateToProps, mapDispatchToProps)(withStyles(styles)(ProductCurrentTable));
