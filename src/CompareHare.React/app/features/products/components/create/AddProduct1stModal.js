import React from 'react';
import {connect} from 'react-redux';
import {
  Modal,
  withStyles,
} from '@material-ui/core';
import autobind from 'class-autobind';
import AddProduct1stForm from './AddProduct1stForm';
import {addOpen1stSelector, loadingSelector, loadErrorSelector } from '../../selectors/addProduct';
import {closeAddProduct, openAddProduct2} from '../../actions/addProduct';
import {addProduct1stFormDefaultValues, addProduct1stFormValidationSchema} from '../../validations/AddProductValidations';
import {withFormik} from 'formik';
import PropTypes from 'prop-types';

const styles = () => ({
  modal: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
  },
})

class AddProduct1stModal extends React.PureComponent {
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
      isValid 
    } = this.props;

    return (
      <Modal open={open} onClose={this.onClose} className={classes.modal} >
        <AddProduct1stForm
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

AddProduct1stModal.propTypes = {
  classes: PropTypes.object.isRequired,
}

const mapPropsToValues = () => addProduct1stFormDefaultValues;

function mapStateToProps(state) {
  return {
    open: addOpen1stSelector(state),
    loading: loadingSelector(state),
    loadError: loadErrorSelector(state)
  };
}

const mapDispatchToProps = {
  closeAddProduct,
  openAddProduct2,
};

export default connect(mapStateToProps, mapDispatchToProps)(withFormik({
  mapPropsToValues,
  handleSubmit: (values, {props, resetForm}) => {
    const {openAddProduct2} = props;

    openAddProduct2(values);

    resetForm();
  },
  validationSchema: addProduct1stFormValidationSchema,
})(
  withStyles(styles)(AddProduct1stModal)
));
