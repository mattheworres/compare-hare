import React from 'react';
import {connect} from 'react-redux';
import {
  Modal,
  withStyles,
} from '@material-ui/core';
import autobind from 'class-autobind';
import AddProduct4thForm from './AddProduct4thForm';
import {addOpen4thSelector, loadingSelector, loadErrorSelector, productNameSelector, productRetailersSelector } from '../../selectors/addProduct';
import {closeAddProduct, openAddProduct2NewRetailer, saveProduct} from '../../actions/addProduct';
import PropTypes from 'prop-types';

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
    });
  }

  render() {
    const {
      classes,
      open,
      errors,
      handleBlur,
      handleChange,
      setFieldValue,
      values,
      touched,
      isSubmitting,
      isValid,
      productName,
      productRetailers
    } = this.props;

    return (
      <Modal open={open} onClose={this.onClose} className={classes.modal} >
        <AddProduct4thForm
          productName={productName}
          productRetailers={productRetailers.toJS()}
          values={values}
          errors={errors}
          touched={touched}
          isSubmitting={isSubmitting}
          isValid={isValid}
          setFieldValue={setFieldValue}
          handleBlur={handleBlur}
          handleChange={handleChange}
          handleSubmit={this.handleSubmit}
          onAddAnotherRetailer={this.onAddAnotherRetailer}
          onClose={this.onClose} />
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
  };
}

const mapDispatchToProps = {
  closeAddProduct,
  openAddProduct2NewRetailer,
  saveProduct,
};

export default withStyles(styles)(connect(mapStateToProps, mapDispatchToProps)(AddProduct4thModal));
