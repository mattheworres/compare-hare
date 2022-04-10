import React from 'react';
import {connect} from 'react-redux';
import {
  Modal,
  withStyles,
} from '@material-ui/core';
import autobind from 'class-autobind';
import AddProduct3rdForm from './AddProduct3rdForm';
import {addOpen3rdSelector, loadingSelector, loadErrorSelector, productNameSelector, newProductRetailerSelector } from '../../selectors/addProduct';
import {closeAddProduct, openAddProduct4} from '../../actions/addProduct';
import {addProduct3rdFormDefaultValues, addProduct3rdFormValidationSchema} from '../../validations/AddProductValidations';
import {withFormik} from 'formik';
import PropTypes from 'prop-types';

const styles = () => ({
  modal: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
  },
});

class AddProduct3rdModal extends React.PureComponent {
  constructor(props) {
    super(props);

    autobind(this);
  }

  onClose() {
    const {closeAddProduct, resetForm} = this.props;
    resetForm();
    closeAddProduct();
  }

  render() {
    const {
      classes,
      open,
      errors,
      handleBlur,
      handleChange,
      handleSubmit,
      setFieldValue,
      values,
      touched,
      isSubmitting,
      isValid,
      productName,
      newProductRetailer
    } = this.props;

    return (
      <Modal open={open} onClose={this.onClose} className={classes.modal} >
        <AddProduct3rdForm
          productName={productName}
          newProductRetailer={newProductRetailer}
          values={values}
          errors={errors}
          touched={touched}
          isSubmitting={isSubmitting}
          isValid={isValid}
          setFieldValue={setFieldValue}
          handleBlur={handleBlur}
          handleChange={handleChange}
          handleSubmit={handleSubmit}
          onClose={this.onClose} />
      </Modal>
    );
  }
}

AddProduct3rdModal.propTypes = {
  classes: PropTypes.object.isRequired,
}

const mapPropsToValues = () => addProduct3rdFormDefaultValues;

function mapStateToProps(state) {
  return {
    open: addOpen3rdSelector(state),
    loading: loadingSelector(state),
    loadError: loadErrorSelector(state),
    productName: productNameSelector(state),
    newProductRetailer: newProductRetailerSelector(state),
  };
}

const mapDispatchToProps = {
  closeAddProduct,
  openAddProduct4,
};

export default connect(mapStateToProps, mapDispatchToProps)(withFormik({
  mapPropsToValues,
  handleSubmit: (values, {props, resetForm}) => {
    const {openAddProduct4} = props;

    openAddProduct4(values);

    resetForm();
  },
  validationSchema: addProduct3rdFormValidationSchema,
})(
  withStyles(styles)(AddProduct3rdModal)
));
