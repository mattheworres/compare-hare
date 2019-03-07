import React from 'react';
import {connect} from 'react-redux';
import {
  Modal,
  FormControl,
  InputLabel,
  Input,
  Select,
  MenuItem,
  TextField,
  Paper,
  Typography,
  Button,
  withStyles,
} from '@material-ui/core';
import {zipLookup} from '../services';
import autobind from 'class-autobind';
import {NavigateNext} from '@material-ui/icons';

const styles = theme => ({
  paper: {
    width: theme.spacing.unit * 51,
    backgroundColor: theme.palette.background.paper,
    boxShadow: theme.shadows[5],
    padding: theme.spacing.unit * 4,
    outline: 'none',
  }
})

class AddAlertModal extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      zip: null,
      utilityType: 1,
      utility: 'Power',
      state: null,
      stateCode: null,
      touched: false,
    };

    autobind(this);
  }

  getModalStyle() {
    const top = 25;

    return {
      top: `${top}%`,
      margin: 'auto'
    };
  }

  handleFieldChange(event) {
    const {name, value} = event.target;

    this.setState({[name]: value, touched: true});
  }

  handleZipChange(event) {
    const {value} = event.target;
    console.log('zip change', event, event.target);

    this.setState({zip: value});

    if (value && value.length > 0) {
      const zipValue = zipLookup(value);

      this.setState({state: zipValue.state, stateCode: zipValue.stateCode});
    } else {
      this.setState({state: null, stateCode: null});
    }
    this.setState({touched: true});
  }

  onSubmit() {
    //TODO: Here is the branching logic where we'd set state to:
    //1 hide this modal
    //2 show the next modal corresponding to the specific utility and state
    console.log('Go baby go!');
  }

  renderState() {
    const {state} = this.state;
    const hasState = state && state.length > 0;

    return hasState ? (
      <TextField
        label="State"
        value={state}
        inputProps={{readOnly: true}}
        margin="normal"
        fullWidth
      />
    ) : null;
  }

  renderStateCode() {
    const {stateCode, state, utility, touched} = this.state;
    const hasCode = stateCode !== null;

    return !hasCode && touched ? (
      <Paper>
        <Typography variant="h5" component="h4">
          Sorry - I can&apos;t help you right now...
        </Typography>
        <Typography component="p">
          Right now CompareHare doesn&apos;t have the right tools in the tool
          box to handle{' '}
          <strong>
            {state} {utility}
          </strong>{' '}
          alerts.
        </Typography>
      </Paper>
    ) : null;
  }

  renderButton() {
    const {stateCode} = this.state;
    const hasCode = stateCode !== null;

    return (
      <FormControl margin="normal">
        <Button
          type="submit"
          variant="contained"
          color="primary"
          //className={classes.next}
          onClick={this.onSubmit}
          disabled={!hasCode}
        >
          Get Started!
        <NavigateNext />
        </Button>
      </FormControl>
    );
  }

  render() {
    const {classes} = this.props;
    return (
      <Modal open>
        <form style={this.getModalStyle()} className={classes.paper}>
          <Typography variant="h5">
            Add New Alert
          </Typography>
          <Typography>
            Let&apos;s start with where you live and what type of utility do you want to setup an alert for.
          </Typography>
          <FormControl margin="normal" required fullWidth>
            <InputLabel htmlFor="utilityType">Utility Type</InputLabel>
            <Select
              value={1}
              inputProps={{
                name: 'utilityType',
                id: 'utilityType',
              }}
            >
              <MenuItem value={1}>Power</MenuItem>
              <MenuItem value={2} disabled>
                Gas
              </MenuItem>
            </Select>
          </FormControl>
          <FormControl margin="normal" required fullWidth>
            <InputLabel htmlFor="zip">ZIP Code</InputLabel>
            <Input
              id="zip"
              name="zip"
              autoComplete="postal-code"
              autoFocus
              onChange={this.handleZipChange}
            />
          </FormControl>
          {this.renderState()}
          {this.renderStateCode()}
          {this.renderButton()}
        </form>
      </Modal>
    );
  }
}

function mapStateToProps() {
  return {

  };
}

const mapDispatchToProps = {

};

export default withStyles(styles)(connect(mapStateToProps, mapDispatchToProps)(AddAlertModal));
