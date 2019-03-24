import React from 'react';
import {connect} from 'react-redux';
import {addOpen2ndSelector} from '../../selectors/paPower';
import {closeAddPaPower, openAddPaPower3} from '../../actions/paPower';
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

class AddPaPowerAlert2ndModal extends React.Component {
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
    open: addOpen2ndSelector(state),
  };
}

const mapDispatchToProps = {
  closeAddPaPower,
  openAddPaPower3,
};

export default connect(mapStateToProps, mapDispatchToProps)(withFormik({
  mapPropsToValues,
  handleSubmit: (values, {props, resetForm}) => {
    const {openAddPaPower3} = props;

    openAddPaPower3(values);
    resetForm();
  },
  validationSchema,
})(
  withStyles(styles)(AddPaPowerAlert2ndModal)
));
