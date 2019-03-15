import React from 'react';
import PropTypes from 'prop-types';
import {
  Typography,
  Button,
  TextField,
  FormControl,
  InputLabel,
  Input,
  Select,
  MenuItem,
  withStyles,
  Grid,
} from '@material-ui/core';
import {NavigateNext} from '@material-ui/icons';
import autobind from 'class-autobind';

const styles = theme => ({
  paper: {
    top: '30%',
    margin: 'auto',
    transform: 'translate(0 30%)',
    width: theme.spacing.unit * 51,
    backgroundColor: theme.palette.background.paper,
    boxShadow: theme.shadows[5],
    padding: theme.spacing.unit * 4,
    outline: 'none',
  },
  rightAligned: {
    textAlign: 'right',
  }
})

class AddAlertForm extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  renderState() {
    const {state} = this.props.state;
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
    const {stateCode, state, utility, touched} = this.props.state;
    const hasCode = stateCode !== null;

    return !hasCode && touched ? (
      <div>
        <Typography variant="h5" component="h4" color="error">
          Sorry - I can&apos;t help you.
        </Typography>
        <Typography component="p" color="error">
          Right now CompareHare doesn&apos;t have the right tools in the tool
          box to handle{' '}
          <strong>
            {state} {utility}
          </strong>{' '}
          alerts - <a href="https://twitter.com/mattheworres" target="_blank">suggest it</a> :-)
        </Typography>
      </div>
    ) : null;
  }

  renderButton() {
    const {onSubmit, onClose, state, classes} = this.props;
    const {stateCode, touched} = state;
    const hasCode = stateCode !== null;

    return hasCode || !touched ? (
      <Grid container>
        <Grid item xs={7}>
          <Button
            type="button"
            variant="outlined"
            onClick={onClose}>
            Cancel
          </Button>
        </Grid>
        <Grid item xs={5} className={classes.rightAligned}>
          <Button
            type="submit"
            variant="contained"
            color="primary"
            onClick={onSubmit}
            disabled={!hasCode}>
              Get Started!
              <NavigateNext />
          </Button>
        </Grid>
      </Grid>
    ) : null;
  }

  render() {
    const {classes, onZipChange} = this.props;
    return (
      <form className={classes.paper}>
        <Typography variant="h5">
          Add A New Alert
        </Typography>
        <Typography>
          Let&apos;s start with where you live and what type of utility do you want to setup an alert for.
        </Typography>
        <FormControl margin="normal" required fullWidth>
          <InputLabel htmlFor="zip">ZIP Code</InputLabel>
          <Input
            id="zip"
            name="zip"
            autoComplete="postal-code"
            autoFocus
            maxLength="5"
            onChange={onZipChange}
          />
        </FormControl>
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
        {this.renderState()}
        {this.renderStateCode()}
        {this.renderButton()}
      </form>
    )
  }
}

AddAlertForm.propTypes = {
  state: PropTypes.object.isRequired,
  onZipChange: PropTypes.func.isRequired,
  onSubmit: PropTypes.func.isRequired,
  onClose: PropTypes.func.isRequired,
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(AddAlertForm);
