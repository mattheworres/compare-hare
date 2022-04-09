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
    width: '600px',
  },
})

const initialState = {
  name: null,
  retailer: null,
  touched: false,
};

class AddProduct1stModal extends React.Component {
  constructor(props) {
    super(props);

    this.state = initialState;

    autobind(this);
  }

  handleFieldChange(event) {
    const {name, value} = event.target;

    this.setState({[name]: value, touched: true});
  }

  handleSubmit(event) {
    event.preventDefault();

    const {
      closeAddProduct,
      openAddPaPower,
      initializePaPower,
    } = this.props;

    const {
      zip,
      utilityType,
      stateCode,
    } = this.state;

    closeAddProduct();

    switch(this.state.stateCode) {
      case 1:
        initializePaPower({zip, utilityType, utilityState: stateCode});
        openAddPaPower();
        break;
    }

    this.setState(initialState);
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
      <Modal open={open} onClose={this.onClose} className={classes.modal}>
        <AddProduct1stForm
          values={values}
          touched={touched}
          isSubmitting={isSubmitting}
          isValid={isValid}
          errors={errors}
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
  addProduct1stFormValidationSchema,
})(
  withStyles(styles)(AddProduct1stModal)
));
