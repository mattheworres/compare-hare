import React from 'react';
import autobind from 'class-autobind';
import {Paper, withStyles} from '@material-ui/core';
import {connect} from 'react-redux';
import {loadProducts} from '../actions/productsTable';
import {
  loadingSelector,
  productsSelector,
  deletingSelector
} from '../selectors/productsTable';
import PropTypes from 'prop-types';

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

  render() {
    const { classes, products } = this.props;
    return (// left it here; need to finish reducers & check to see that API call succeeds
      <Paper className={classes.root}>Hello! We have {products.length} produvcts!</Paper>
    );
  }
}

ProductsTable.propTypes = {
  classes: PropTypes.object
};

function mapStateToProps(state) {
  return {
    loading: loadingSelector(state),
    deleting: deletingSelector(state),
    products: productsSelector(state),
  };
}

const mapDispatchToProps = {
  loadProducts,
};

export default connect(mapStateToProps, mapDispatchToProps)(withStyles(styles)(ProductsTable));
