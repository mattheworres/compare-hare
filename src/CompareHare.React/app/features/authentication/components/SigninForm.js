import React from 'react';
import {Map} from 'immutable';
import PropTypes from 'prop-types';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import FormControl from '@material-ui/core/FormControl';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import {
  withStyles,
  Checkbox,
  Input,
  InputLabel,
  Paper,
  Typography,
  CircularProgress,
} from '@material-ui/core';
import autobind from 'class-autobind';

const styles = theme => ({
  paper: {
    marginTop: theme.spacing.unit * 8,
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    padding: `${theme.spacing.unit * 2}px ${theme.spacing.unit * 3}px ${theme
      .spacing.unit * 3}px`,
  },
  avatar: {
    margin: theme.spacing.unit,
    backgroundColor: theme.palette.secondary.main,
  },
  form: {
    width: '100%', // Fix IE 11 issue.
    marginTop: theme.spacing.unit,
  },
  submit: {
    marginTop: theme.spacing.unit * 3,
  },
});

class SigninForm extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  hasValidationErrors(fieldName) {
    const {validationErrors} = this.props;
    return (
      validationErrors[fieldName] != null &&
      validationErrors[fieldName].length > 0
    );
  }

  getValidationErrorText(fieldName) {
    const {validationErrors} = this.props;
    const hasError = this.hasValidationErrors(fieldName);

    return hasError ? validationErrors[fieldName].join(' ') : '';
  }

  renderButton() {
    const {classes, onSubmit, submitting} = this.props;
    const text = submitting
      ? <CircularProgress color="secondary" />
      : "Sign In";

    return (
      <Button
        type="submit"
        fullWidth
        variant="contained"
        color="primary"
        className={classes.submit}
        onClick={onSubmit}
        disabled={submitting}
      >
        {text}
          </Button>
    )
  }

  render() {
    const {classes, onFieldChange} = this.props;

    return (
      <Paper className={classes.paper}>
        <Avatar className={classes.avatar}>
          <LockOutlinedIcon />
        </Avatar>
        <Typography component="h1" variant="h5">
          CompareHare (Alpha)
        </Typography>
        <Typography component="h1" variant="h4">
          Sign in
        </Typography>
        <form className={classes.form}>
          <FormControl margin="normal" required fullWidth>
            <InputLabel htmlFor="email">Email Address</InputLabel>
            <Input
              id="email"
              name="email"
              autoComplete="email"
              autoFocus
              error={this.hasValidationErrors('email')}
              onChange={onFieldChange}
            />
          </FormControl>
          <FormControl margin="normal" required fullWidth>
            <InputLabel htmlFor="password">Password</InputLabel>
            <Input
              name="password"
              type="password"
              id="password"
              error={this.hasValidationErrors('password')}
              autoComplete="current-password"
              onChange={onFieldChange}
            />
          </FormControl>
          <FormControlLabel
            control={
              <Checkbox
                value="remember"
                color="primary"
                onChange={onFieldChange}
              />
            }
            label="Remember me"
          />
          {this.renderButton()}
        </form>
      </Paper>
    );
  }
}

SigninForm.propTypes = {
  disabled: PropTypes.bool,
  validationErrors: PropTypes.object.isRequired,
  onFieldChange: PropTypes.func.isRequired,
  onSubmit: PropTypes.func.isRequired,
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(SigninForm);
