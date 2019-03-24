import React from 'react';
import {connect} from 'react-redux';
import {
  Modal,
  withStyles,
} from '@material-ui/core';
import {zipLookup} from '../services';
import autobind from 'class-autobind';
import {AddAlertForm} from './index';
import {addOpenSelector} from '../selectors/addAlert';
import {closeAddAlert} from '../actions/addAlert';
import {openAddPaPower, initializeAddPaPower} from '../actions/paPower';

const styles = () => ({
  modal: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center'
  },
})

const initialState = {
  zip: null,
  utilityType: 1,
  utility: 'Power',
  state: null,
  stateCode: null,
  touched: false,
};

class AddAlertModal extends React.Component {
  constructor(props) {
    super(props);

    this.state = initialState;

    autobind(this);
  }

  handleFieldChange(event) {
    const {name, value} = event.target;

    this.setState({[name]: value, touched: true});
  }

  handleZipChange(event) {
    const {value} = event.target;

    this.setState({zip: value});

    if (value && value.length === 5) {
      const zipValue = zipLookup(value);

      this.setState({state: zipValue.state, stateCode: zipValue.stateCode, touched: true});
    } else {
      this.setState({state: null, stateCode: null, touched: false});
    }
  }

  handleSubmit(event) {
    event.preventDefault();

    const {
      closeAddAlert,
      openAddPaPower,
      initializeAddPaPower,
    } = this.props;

    const {
      zip,
      utilityType,
      stateCode,
    } = this.state;

    closeAddAlert();

    switch(this.state.stateCode) {
      case 1:
        initializeAddPaPower({zip, utilityType, utilityState: stateCode});
        openAddPaPower();
        break;
    }

    this.setState(initialState);
  }

  onClose() {
    this.props.closeAddAlert();
  }

  render() {
    const {classes, open} = this.props;

    return (
      <Modal open={open} onClose={this.onClose} className={classes.modal}>
        <AddAlertForm
          state={this.state}
          onZipChange={this.handleZipChange}
          onSubmit={this.handleSubmit}
          onClose={this.onClose} />
      </Modal>
    );
  }
}

function mapStateToProps(state) {
  return {
    open: addOpenSelector(state),
  };
}

const mapDispatchToProps = {
  closeAddAlert,
  initializeAddPaPower,
  openAddPaPower,
};

export default withStyles(styles)(connect(mapStateToProps, mapDispatchToProps)(AddAlertModal));
