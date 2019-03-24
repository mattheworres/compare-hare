import React from 'react';
import {connect} from 'react-redux';
import {addOpen1stSelector} from '../../selectors/paPower';
import {closeAddPaPower, openAddPaPower2} from '../../actions/paPower';
import {openAddAlert} from '../../actions/addAlert';
import {Modal, withStyles} from '@material-ui/core';
import {withFormik} from 'formik';
import {
  paPower1stFormDefaultValues,
  paPower1stFormValidationSchema as validationSchema
} from '../../validations';
import autobind from 'class-autobind';
import {PaPowerAlert1stForm} from './index';
import PropTypes from 'prop-types';

const styles = () => ({
  modal: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center'
  },
});

class AddPaPowerAlert1stModal extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  onClose() {
    const {closeAddPaPower, resetForm} = this.props;

    resetForm();
    closeAddPaPower();
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
    } = this.props;

    return (
      <Modal open={open} onClose={this.onClose} className={classes.modal}>
        <PaPowerAlert1stForm
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
          values={values} />
      </Modal>
    );
  }
}

AddPaPowerAlert1stModal.propTypes = {
  classes: PropTypes.object.isRequired,
}

const mapPropsToValues = () => paPower1stFormDefaultValues;

function mapStateToProps(state) {
  return {
    open: addOpen1stSelector(state),
  };
}

const mapDispatchToProps = {
  closeAddPaPower,
  openAddAlert,
  openAddPaPower2,
};

export default connect(mapStateToProps, mapDispatchToProps)(withFormik({
  mapPropsToValues,
  handleSubmit: (values, {props, resetForm}) => {
    const {openAddPaPower2} = props;

    openAddPaPower2(values);

    resetForm();
  },
  validationSchema,
})(
  withStyles(styles)(AddPaPowerAlert1stModal)
));
