import React from 'react';
import PropTypes from 'prop-types';

export default class ConfirmModal extends React.Component {
  constructor(props) {
    super(props);

    this.handleCancel = this.handleCancel.bind(this);
    this.handleConfirm = this.handleConfirm.bind(this);
  }

  handleCancel() {
    this.props.onDestroy({result: false});
  }

  handleConfirm() {
    this.props.onDestroy({result: true});
  }

  render() {
    const {onDestroy, ...modalProps} = this.props; // eslint-disable-line no-unused-vars

    return <div>Uh, confirm modal? Maybe this goes away now with MUI</div>;
  }
}

ConfirmModal.propTypes = {
  onDestroy: PropTypes.func,
};
