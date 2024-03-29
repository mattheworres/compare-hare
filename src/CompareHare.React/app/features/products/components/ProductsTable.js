import React from 'react';
import autobind from 'class-autobind';
import {
  Button,
  Card,
  CardActions,
  CardContent,
  CircularProgress,
  Fab,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableFooter,
  TableHead,
  TableRow,
  Typography,
  Menu,
  MenuItem,
  ListItemIcon,
  ListItemText,
  withStyles
} from '@material-ui/core';
import {
  Delete,
  Edit,
  Add,
  CheckCircle,
  Close,
  MoreVert,
  ToggleOff,
  ToggleOn,
  ErrorOutline
} from '@material-ui/icons';
import {connect} from 'react-redux';
import {Link} from 'react-router-dom';
import {loadProducts, toggleProduct} from '../actions/productsTable';
import {openAddProduct} from '../actions/addProduct';
import {
  loadingSelector,
  productsSelector,
  deletingSelector,
  hasErrorSelector,
  togglingSelector,
  toggleErrorSelector
} from '../selectors/productsTable';
import {
  loadingSelector as loadingAddSelector
} from '../selectors/addProduct';
import PropTypes from 'prop-types';
import {LoadingModal} from '../../shared/components';
import {retrieveAttributeValue, printMoney} from '../../shared/services/displayHelpers';
import PriceChangeDisplay from '../../shared/components/PriceChangeDisplay';
import moment from 'moment';

const styles = theme => ({
  root: {
    width: '100%',
    marginTop: theme.spacing.unit * 3,
    overflowX: 'auto'
  },
  lowestPrice: {
    display: 'inline-block'
  },
  lowestPriceChange: {
    display: 'inline-block',
    marginLeft: '10px'
  }
});

const ENABLED_ICON = <CheckCircle color="primary" />;
const DISABLED_ICON = <Close color="disabled" />;
const EXCEPTION_ICON = <ErrorOutline color="error" />;

class ProductsTable extends React.PureComponent {
  constructor(props) {
    super(props);

    autobind(this);

    this.state = {
      productId: null,
      menuAnchor: null,
      menuProductId: null,
      menuProductEnabled: false
    };
  }

  openProductMenu(event) {
    const productId = retrieveAttributeValue(event, 'data-tracked-product-id');
    const {products} = this.props;
    const product = products ? products.filter(r => r.id == productId)[0] : null;

    this.setState({
      menuAnchor: event.currentTarget,
      menuProductId: productId,
      menuProductEnabled: product && product.enabled
    })
  }

  closeProductMenu() {
    this.setState({
      menuAnchor: null,
      menuProductId: null
    });
  }

  componentDidMount() {
    this.props.loadProducts();
  }

  componentDidUpdate(prevProps) {
    const {toggling, toggleError, loadProducts} = this.props;
    if (!toggling && toggling != prevProps.toggling && !toggleError) {
      loadProducts();
    }
  }

  handleAddClick() {
    this.props.openAddProduct();
  }

  handleProductToggle() {
    const { menuProductId, menuProductEnabled } = this.state;
    this.props.toggleProduct(menuProductId, !menuProductEnabled);
    this.closeProductMenu();
  }

  renderLoading() {
    return (
      <Paper>
        <CircularProgress />
        <Typography variant="h5" className={this.props.paddedTypography}>Loading...</Typography>
      </Paper>
    );
  }

  renderProductPrice(product) {
    const { classes } = this.props;
    const { lowPriceRetailerName, price, priceLastUpdated, amountChange, percentChange } = product;
    const emptyDisplay = <>&mdash;</>
    const retailerNameDisplay = price
      ? <Typography variant="caption" className={classes.lowestPriceRetailer}>{`@ ${lowPriceRetailerName} on ${moment(priceLastUpdated || '12/12/1900').format('MMMM Do YYYY')}`}</Typography>
      : null;

    return priceLastUpdated !== null ? <>
      <Typography variant="subheading" color="primary" className={classes.lowestPrice}>
        {price ? printMoney(price) : emptyDisplay}
      </Typography>
      <PriceChangeDisplay amountChange={amountChange} percentChange={percentChange} variant='' className={classes.lowestPriceChange} />
      {retailerNameDisplay}
    </> : emptyDisplay;
  }

  renderEmptyContent() {
    const {classes} = this.props;
    return (
      <Card className={classes.card}>
        <CardContent>
          <Typography variant="h5" component="h2" className={this.props.paddedTypography}>
            You have no tracked products!
          </Typography>
          <Typography component="p">Get started by clicking below!</Typography>
        </CardContent>
        <CardActions>
          <Button size="small" onClick={this.handleAddClick}>Add one now!</Button>
        </CardActions>
      </Card>
    );
  }

  renderError() {
    return (
      <Paper>
        <Typography variant="h5" className={this.props.paddedTypography}>Whoops we can&apos;t load your products...</Typography>
      </Paper>
    );
  }

  renderMenu() {
    const {menuAnchor, menuProductId, menuProductEnabled} = this.state;
    const open = Boolean(menuAnchor && menuProductId);

    return (
      <Menu
        id="product-menu"
        anchorEl={menuAnchor}
        open={open}
        onClose={this.closeProductMenu}
        PaperProps={{
          style: {
            maxHeight: 216,
            width: 225
          }
        }}
        >
          <MenuItem onClick={this.handleProductToggle}>
            <ListItemIcon>{menuProductEnabled ? <ToggleOff /> : <ToggleOn />}</ListItemIcon>
            <ListItemText inset primary={menuProductEnabled ? 'Disable' : 'Enable'} />
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

  renderProduct(product) {
    const enabledIcon = product.enabled ? ENABLED_ICON : DISABLED_ICON;
    const exceptionIcon = product.hasScrapingExceptions ? EXCEPTION_ICON : null;
    const url = `/products/${product.id}/display`;

    return (
      <TableRow key={product.id}>
        <TableCell padding="checkbox">{enabledIcon}{exceptionIcon}</TableCell>
        <TableCell><Link to={url}>{product.name}</Link></TableCell>
        <TableCell>{this.renderProductPrice(product)}</TableCell>
        <TableCell padding="checkbox" align="right">
          <Fab size="small" color="default"
            data-tracked-product-id={product.id}
            onClick={this.openProductMenu}><MoreVert /></Fab>
        </TableCell>
      </TableRow>
    );
  }

  render() {
    const { classes, products, loading, loadingAdd, hasError } = this.props;

    if (loading) return this.renderLoading();

    if (hasError) return this.renderError();

    if (products.length === 0) return this.renderEmptyContent();

    return (
      <Paper className={classes.root}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell padding="checkbox" variant="head">&nbsp;</TableCell>
              <TableCell variant="head">Product Name</TableCell>
              <TableCell variant="head">Lowest Price</TableCell>
              <TableCell padding="checkbox" variant="head">&nbsp;</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>{products.map(this.renderProduct)}</TableBody>
        <TableFooter>
          <TableRow>
            <TableCell colSpan={4}>
              <Button size="small" color="primary" onClick={this.handleAddClick}><Add />&nbsp;Add a new product</Button>
            </TableCell>
          </TableRow>
        </TableFooter>
        </Table>
        {this.renderMenu()}
        <LoadingModal open={loadingAdd} message="Loading retailers, one sec..." />
      </Paper>
    );
  }
}

ProductsTable.propTypes = {
  classes: PropTypes.object,
};

function mapStateToProps(state) {
  return {
    loading: loadingSelector(state),
    loadingAdd: loadingAddSelector(state),
    deleting: deletingSelector(state),
    products: productsSelector(state),
    hasError: hasErrorSelector(state),
    toggling: togglingSelector(state),
    toggleError: toggleErrorSelector(state)
  };
}

const mapDispatchToProps = {
  loadProducts,
  openAddProduct,
  toggleProduct
};

export default connect(mapStateToProps, mapDispatchToProps)(withStyles(styles)(ProductsTable));
