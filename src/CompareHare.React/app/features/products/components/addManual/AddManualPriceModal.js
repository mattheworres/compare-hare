import React from 'react';
import {connect} from 'react-redux';
import {
  Modal,
  withStyles,
} from '@material-ui/core';
import autobind from 'class-autobind';
import AddManualPriceForm from './AddManualPriceForm';
import {
  addManualOpenSelector,
  loadingSelector,
  saveErrorSelector,
  dateCheckSelector,
  trackedProductRetailerIdSelector,
  trackedProductIdSelector
} from '../../selectors/addManual';
import {closeAddManual, checkManualDate, saveManual} from '../../actions/addManual';
import {loadProductCurrent} from '../../actions/productDisplay';
import {addManualFormDefaultValues, addManualFormValidationSchema} from '../../validations/AddManualValidations';
import {withFormik} from 'formik';
import PropTypes from 'prop-types';

const styles = () => ({
  modal: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
  },
})

class AddManualPriceModal extends React.PureComponent {
  constructor(props) {
    super(props);

    autobind(this);
  }

  onClose() {
    const {closeAddManual, resetForm} = this.props;
    resetForm();
    closeAddManual();
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
      checkManualDate,
      dateCheck,
      trackedProductRetailerId,
      saveError
    } = this.props;

    return (
      <Modal open={open} onClose={this.onClose} className={classes.modal} >
        <AddManualPriceForm
          values={values}
          errors={errors}
          saveError={saveError}
          touched={touched}
          trackedProductRetailerId={trackedProductRetailerId}
          checkManualDate={checkManualDate}
          dateCheck={dateCheck}
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

AddManualPriceModal.propTypes = {
  classes: PropTypes.object.isRequired,
}

const mapPropsToValues = () => addManualFormDefaultValues;

function mapStateToProps(state) {
  return {
    open: addManualOpenSelector(state),
    loading: loadingSelector(state),
    saveError: saveErrorSelector(state),
    dateCheck: dateCheckSelector(state),
    trackedProductRetailerId: trackedProductRetailerIdSelector(state),
    trackedProductId: trackedProductIdSelector(state)
  };
}

const mapDispatchToProps = {
  closeAddManual,
  checkManualDate,
  saveManual,
  loadProductCurrent
};

export default connect(mapStateToProps, mapDispatchToProps)(withFormik({
  mapPropsToValues,
  handleSubmit: (values, {props, resetForm}) => {
    const {
      trackedProductId,
      trackedProductRetailerId,
      saveManual,
      closeAddManual,
      loadProductCurrent
    } = props;

    saveManual(trackedProductRetailerId, values, () => {
      loadProductCurrent(trackedProductId);
      closeAddManual();
    });

    resetForm();
  },
  validationSchema: addManualFormValidationSchema,
})(
  withStyles(styles)(AddManualPriceModal)
));
