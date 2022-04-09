import React from 'react';
import autobind from 'class-autobind';
import {Button, Card, CardActions, CardContent, CircularProgress, Fab, Paper, Table, TableBody, TableCell, TableFooter, TableHead, TableRow, Typography, withStyles} from '@material-ui/core';
import {connect} from 'react-redux';
import {Link} from 'react-router-dom';
import {loadProducts} from '../actions/productsTable';
import {openAddProduct} from '../actions/addProduct';
import {
  loadingSelector,
  productsSelector,
  deletingSelector,
  hasErrorSelector
} from '../selectors/productsTable';
import {
  loadingSelector as loadingAddSelector
} from '../selectors/addProduct';
import {Delete, Edit, Add} from '@material-ui/icons';
import PropTypes from 'prop-types';
import {LoadingModal} from '../../shared/components';

const styles = theme => ({
  root: {
    width: '100%',
    marginTop: theme.spacing.unit * 3,
    overflowX: 'auto'
  }
});

class ProductsTable extends React.PureComponent {
  constructor(props) {
    super(props);

    autobind(this);

    this.state = {
      productId: null
    };
  }

  componentDidMount() {
    this.props.loadProducts();
  }

  handleAddClick() {
    this.props.openAddProduct();
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

  renderProduct(product) {
    const enabledText = product.enabled ? 'Enabled' : 'Disabled';
    const retailers = product.retailers && product.retailers.length > 0 ? product.retailers.join(', ') : '(no retailers selected)';
    const url = `/products/${product.id}/display`;

    return (
      <TableRow key={product.id}>
        <TableCell><Link to={url}>{product.name}</Link></TableCell>
        <TableCell>{enabledText}</TableCell>
        <TableCell>{retailers}</TableCell>
        <TableCell align="right">
          <Fab size="medium" color="primary"  data-product-id={product.id}><Edit /></Fab>&nbsp;
          <Fab size="medium" color="secondary"  data-product-id={product.id}><Delete /></Fab>
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
              <TableCell>Product Name</TableCell>
              <TableCell>Enabled</TableCell>
              <TableCell>Retailers</TableCell>
              <TableCell>&nbsp;</TableCell>
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
  };
}

const mapDispatchToProps = {
  loadProducts,
  openAddProduct,
};

export default connect(mapStateToProps, mapDispatchToProps)(withStyles(styles)(ProductsTable));
