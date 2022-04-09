import React from 'react';
import {connect} from 'react-redux';
import {
  Modal,
  withStyles,
} from '@material-ui/core';
import autobind from 'class-autobind';
import AddProduct2ndForm from './AddProduct2ndForm';
import {addOpen2ndSelector, loadingSelector, loadErrorSelector, productNameSelector, productRetailerOptionsSelector } from '../../selectors/addProduct';
import {closeAddProduct, openAddProduct3} from '../../actions/addProduct';
import {addProduct2ndFormDefaultValues, addProduct2ndFormValidationSchema} from '../../validations/AddProductValidations';
import {withFormik} from 'formik';
import PropTypes from 'prop-types';

const styles = () => ({
  modal: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
  },
});

class AddProduct2ndModal extends React.PureComponent {
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
      productRetailerOptions
    } = this.props;

    return (
      <Modal open={open} onClose={this.onClose} className={classes.modal} >
        <AddProduct2ndForm
          productName={productName}
          productRetailerOptions={productRetailerOptions}
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

AddProduct2ndModal.propTypes = {
  classes: PropTypes.object.isRequired,
}

const mapPropsToValues = () => addProduct2ndFormDefaultValues;

function mapStateToProps(state) {
  return {
    open: addOpen2ndSelector(state),
    loading: loadingSelector(state),
    loadError: loadErrorSelector(state),
    productName: productNameSelector(state),
    productRetailerOptions: productRetailerOptionsSelector(state),
  };
}

const mapDispatchToProps = {
  closeAddProduct,
  openAddProduct3,
};

export default connect(mapStateToProps, mapDispatchToProps)(withFormik({
  mapPropsToValues,
  handleSubmit: (values, {props, resetForm}) => {
    const {openAddProduct3} = props;

    openAddProduct3(values);

    resetForm();
  },
  validationSchema: addProduct2ndFormValidationSchema,
})(
  withStyles(styles)(AddProduct2ndModal)
));
