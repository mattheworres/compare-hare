import React from 'react';
import {connect} from 'react-redux';
import {addOpenSetupSelector, distributorsSelector} from '../../selectors/paPower';
import {closePaPower, openAddPaPower, paPowerDistributorChanged} from '../../actions/paPower';
import {openAddAlert} from '../../actions/addAlert';
import {Modal, withStyles} from '@material-ui/core';
import {withFormik} from 'formik';
import {
  paPowerSetupFormDefaultValues,
  paPowerSetupFormValidationSchema as validationSchema
} from '../../validations';
import autobind from 'class-autobind';
import {PaPowerAlertSetupForm} from './index';
import PropTypes from 'prop-types';
import distributorRatesSelector from '../../selectors/paPower/distributorRatesSelector';

const styles = () => ({
  modal: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center'
  },
});

class AddPaPowerAlertSetupModal extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  onClose() {
    const {closePaPower, resetForm} = this.props;

    resetForm();
    closePaPower();
  }

  render() {
    const {
      open,
      errors,
      handleBlur,
      handleChange,
      handleSubmit,
      isSubmitting,
      isValid,
      setFieldValue,
      touched,
      values,
      classes,
      distributors,
      paPowerDistributorChanged,
      distributorRates,
    } = this.props;

    return (
      <Modal open={open} onClose={this.onClose} className={classes.modal}>
        <PaPowerAlertSetupForm
          isNew
          onClose={this.onClose}
          errors={errors}
          handleBlur={handleBlur}
          handleChange={handleChange}
          handleSubmit={handleSubmit}
          isSubmitting={isSubmitting}
          isValid={isValid}
          setFieldValue={setFieldValue}
          touched={touched}
          values={values}
          distributors={distributors}
          distributorRates={distributorRates}
          onDistributorChange={paPowerDistributorChanged} />
      </Modal>
    );
  }
}

AddPaPowerAlertSetupModal.propTypes = {
  classes: PropTypes.object.isRequired,
}

const mapPropsToValues = () => paPowerSetupFormDefaultValues;

function mapStateToProps(state) {
  return {
    open: addOpenSetupSelector(state),
    distributors: distributorsSelector(state),
    distributorRates: distributorRatesSelector(state)
  };
}

const mapDispatchToProps = {
  closePaPower,
  openAddAlert,
  openAddPaPower,
  paPowerDistributorChanged
};

export default connect(mapStateToProps, mapDispatchToProps)(withFormik({
  mapPropsToValues,
  handleSubmit: (values, {props, resetForm}) => {
    const {openAddPaPower} = props;

    openAddPaPower(values);

    resetForm();
  },
  validationSchema,
})(
  withStyles(styles)(AddPaPowerAlertSetupModal)
));
