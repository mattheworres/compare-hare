import React from 'react';
import {connect} from 'react-redux';
import {
  Modal,
  withStyles,
} from '@material-ui/core';
import autobind from 'class-autobind';
import AddProduct4thForm from './AddProduct4thForm';
import {
  addOpen4thSelector,
  loadingSelector,
  loadErrorSelector,
  productNameSelector,
  productRetailersSelector,
  savingSelector
} from '../../selectors/addProduct';
import {closeAddProduct, openAddProduct2NewRetailer, saveProduct} from '../../actions/addProduct';
import PropTypes from 'prop-types';
import toastr from 'toastr';
import {LoadingModal} from '../../../shared/components';

const styles = () => ({
  modal: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
  },
});

class AddProduct4thModal extends React.PureComponent {
  constructor(props) {
    super(props);

    autobind(this);
  }

  onAddAnotherRetailer() {
    this.props.openAddProduct2NewRetailer();
  }

  onClose() {
    this.props.closeAddProduct();
  }

  handleSubmit() {
    const { saveProduct, productName, productRetailers } = this.props;

    saveProduct({
      name: productName,
      productRetailers: productRetailers.toJS()
    }, () => {
      toastr.success(`Success! ${productName} was saved!`);
    });
  }

  render() {
    const {
      classes,
      open,
      saving,
      productName,
      productRetailers
    } = this.props;

    return (
      <Modal open={open} onClose={this.onClose} className={classes.modal} >
        <>
          <AddProduct4thForm
            productName={productName}
            productRetailers={productRetailers.toJS()}
            handleSubmit={this.handleSubmit}
            onAddAnotherRetailer={this.onAddAnotherRetailer}
            onClose={this.onClose} />
          <LoadingModal open={saving} message="Saving product..." />
        </>
      </Modal>
    );
  }
}

AddProduct4thModal.propTypes = {
  classes: PropTypes.object.isRequired,
}

function mapStateToProps(state) {
  return {
    open: addOpen4thSelector(state),
    loading: loadingSelector(state),
    loadError: loadErrorSelector(state),
    productName: productNameSelector(state),
    productRetailers: productRetailersSelector(state),
    saving: savingSelector(state)
  };
}

const mapDispatchToProps = {
  closeAddProduct,
  openAddProduct2NewRetailer,
  saveProduct,
};

export default withStyles(styles)(connect(mapStateToProps, mapDispatchToProps)(AddProduct4thModal));
