import React from 'react';
import {connect} from 'react-redux';
import {addOpen3rdSelector, alertModelSelector} from '../../selectors/paPower';
import {closeAddPaPower} from '../../actions/paPower';
import {saveAlert} from '../../actions/addAlert';
import {Modal, withStyles} from '@material-ui/core';
import {withFormik} from 'formik';
import {
  paPower3rdFormDefaultValues,
  paPower3rdFormValidationSchema as validationSchema
} from '../../validations';
import autobind from 'class-autobind';
import {PaPowerAlert3rdForm} from './index';

const styles = () => ({
  modal: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    overflow: 'scroll',
  },
});

class AddPaPowerAlert3rdModal extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  componentDidMount() {
    this.props.resetForm();
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
      alertModel,
    } = this.props;

    return (
      <Modal open={open} onClose={this.onClose} className={classes.modal}>
        <PaPowerAlert3rdForm
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
          alertModel={alertModel} />
      </Modal>
    );
  }
}

const mapPropsToValues = () => paPower3rdFormDefaultValues;

function mapStateToProps(state) {
  return {
    open: addOpen3rdSelector(state),
    alertModel: alertModelSelector(state),
  };
}

const mapDispatchToProps = {
  closeAddPaPower,
  saveAlert,
};

export default connect(mapStateToProps, mapDispatchToProps)(withFormik({
  mapPropsToValues,
  handleSubmit: (values, {props, resetForm}) => {
    const {closeAddPaPower, saveAlert, alertModel} = props;

    alertModel.comments = values.comments;

    saveAlert(alertModel);
    closeAddPaPower();
    resetForm();
  },
  validationSchema,
})(
  withStyles(styles)(AddPaPowerAlert3rdModal)
));
