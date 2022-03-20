import React from 'react';
import {connect} from 'react-redux';
import {editOpen3rdSelector, alertModelSelector} from '../../selectors/paPower';
import {closePaPower} from '../../actions/paPower';
import {updateAlert} from '../../actions/editAlert';
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

class EditPaPowerAlert3rdModal extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  componentDidMount() {
    this.props.resetForm();
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
    open: editOpen3rdSelector(state),
    alertModel: alertModelSelector(state),
  };
}

const mapDispatchToProps = {
  closePaPower,
  updateAlert,
};

export default connect(mapStateToProps, mapDispatchToProps)(withFormik({
  mapPropsToValues,
  handleSubmit: (values, {props, resetForm}) => {
    const {closePaPower, updateAlert, alertModel} = props;

    alertModel.comments = values.comments;

    updateAlert(alertModel);
    closePaPower();
    resetForm();
  },
  validationSchema,
})(
  withStyles(styles)(EditPaPowerAlert3rdModal)
));
