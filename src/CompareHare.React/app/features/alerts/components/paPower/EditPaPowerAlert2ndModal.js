import React from 'react';
import {connect} from 'react-redux';
import {editOpen2ndSelector} from '../../selectors/paPower';
import {closePaPower, openEditPaPower3} from '../../actions/paPower';
import {Modal, withStyles} from '@material-ui/core';
import {withFormik} from 'formik';
import {
  paPower2ndFormDefaultValues,
  paPower2ndFormValidationSchema as validationSchema
} from '../../validations';
import autobind from 'class-autobind';
import {PaPowerAlert2ndForm} from './index';

const styles = () => ({
  modal: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center'
  },
});

class EditPaPowerAlert2ndModal extends React.Component {
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
    } = this.props;

    return (
      <Modal open={open} onClose={this.onClose} className={classes.modal}>
        <PaPowerAlert2ndForm
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

const mapPropsToValues = () => paPower2ndFormDefaultValues;

function mapStateToProps(state) {
  return {
    open: editOpen2ndSelector(state),
  };
}

const mapDispatchToProps = {
  closePaPower,
  openEditPaPower3,
};

export default connect(mapStateToProps, mapDispatchToProps)(withFormik({
  mapPropsToValues,
  handleSubmit: ({props, resetForm}) => {
    const {openEditPaPower3} = props;

    openEditPaPower3();
    resetForm();
  },
  validationSchema,
})(
  withStyles(styles)(EditPaPowerAlert2ndModal)
));
