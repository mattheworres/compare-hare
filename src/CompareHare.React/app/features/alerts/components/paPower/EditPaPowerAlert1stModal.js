import React from 'react';
import {connect} from 'react-redux';
import {editOpen1stSelector} from '../../selectors/paPower';
import {closePaPower, openEditPaPower2} from '../../actions/paPower';
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

class EditPaPowerAlert1stModal extends React.Component {
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

EditPaPowerAlert1stModal.propTypes = {
  classes: PropTypes.object.isRequired,
}

const mapPropsToValues = () => paPower1stFormDefaultValues;

function mapStateToProps(state) {
  return {
    open: editOpen1stSelector(state),
  };
}

const mapDispatchToProps = {
  closePaPower,
  openEditPaPower2,
};

export default connect(mapStateToProps, mapDispatchToProps)(withFormik({
  mapPropsToValues,
  handleSubmit: ({props, resetForm}) => {
    const {openEditPaPower2} = props;

    openEditPaPower2();

    resetForm();
  },
  validationSchema,
})(
  withStyles(styles)(EditPaPowerAlert1stModal)
));
